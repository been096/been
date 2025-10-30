using UnityEngine;

public class DeadState : EnemyState
{
    public DeadState()
    {

    }
    public DeadState(EnemyBrain b)
    {
        brain = b;
    }

    public override string Name()
    {
        return "Dead";
    }

    public override void OnEnter()
    {
        // ��� ���� ���� �� ��� �ൿ ����.
        // �� ���¿����� Update�� �ص� �ƹ� ������ ���� �ʴ´�.
    }

    public override void OnUpdate(float dt)
    {
        
    }

    public override void OnExit()
    {
        
    }
}
