using UnityEngine;

/// <summary>
/// ���� ����ȿ��. ���ӽð� ���� '�ൿ ����' �÷��׸� ����.
/// - Host.IsStunned()�� �ܺ� �ý����� üũ�Ͽ� �ൿ�� ���´�.
/// </summary>
public class StatusEffect_Stun : StatusEffectBase
{
    protected override void OnAttached()
    {
        // ���� �� ���� ���� ����. ���Ǵ� Host.IsStunned()��.
    }

    protected override void OnTick(float dt)
    {
        // ���� ���Ҹ� ó��(�θ� Tick���� �̹� ����)
    }

    protected override void OnDetached()
    {
        // ���� �� ���� ó�� ����.
    }

    public bool IsStunned()
    {
        // ������ ����ִ� ���� true�� ����
        bool v = GetTimeLeft() > 0.0f;
        return v;
    }
}
