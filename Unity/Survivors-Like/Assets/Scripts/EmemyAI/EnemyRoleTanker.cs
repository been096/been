using UnityEngine;

/// <summary>
/// ��Ŀ ���� ����.
/// </summary>
public class EnemyRoleTanker : MonoBehaviour
{
    public EnemyCore core;
    public Health health;

    public float speed = 1.4f;
    public int hp = 80;
    public float knockbackResistance = 0.7f; // 0~1 (1�� �������� ���� �� �и�)

    void Awake()
    {
        if (core == null)
        {
            core = GetComponent<EnemyCore>();
        }

        if (health == null)
        {
            health = GetComponent<Health>();
        }

        if (core != null)
        {
            core.moveSpeed = speed;
            core.externalSpeedMultiplier = 1f;
            //core.knockbackResistance = knockbackResistance;
        }
        if (health != null)
        {
            health.currentHP = hp;
            health.maxHP = hp;
        }
    }
}