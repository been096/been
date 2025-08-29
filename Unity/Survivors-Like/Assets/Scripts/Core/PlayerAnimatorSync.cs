using UnityEngine;

/// <summary>
/// [12����] �÷��̾� �ִϸ��̼� ����ȭ �����
/// Animator �Ķ����:
///  - Speed(float), IsMoving(bool), IsAttacking(bool), HitBool(bool), IsDead(bool)
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimatorSync : MonoBehaviour
{
    [Header("Refs")]
    public Animator anim;
    public Rigidbody2D rb;
    public Health health; // ����

    [Header("Config")]
    public float moveThreshold = 0.05f;
    public float attackFlagTime = 0.15f;
    public float hitFlagTime = 0.12f;

    private float attackTimer = 0f;
    private float hitTimer = 0f;

    void Awake()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if (health == null)
        {
            health = GetComponent<Health>();
        }
    }

    void Update()
    {
        // �̵� ���� ����
        float speed = 0f;
        if (rb != null)
        {
            speed = rb.velocity.magnitude;
        }

        if (anim != null)
        {
            anim.SetFloat("Speed", speed);
            if (speed > moveThreshold)
            {
                anim.SetBool("IsMoving", true);
            }
            else
            {
                anim.SetBool("IsMoving", false);
            }
        }

        // ���� �÷��� Ÿ�̸�
        if (attackTimer > 0f)
        {
            attackTimer = attackTimer - Time.deltaTime;
            if (attackTimer <= 0f)
            {
                if (anim != null)
                {
                    anim.SetBool("IsAttacking", false);
                }
            }
        }

        // �ǰ� �÷��� Ÿ�̸�
        if (hitTimer > 0f)
        {
            hitTimer = hitTimer - Time.deltaTime;
            if (hitTimer <= 0f)
            {
                if (anim != null)
                {
                    anim.SetBool("HitBool", false);
                }
            }
        }

        // ��� ����(����)
        if (health != null && anim != null)
        {
            bool alive = health.IsAliveNow();
            if (alive == false)
            {
                anim.SetBool("IsDead", true);
            }
        }
    }

    public void NotifyAttack()
    {
        if (anim != null)
        {
            anim.SetBool("IsAttacking", true);
        }
        attackTimer = attackFlagTime;
    }

    public void NotifyHit()
    {
        if (anim != null)
        {
            anim.SetBool("HitBool", true);
        }
        hitTimer = hitFlagTime;
    }

    public void NotifyDead()
    {
        if (anim != null)
        {
            anim.SetBool("IsDead", true);
        }
    }
}