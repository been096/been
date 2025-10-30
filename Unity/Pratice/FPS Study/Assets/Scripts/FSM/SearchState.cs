using UnityEngine;

/// <summary>
/// Search : �þ߸� ���� �� ������ ��ǥ �ٹ��� ª�� ����.
/// Ÿ�Ӿƿ� �� Idle ����, �ٽ� ���̸� Chase.
/// </summary>
public class SearchState : EnemyState
{
    public SearchState()
    {

    }
    
    public SearchState(EnemyBrain b)
    {
        brain = b;
    }

    public override string Name()
    {
        return "Search";
    }

    public override void OnEnter()
    {
        // ���� Ÿ�̸� �ʱ�ȭ.
        brain.searchTimer = brain.searchDuration;
    }

    public override void OnUpdate(float dt)
    {
        // 1) �þ� ȸ�� �� ��� Chase
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

        if (seen == true)
        {
            brain.RequestStateChange(new ChaseState(brain));
            return;
        }

        // 2) ������ ��ǥ ������ ������ ����.
        brain.FacePosition(brain.lastKnownPos, dt);

        float dist = Vector3.Distance(brain.transform.position, brain.lastKnownPos);
        if (dist > brain.stoppingDistance)
        {
            brain.MoveForward(brain.searchSpeed, dt);
        }

        // 3) Ÿ�Ӿƿ� -> Idle
        brain.searchTimer = brain.searchTimer - dt;
        if (brain.searchTimer <= 0.0f)
        {
            brain.RequestStateChange(new IdleState(brain));
            return;
        }
    }

    public override void OnExit()
    {
        // ����.
    }

}
