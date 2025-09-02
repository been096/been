using UnityEngine;

/// <summary>
/// ���� ���� ����.
/// </summary>
public class EnemyRoleRunner : MonoBehaviour
{
    public EnemyCore core;
    public Health health;

    public float speed = 3.2f;
    public int hp = 20;

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
            core.externalSpeedMultiplier = 1f; // �⺻.
        }
        if (health != null)
        {
            health.currentHP = hp;
            health.maxHP = hp;
        }
    }
}