using UnityEngine;

/// <summary>
/// ��� źȯ.
/// - '���� �浹 üũ'�� ��ü ���� : ���� ��ġ -> ���� ��ġ�� Raycast(Sweep) �� �̵�
/// -> ���� �ӵ������� �� / ���� �ݶ��̴��� �������� ����.
/// - ��Ʈ �� IDamageable�� ������ ����, ����Ʈ ����Ʈ(����)
/// - ���� �ð� �� �ڵ� �ı�.
/// </summary>
public class ProjectileBullet : MonoBehaviour
{
    [Header("Ballistics")]
    public float speed = 120.0f;                // ź �ӵ�(m/s). �ſ� ����.
    public bool useGravity = false;             // ���� ���� ����.
    public float gravity = -9.81f;              // �߷� ���ӵ�(�ɼ�)
    public float maxLifeTime = 4.0f;            // �ִ� ���� �ð�(��)
    public LayerMask hitmask;

    [Header("Damage")]
    public float damage = 20.0f;                // �⺻ ������.
    public float headshotMultiplier = 2.0f;     // Hitbox�� ������ ��.

    [Header("Effects (OPtional)")]
    public GameObject impactVfxPrefab;          // �ǰ� ���� VFX
    public GameObject decalPrefab;              // ��Į(����)
    public float decalOffset = 0.01f;           // ǥ�� �İ�� ���� ������.

    // ���� ����.
    private Vector3 velocity;                   // ���� �ӵ�(�߷� ����)
    private Vector3 lastPosition;               // ���� ������ ��ġ(���� ������)
    private float life;                         // ���� ����.

    private void Awake()
    {
        // �ʱ�ȭ�� �ܺο��� Spawn ���� SetInitialVelocity ������ �����ص� ��.
        velocity = transform.forward * speed;
        lastPosition = transform.position;
        life = 0.0f;
    }

    /// <summary>
    /// ��ó�� �߻� ���� ȣ���Ͽ� �ʱ� �ӵ��� ����(ADS/�������� �ݿ� �� ���� ��� ����)
    /// </summary>
    public void SetInitialVelocity(Vector3 v)
    {
        velocity = v;
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        // 1) �߷� ����(�ɼ�)
        if (useGravity == true)
        {
            velocity.y = velocity.y + gravity * dt;
        }

        // 2) �̵� �Ÿ� ���.
        Vector3 start = transform.position;             // ���� ��ġ(���� ���������ε� ��� ����)
        Vector3 displacement = velocity * dt;           // �̹� ������ �̵� ���� ����.
        Vector3 end = start + displacement;             // �̵� �� ��ġ(�̻���)

        // 3) ���� �浹 üũ(Sweep) : ���� ��ġ -> �̹� ��ġ '�� ��'�� ���.
        //    - FixedUpdate ����/������ ���� ������ ���� lastPosition���� end������ ����.
        Vector3 sweepStart = lastPosition;
        Vector3 sweepDir = (end - sweepStart);
        float sweepDist = sweepDir.magnitude;

        bool hitSomething = false;
        RaycastHit hitInfo = new RaycastHit();

        if (sweepDist > 0.0001f)
        {
            Vector3 dir = sweepDir / sweepDist;
            bool got = Physics.Raycast(sweepStart, dir, out hitInfo, sweepDist, hitmask,
                QueryTriggerInteraction.Ignore);
            if ( got == true)
            {
                hitSomething = true;
            }
        }

        // 4) ��Ʈ ó�� �Ǵ� �̵�.
        if (hitSomething == true)
        {
            OnHit(hitInfo);
            // ��Ʈ ��� �ı�(���� ������ �ʿ��ϸ� Ȯ��)
            Destroy(gameObject);
        }
        else
        {
            // �浹 ���� : ��ġ ����.
            transform.position = end;
            // ź�ΰ� ���� ������ �ٶ󺸵��� ȸ��(�ð�����)
            if (velocity.sqrMagnitude > 0.0001f)
            {
                Quaternion look = Quaternion.LookRotation(velocity.normalized, Vector3.up);
                transform.rotation = look;
            }
        }

        // 5) ���� �ð� �ʰ� �� �ı�.
        life = life + dt;
        if (life >= maxLifeTime)
        {
            Destroy(gameObject);
        }

        // 6) ���� ������ ���� ��ġ ���.
        lastPosition = transform.position;
    }

    private void OnHit(RaycastHit hit)
    {
        // 1) ������ �����(IDamageable)
        float finalDamage = damage;

        // Hitbox�� ������ ��Ƽ�ö��̾�.
        Hitbox hb = hit.collider.GetComponent<Hitbox>();
        if (hb != null)
        {
            finalDamage = finalDamage * hb.damageMultiplier;
            if (hb.owner != null)
            {
                hb.owner.ApplyDamage(finalDamage, hit.point, hit.normal, transform);
            }
            else
            {
                IDamagealbe id = hit.collider.GetComponentInParent<IDamagealbe>();
                if (id != null)
                {
                    id.ApplyDamage(finalDamage, hit.point, hit.normal, transform);
                }
            }
        }
        else
        {
            IDamagealbe id = hit.collider.GetComponentInParent<IDamagealbe>();
        }

        // 2) ����Ʈ VFX/��Į(����)
        if (impactVfxPrefab != null)
        {
            Quaternion rot = Quaternion.LookRotation(hit.normal);
            GameObject vfx = Instantiate(impactVfxPrefab, hit.point + hit.normal * decalOffset, rot);
        }
        if (decalPrefab != null)
        {
            Quaternion rot = Quaternion.LookRotation(-hit.normal);
            GameObject decal = Instantiate(decalPrefab, hit.point + hit.normal * decalOffset, rot);
        }

        //====================================================================
        // �� : ��ź ��󿡰� 2��¥�� 0.7�� ���ο� �ο�.
        StatusEffect_Slow slowPreset = GameObject.FindAnyObjectByType<StatusEffect_Slow>();
        if (slowPreset != null)
        {
            StatusEffectApplier.ApplyTo(hit.collider.gameObject, slowPreset);
        }
    }
}
