using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine.UIElements;

/// <summary>
/// RaycastHIt ������ �޾� ǥ�麰 ����Ʈ ����Ʈ/��Į/���带 �߻�.
/// SurfaceMaterial�� ����.
/// </summary>
public class ImpactEffectRouter : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;                 // �����(������ ����)
    public AudioClip concreteClip;
    public AudioClip dirtClip;
    public AudioClip woodClip;
    public AudioClip metalClip;
    public AudioClip waterClip;

    [Header("VFX")]
    public ParticleSystem concreteVfx;
    public ParticleSystem dirtVfx;
    public ParticleSystem woodVfx;
    public ParticleSystem metalVfx;
    public ParticleSystem waterVfx;

    [Header("Decals")]
    public DecalPool decalPool;                     // ��Į Ǯ(������ ��ŵ)
    public GameObject concreteDecal;
    public GameObject dirtDecal;
    public GameObject woodDecal;
    public GameObject metalDecal;
    public GameObject waterDecal;

    [Header("Options")]
    public float decalNormalOffset = 0.002f;        // ǥ�鿡�� ��¦ ����.
    public float vfxNormalOffset = 0.01f;           // VFX�� ǥ�鿡�� ��¦.
    public bool parentDecalToHit = true;            // �����̴� ǥ���̸� �θ�� ���̱�.

    public void SpawnImpact(RaycastHit hit)
    {
        // 1) ǥ�� Ÿ�� ����.
        SurfaceType type = SurfaceType.Contrete;
        SurfaceMaterial sm = hit.collider.GetComponent<SurfaceMaterial>();
        if (sm != null)
        {
            type = sm.SurfaceType;
        }

        // 2) �����.
        AudioClip clip = GetClip(type);
        if (audioSource != null)
        {
            if (clip != null)
            {
                audioSource.PlayOneShot(clip, 1.0f);
            }
        }

        // 3) VFX
        ParticleSystem vfx = GetVfx(type);
        if (vfx != null)
        {
            Quaternion rot = Quaternion.LookRotation(Vector3.ProjectOnPlane(hit.normal, Vector3.up), hit.normal);
            Vector3 pos = hit.point + hit.normal * vfxNormalOffset;
            Instantiate(vfx, pos, rot);
        }

        // 4) Decal (Ǯ ���)
        GameObject decalPrefab = GetDecal(type);
        if (decalPrefab != null)
        {
            if (decalPool != null)
            {
                Quaternion rot = Quaternion.LookRotation(-hit.normal, Vector3.up);
                Vector3 pos = hit.point + hit.normal * decalNormalOffset;
                GameObject decal = decalPool.Spawn(decalPrefab, pos, rot);
                if (parentDecalToHit == true)
                {
                    decal.transform.SetParent(hit.collider.transform, true);
                }
            }
        }
    }

    private AudioClip GetClip(SurfaceType t)
    {
        if (t == SurfaceType.Contrete)
        {
            return concreteClip;
        }
        if (t == SurfaceType.Dirt)
        {
            return dirtClip;
        }
        if (t == SurfaceType.Wood)
        {
            return woodClip;
        }
        if (t == SurfaceType.Metal)
        {
            return metalClip;
        }
        if (t == SurfaceType.Water)
        {
            return waterClip;
        }
        return concreteClip;
    }

    private ParticleSystem GetVfx(SurfaceType t)
    {
        if (t == SurfaceType.Concrete)
        {
            return concreteVfx;
        }
        if (t == SurfaceType.Dirt)
        {
            return dirtVfx;
        }
        if (t == SurfaceType.Wood)
        {
            return woodVfx;
        }
        if (t == SurfaceType.Metal)
        {
            return metalVfx;
        }
        if (t == SurfaceType.Water)
        {
            return waterVfx;
        }
        return concreteVfx;
    }

    private GameObject GetDecal(SurfaceType t)
    {
        if (t == SurfaceType.Concrete)
        {
            return concreteDecal;
        }
        if (t == SurfaceType.Dirt)
        {
            return dirtDecal;
        }
        if (t == SurfaceType.Wood)
        {
            return woodDecal;
        }
        if (t == SurfaceType.Metal)
        {
            return metalDecal;
        }
        if (t == SurfaceType.Water)
        {
            return waterDecal;
        }
        return concreteDecal;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
