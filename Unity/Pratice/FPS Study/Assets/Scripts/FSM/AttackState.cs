using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Attack : ��Ÿ� ������ ��ٿ��� ��Ű�� �÷��̾ ����.
/// ��Ÿ� ��Ż �� Chase, �þ� ���� ��� �� Search.
/// </summary>
public class AttackState : EnemyState
{
    public AttackState()
    {

    }

    public AttackState(EnemyBrain b)
    {
        brain = b;
    }

    public override string Name()
    {
        return "Attack";
    }

    public override void OnEnter()
    {
        // ���� ���� ���� �� ��ٿ��� ��� 0���� �θ� �ٷ� �� �� ���� ����.
        brain.attackTimer = 0.0f;
    }

    public override void OnUpdate(float dt)
    {
        // 1) �þ� �Ǵ� + lastKnownPos ����.
        bool seen = false;
        Vector3 seenPos = Vector3.zero;

        if (brain.senses != null)
        {
            bool can = brain.senses.CanSeeTarget(out seenPos);
            if (can == true)
            {
                seen = true;
                brain.lastKnownPos = seenPos;
            }
        }

        // 2) ��Ÿ� ���� üũ : ��Ż �� Chase ����
        float dist = brain.DistanceToPlayer();
        if (dist > brain.attackRange)
        {
            brain.RequestStateChange(new ChaseState(brain));
            return;
        }

        // 3) �þ� ������ ����� Search
        if (seen == false)
        {
            brain.RequestStateChange(new SearchState(brain));
            return;
        }

        // 4) ���� ��ٿ� Ÿ�̸�
        if (brain.attackTimer > 0.0f)
        {
            brain.attackTimer = brain.attackTimer - dt;
            if (brain.attackTimer < 0.0f)
            {
                brain.attackTimer = 0.0f;
            }
        }

        // 5) ��ٿ��� 0�̸� ���� ����.
        if (brain.attackTimer <= 0.0f)
        {
            DoAttack();
            brain.attackTimer = brain.attackCooldown;
        }

        // 6) �ð��� ���� : �÷��̾ �ٶ󺸰� ȸ��.
        if (brain.player != null)
        {
            brain.FacePosition(brain.player.position, dt);
        }

    }
    public override void OnExit()
    {
        
    }

    /// <summary>
    /// ���� �������� �÷��̾�(IDamageable)���� �����Ѵ�.
    /// (���� ���� : �÷��̾� ��ġ�� ��Ʈ ����Ʈ�� ���)
    /// </summary>
    private void DoAttack()
    {
        if (brain.player == null)
        {
            return;
        }

        IDamagealbe id = brain.player.GetComponent<IDamagealbe>();
        if (id == null)
        {
            return;
        }

        Vector3 hp = brain.player.position;     // ��Ʈ ����Ʈ
        Vector3 n = Vector3.up;                 // ���.

        id.ApplyDamage(brain.attackDamage, hp, n, brain.transform);
    }
}
