using UnityEditor.Search;
using UnityEngine;

/// <summary>
/// Chase : ���������� �� �÷��̾� ��ġ�� ȸ��/����.
/// ��Ÿ� �̳��� Attack, �þ� ��� �� �����ϸ� Search.
/// </summary>
public class ChaseState : EnemyState
{
    public ChaseState()
    {

    }

    public ChaseState(EnemyBrain b)
    {
        brain = b;
    }

    public override string Name()
    {
        return "Chase";
    }

    public override void OnEnter()
    {
        // ��ٿ� ���� AttackState�� �����ϹǷ� ���⼱ ����.
    }

    public override void OnUpdate(float dt)
    {
        // 1) �þ� ����.
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

        // 2) ȸ��/����.
        brain.FacePosition(brain.lastKnownPos, dt);

        float distToLast = Vector3.Distance(brain.transform.position, brain.lastKnownPos);

        if (distToLast > brain.stoppingDistance)
        {
            brain.MoveForward(brain.chaseSpeed, dt);
        }

        // 3) ��Ÿ� ���� -> Attack
        float distToPlayer = brain.DistanceToPlayer();
        if (distToPlayer <= brain.attackRange)
        {
            brain.RequestStateChange(new AttackState(brain));
            return;
        }

        // 4) �þ� ��� + ������ ���� -> Search
        if (seen == false)
        {
            if (distToLast <= brain.stoppingDistance)
            {
                brain.RequestStateChange(new SearchState(brain));
                return;
            }
        }
    }

    public override void OnExit()
    {
        // ����.
    }
}
