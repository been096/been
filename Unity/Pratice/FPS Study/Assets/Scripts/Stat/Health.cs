using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �⺻ ü��/��� ����. IDamageable ����ü.
/// </summary>
public class Health : MonoBehaviour, IDamagealbe
{
    [Header("Health")]
    public float maxHealth = 1000.0f;           // �ִ� ü��.
    public bool destroyonDeath = true;          // ��� �� ������Ʈ �ı����� ����.
    public UnityEvent onDeath;                  // ��� �̺�Ʈ(��ƼŬ/��� ��)

    [Header("Optional Feedback")]
    public ParticleSystem hitVfxPrefab;         // �ǰݽ� ��/���� ��.
    public AudioSource audioSource;             // �ǰ� ����(����)
    public AudioClip hurtClip;                  // �ǰ� ȿ����(����)

    private float currentHealth;                // ���� ü��.

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(float amount, Vector3 hitPoint, Vector3 hitNormal, Transform source)
    {
        float dmg = amount;
        if (dmg < 0.0f)
        {
            dmg = 0.0f;
        }

        currentHealth = currentHealth - dmg;

        if (hitVfxPrefab != null)
        {
            Quaternion rot = Quaternion.LookRotation(Vector3.ProjectOnPlane(hitNormal, Vector3.up), hitNormal);
            ParticleSystem vfx = Instantiate(hitVfxPrefab, hitPoint + hitNormal * 0.01f, rot);
        }
        if (audioSource != null)
        {
            if (hurtClip != null)
            {
                audioSource.PlayOneShot(hurtClip, 1.0f);
            }
        }

        if (currentHealth <= 0.0f)
        {
            if (onDeath != null)
            {
                onDeath.Invoke();
            }

            if (destroyonDeath == true)
            {
                Destroy(gameObject);
            }
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
