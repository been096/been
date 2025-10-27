using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 플레이어용 체력 구현.
/// 플레이어도 IDamageable을 구현해야 공격이 들어온다.
/// </summary>
public class PlayerHealth : MonoBehaviour, IDamagealbe
{
    public float maxHealth = 100.0f;        // 최대 체력.
    public UnityEvent onDeath;              // 사망 이벤트(리트라이/리스폰 연동 가능)

    private float currentHealth;            // 현재 체력.

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(float amount, Vector3 hitPoint, Vector3 hitNormal, Transform source)
    {
        Debug.Log("Player is Attacked!!!!!!");
        // 음수 방어
        float dmg = amount;
        if (dmg < 0.0f)
        {
            dmg = 0.0f;
        }

        currentHealth -= dmg;

        // 데모 : UI/후처리는 추후 강의에서 확장.
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
