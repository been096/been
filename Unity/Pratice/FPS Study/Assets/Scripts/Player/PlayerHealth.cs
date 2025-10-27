using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �÷��̾�� ü�� ����.
/// �÷��̾ IDamageable�� �����ؾ� ������ ���´�.
/// </summary>
public class PlayerHealth : MonoBehaviour, IDamagealbe
{
    public float maxHealth = 100.0f;        // �ִ� ü��.
    public UnityEvent onDeath;              // ��� �̺�Ʈ(��Ʈ����/������ ���� ����)

    private float currentHealth;            // ���� ü��.

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(float amount, Vector3 hitPoint, Vector3 hitNormal, Transform source)
    {
        Debug.Log("Player is Attacked!!!!!!");
        // ���� ���
        float dmg = amount;
        if (dmg < 0.0f)
        {
            dmg = 0.0f;
        }

        currentHealth -= dmg;

        // ���� : UI/��ó���� ���� ���ǿ��� Ȯ��.
        if (currentHealth <= 0.0f)
        {
            if (onDeath != null)
            {
                onDeath.Invoke();
            }
            Debug.Log("Player Dead!!!");
        }
    }

    public float GetCurrentHealth()
    {
        float v = currentHealth;
        return v;
    }
}
