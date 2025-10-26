using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

/// <summary>
/// StepEventSource�� ���� ���� �̺�Ʈ�� �޾Ƽ�
/// ǥ�� Ž�� -> ����� ����/��� -> ��ƼŬ ���� -> ī�޶� ���޽� ȣ��
/// </summary>
[DisallowMultipleComponent]
public class FootstepRouter : MonoBehaviour
{
    [Header("Refs")]
    public FootstepSurfaceDetector surfaceDetector;
    public AudioSource audioSource;
    public SimpleCameraImpulse cameraImpulse;

    [Header("Audio Library")]
    public List<AudioClip> concreteClips = new List<AudioClip>();
    public List<AudioClip> dirtClips = new List<AudioClip>();
    public List<AudioClip> woodClips = new List<AudioClip>();
    public List<AudioClip> metalClips = new List<AudioClip>();
    public List<AudioClip> waterClips = new List<AudioClip>();
    public float volumeMin = 0.8f;
    public float volumMax = 1.0f;
    public float pitchMin = 0.95f;
    public float pitchMax = 1.05f;

    [Header("VFX")]
    public ParticleSystem concreteVfxPrefab;
    public ParticleSystem dirtVfxPrefab;
    public ParticleSystem woodVfxPrefab;
    public ParticleSystem metalVfxPrefab;
    public ParticleSystem waterVfxPrefab;

    [Header("VFX Options")]
    public float vfxUpOffset = 0.05f;
    public bool parentToWorld = true;

    // ==== �ܺο��� ������ ���� �̺�Ʈ ������ ====
    public void OnStepLeft()
    {
        HandleStep();
    }
    public void OnStepRight()
    {
        HandleStep();
    }

    // ==== ���� ====
    private void HandleStep()
    {
        if (surfaceDetector == null)
        {
            return;
        }

        SurfaceType type;
        Vector3 point;
        Vector3 normal;

        bool ok = surfaceDetector.TryGetSurface(out type, out point, out normal);
        if (ok == false)
        {
            // ǥ���� �� ã���� �ּ��� ������� �⺻������ ó��.
            type = SurfaceType.Concrete;
            point = transform.position;
            normal = Vector3.up;
        }

        PlayFootstepAudio(type);
        SpawnFootstepVfx(type, point, normal);
        PulseCameraImpulse();
    }

    private void PlayFootstepAudio(SurfaceType type)
    {
        Debug.Log("Audio Ground Type = " + type);

        if (audioSource == null)
        {
            return;
        }

        List<AudioClip> bank = GetClipBank(type);
        if (bank == null)
        {
            return;
        }
        if (bank.Count <= 0)
        {
            return;
        }

        // �������� �ϳ� ���� + ����/��ġ ��¦ ����.
        int idx = Random.Range(0, bank.Count);
        AudioClip clip = bank[idx];

        float vol = Random.Range(volumeMin, volumMax);
        float pit = Random.Range(pitchMin, pitchMax);

        audioSource.pitch = pit;
        audioSource.PlayOneShot(clip, vol);
    }

    private void SpawnFootstepVfx(SurfaceType type, Vector3 point, Vector3 normal)
    {
        Debug.Log("Vfx Ground Tpye = " + type);

        ParticleSystem prefab = GetVfxPrefab(type);
        if (prefab == null)
        {
            return;
        }

        Vector3 spawnPos = point + normal * vfxUpOffset;
        Quaternion spawnRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(normal, Vector3.up), normal);

        if (parentToWorld == true)
        {
            ParticleSystem vfx = Instantiate(prefab, spawnPos, spawnRot);
            // �ڵ��ı��� �����鿡 �����Ǿ� ���� �ʴٸ� ���⼭ ���� �� Destroy�� ���.
            if (vfx != null)
            {
                Destroy(vfx.gameObject, 0.5f);
            }
        }
        else
        {
            ParticleSystem vfx = Instantiate(prefab, spawnPos, spawnRot, null);
            if (vfx != null)
            {
                Destroy(vfx.gameObject, 0.5f);
            }
        }
    }

    private void PulseCameraImpulse()
    {
        if (cameraImpulse != null)
        {
            cameraImpulse.Pulse();
        }
    }

    private List<AudioClip> GetClipBank(SurfaceType type)
    {
        if (type == SurfaceType.Concrete)
        {
            return concreteClips;
        }
        if (type == SurfaceType.Dirt)
        {
            return dirtClips;
        }
        if (type == SurfaceType.Wood)
        {
            return woodClips;
        }
        if (type == SurfaceType.Metal)
        {
            return metalClips;
        }
        if (type == SurfaceType.Water)
        {
            return waterClips;
        }
        return concreteClips;
    }

    private ParticleSystem GetVfxPrefab(SurfaceType type)
    {
        if (type == SurfaceType.Concrete)
        {
            return concreteVfxPrefab;
        }
        if (type == SurfaceType.Dirt)
        {
            return dirtVfxPrefab;
        }
        if (type == SurfaceType.Wood)
        {
            return woodVfxPrefab;
        }
        if (type == SurfaceType.Metal)
        {
            return metalVfxPrefab;
        }
        if (type == SurfaceType.Water)
        {
            return waterVfxPrefab;
        }
        return concreteVfxPrefab;
    }
}
