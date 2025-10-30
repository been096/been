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
        // 사망 상태 진입 시 모든 행동 중지.
        // 이 상태에서는 Update를 해도 아무 동작을 하지 않는다.
    }

    public override void OnUpdate(float dt)
    {
        
    }

    public override void OnExit()
    {
        
    }
}
