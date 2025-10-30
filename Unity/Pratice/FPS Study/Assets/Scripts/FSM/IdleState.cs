using UnityEngine;

/// <summary>
/// Idle : ���. �÷��̾ ���� Chase�� ����.
/// </summary>
public class IdleState : EnemyState
{
    // ������. Ŭ���� �̸��� ������ �Լ�. Ŭ���� ��ü�� ������� �� �ڵ����� ȣ��Ǵ� �Լ�.
    // ������ �ʱ�ȭ ���� �� �� ����.
    public IdleState()
    {

    }

    public IdleState(EnemyBrain b)
    {
        brain = b;
    }

    public override string Name()
    {
        return "Idle";
    }

    public override void OnEnter()
    {
        // Idle ���� �� Ư���� �ʱ�ȭ�� ����.
        // �ʿ��ϸ� ��� ���/���� Ʈ���� ����.
    }

    public override void OnUpdate(float dt)
    {
        // �þ� ����.
        bool seen = false;
        Vector3 seenPos = Vector3.zero;

        if (brain.senses != null)
        {
            bool can = brain.senses.CanSeeTarget(out seenPos);
            if (can == true)
            {
                seen = true;
            }
        }

        if (seen == true)
        {
            brain.lastKnownPos = seenPos;
            brain.RequestStateChange(new ChaseState(brain));    // �� �ν��Ͻ� Ȥ�� ���� ����.
            return;
        }

        // Idle ����.
    }

    public override void OnExit()
    {
        // Idle ���� �� ������ �� ����.
    }
}
