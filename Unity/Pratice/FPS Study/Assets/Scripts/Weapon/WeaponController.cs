using UnityEngine;
using System.Collections;

/// <summary>
/// ������ �ٽ� ����: ź��, ���� ��ٿ�, ������, ADS(FOV), ��Ʈ��ĵ ���, �ݵ� �̺�Ʈ.
/// ����: Idle / Firing / Reloading / Ads(���� ���� �÷���).
/// </summary>
[DisallowMultipleComponent]
public class WeaponController : MonoBehaviour
{
    [Header("Refs")]
    public Camera playerCamera;                 // ����ĳ��Ʈ/ADS FOV ����.
    public ParticleSystem muzzleFlash;          // �ѱ� ȭ��.
    public AudioSource audioSource;             // ���� ����.
    public Transform raycastOrigin;             // ���� ����(ī�޶� �߾��� ����)
    public LayerMask hitMask;                   // ���� �� �ִ� ���̾�.
    public RecoilApplier recoil;                // �ݵ� ����(ī�޶� ű)
    public CrosshairUI crosshair;               // ũ�ν���� Ȯ�� �ݿ�.
    public AmmoHUD ammoHud;                     // ź�� ǥ�� TMP.

    [Header("Ammo")]
    public int magSize = 30;                    // źâ ũ��.
    public int reserveAmmo = 90;                // ���� ź��.
    public float reloadTime = 1.9f;             // ������ �ð�(��)
    public bool chamberedRound = true;          // ��ź �� �� ���� ���(����)

    [Header("Fire")]
    public float fireRate = 10.0f;              // �ʴ� �߻��(10 �� 0.1�� ����)
    public float damage = 20.0f;                // ��Ʈ ������(�����)
    public float maxRange = 150.0f;             // ��Ʈ��ĵ �ִ� ��Ÿ�.
    public AudioClip shotSfx;                   // �߻���.
    public AudioClip drySfx;                    // �� źâ Ŭ����.

    [Header("ADS")]
    public float adsFov = 55.0f;                // ADS �� FOV
    public float hipFov = 70.0f;                // �㸮��� FOV
    public float adsBlendTime = 0.12f;          // FOV ��ȯ �ð�(��)
    public bool applyMouseSensitivityScale = false; // (����) ���콺 ���� ���� ����.
    public float adsMouseScale = 0.8f;          // ADS �� ���� ����.

    [Header("VFX")]
    public ParticleSystem hitVfxPrefab;         // �ǰ� ����ũ/����.
    public GameObject bulletDecalPrefab;        // ��Į(����)

    public ImpactEffectRouter impactRouter;     // ǥ�� ����Ʈ �����.
    public bool applyHitboxMultiplier = true;

    // ���� ����
    private int ammoInMag;                      // ���� źâ ��ź.
    private float fireCooldown;                 // �߻� ��ٿ� Ÿ�̸�.
    private bool isReloading;                   // ������ �� ����.
    private bool fireHeld;                      // �Է�: �߻� ����.
    private bool adsHeld;                       // �Է�: ADS ����.
    private float fovVel;                       // FOV SmoothDamp �ӵ� ĳ��.

    private void Start()
    {
        ammoInMag = magSize; // ���� �� ���� ����.
        if (playerCamera != null)
        {
            playerCamera.fieldOfView = hipFov;
        }
        UpdateAmmoHud();
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        // 1) FOV(ADS) ��ȯ.
        UpdateAdsFov(dt);

        // 2) ������ ���̸� �߻� ����.
        if (isReloading == true)
        {
            // ��ٿ õõ�� ����(���ʿ��� �ܻ� ����)
            if (fireCooldown > 0.0f)
            {
                fireCooldown -= dt;
                if (fireCooldown < 0.0f)
                {
                    fireCooldown = 0.0f;
                }
            }
            return;
        }

        // 3) �߻� ��ٿ� ����.
        if (fireCooldown > 0.0f)
        {
            fireCooldown -= dt;
            if (fireCooldown < 0.0f)
            {
                fireCooldown = 0.0f;
            }
        }

        // 4) �߻� ó��(�ڵ����: ��ư ���� �� ����)
        if (fireHeld == true)
        {
            TryFire();
        }
    }

    // ===== �Է� ���ε� =====
    public void SetFireHeld(bool held)
    {
        fireHeld = held;
    }

    public void SetAdsHeld(bool held)
    {
        adsHeld = held;
    }

    public void RequestReload()
    {
        if (isReloading == true)
        {
            return;
        }
        if (ammoInMag >= magSize)
        {
            // �̹� ���� �� ������ ����.
            return;
        }
        if (reserveAmmo <= 0)
        {
            return;
        }
        StartCoroutine(CoReload());
    }

    // ===== �ٽ� ���� =====

    private void TryFire()
    {
        // 1) ��ٿ� Ȯ��.
        if (fireCooldown > 0.0f)
        {
            return;
        }

        // 2) ź�� Ȯ��.
        if (ammoInMag <= 0)
        {
            PlayDryFire();
            // ��ٿ��� �ణ �༭ Ŭ�� �ߺ� ����.
            fireCooldown = 0.2f;
            return;
        }

        // 3) 1�� �Һ�.
        --ammoInMag;
        UpdateAmmoHud();

        // 4) �߻� ��ٿ� ��.
        float interval = 1.0f / fireRate;
        fireCooldown = interval;

        // 5) ����Ʈ/����/�ݵ�.
        PlayMuzzle();
        PlayShotSfx();
        ApplyRecoilKick();

        // 6) ��Ʈ��ĵ.
        DoHitscan();

        // 7) ũ�ν���� Ȯ�� �޽�.
        if (crosshair != null)
        {
            crosshair.PulseFireSpread();
        }
    }

    private IEnumerator CoReload()
    {
        isReloading = true;

        // (����) ������ ����/�ִϸ��̼� ��.
        // audioSource.PlayOneShot(reloadSfx);

        yield return new WaitForSeconds(reloadTime);

        int needed = magSize - ammoInMag;
        if (needed < 0)
        {
            needed = 0;
        }
        int toLoad = Mathf.Min(needed, reserveAmmo);

        ammoInMag += toLoad;
        reserveAmmo -= toLoad;

        // ��ź �� �� ���� ����̸�, �� źâ���� ���ε� �� +1 ���.
        if (chamberedRound == true)
        {
            if (ammoInMag > 0)
            {
                // �̹� �� �� �����Ǿ� �ִٰ� ���� -> ��Ģ�� �°� ���� ����.
            }
        }

        UpdateAmmoHud();
        isReloading = false;
    }

    private void DoHitscan()
    {
        if (playerCamera == null)
        {
            return;
        }

        // ī�޶� ���߾ӿ��� ����ĳ��Ʈ.
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;
        bool got = Physics.Raycast(ray, out hit, maxRange, hitMask, QueryTriggerInteraction.Ignore);

        if (got == true)
        {
            
            // ������ ����.
            float finalDamage = damage;

            Hitbox hb = hit.collider.GetComponent<Hitbox>();
            if (hb != null)
            {
                Debug.Log("name = " + hb.gameObject.name);
                if (applyHitboxMultiplier == true)
                {
                    finalDamage = finalDamage * hb.damageMultiplier;
                }

                if (hb.owner != null)
                {
                    Debug.Log("Damage = " + finalDamage);
                    hb.owner.ApplyDamage(finalDamage, hit.point, hit.normal, transform);
                }
                else
                {
                    IDamagealbe id = hit.collider.GetComponentInParent<IDamagealbe>();
                    if (id != null)
                    {
                        Debug.Log("Damage = " + finalDamage);
                        id.ApplyDamage(finalDamage, hit.point, hit.normal, transform);
                    }
                }
            }
            else
            {
                IDamagealbe dmg = hit.collider.GetComponentInParent<IDamagealbe>();
                if (dmg != null)
                {
                    Debug.Log("Damage = " + finalDamage);
                    dmg.ApplyDamage(finalDamage, hit.point, hit.normal, transform);
                }
            }

            // ������ �������̽��� �ִٸ� TryGetComponent�� ó�� ����(���𿡼� VFX/��Į��)
            //SpawnHitVfx(hit.point, hit.normal);
            //SpawnDecal(hit.point, hit.normal);

            // ǥ�� ����Ʈ(�����/VFX/��Į)
            if (impactRouter != null)
            {
                impactRouter.SpawnImpact(hit);
            }
            else
            {
                // �⺻ VFX/��Į ȣ���� �־��ٸ�, ���� Router�� ��ü�ϴ� �� ����.
                SpawnHitVfx(hit.point, hit.normal);
                SpawnDecal(hit.point, hit.normal);
            }
        }
    }

    private void SpawnHitVfx(Vector3 point, Vector3 normal)
    {
        if (hitVfxPrefab == null)
        {
            return;
        }

        Quaternion rot = Quaternion.LookRotation(Vector3.ProjectOnPlane(normal, Vector3.up), normal);
        ParticleSystem vfx = Instantiate(hitVfxPrefab, point + normal * 0.01f, rot);
        // �ڵ� �ı� ������ �����տ� ���� �ʴٸ�, ���� �� Destroy �߰� ����.
    }

    private void SpawnDecal(Vector3 point, Vector3 normal) // ��Į�� ���ߵ� ����� ���뿡 ���ڱ������� ���� ȿ��. �ǰ� ����Ʈ�� �ٸ�.
    {
        if (bulletDecalPrefab == null)
        {
            return;
        }

        Quaternion rot = Quaternion.LookRotation(-normal, Vector3.up);
        GameObject decal = Instantiate(bulletDecalPrefab, point + normal * 0.002f, rot);
        // �ʿ��ϸ� �θ� ��Ʈ�� �ݶ��̴��� �����Ͽ� �̵��� ����.
    }

    private void PlayMuzzle()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
    }

    private void PlayShotSfx()
    {
        if (audioSource != null)
        {
            if (shotSfx != null)
            {
                audioSource.PlayOneShot(shotSfx, 1.0f);
            }
        }
    }

    private void PlayDryFire()
    {
        if (audioSource != null)
        {
            if (drySfx != null)
            {
                audioSource.PlayOneShot(drySfx, 1.0f);
            }
        }
    }

    private void ApplyRecoilKick()
    {
        if (recoil != null)
        {
            recoil.Kick();
        }
    }

    private void UpdateAdsFov(float dt)
    {
        if (playerCamera == null)
        {
            return;
        }

        float target = hipFov;
        if (adsHeld == true)
        {
            target = adsFov;
        }

        // SmoothDamp ��� FOV ��ȯ.
        float newFov = Mathf.SmoothDamp(playerCamera.fieldOfView, target, ref fovVel, adsBlendTime);
        playerCamera.fieldOfView = newFov;

        // (����) ���콺 ���� ����.
        //if (applyMouseSensitivityScale == true)
        //{
        //    MouseLook ml = playerCamera.GetComponentInParent<MouseLook>();
        //    if (ml != null)
        //    {
        //        if (adsHeld == true)
        //        {
        //            ml.SetSensitivityMultiplier(adsMouseScale);
        //        }
        //        else
        //        {
        //            ml.SetSensitivityMultiplier(1.0f);
        //        }
        //    }
        //}
    }

    private void UpdateAmmoHud()
    {
        if (ammoHud != null)
        {
            ammoHud.SetAmmo(ammoInMag, reserveAmmo);
        }
    }
}
