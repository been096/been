using UnityEngine;

/// <summary>
/// ���ο� ����ȿ��. �̵��ӵ� ����(0~1)�� ����.
/// - Host�� ��� ���ο� �� '���� ���� ����'�� �ݿ�(���� ���� ���ο� �켱)
/// </summary>
public class StatusEffect_Slow : StatusEffectBase
{
    [Header("Slow")]
    [Range(0.0f, 1.0f)]
    public float speedMultiplier = 0.6f;    // ���� ����(�� : 0.6�̸� 40% ����)

    protected override void OnAttached()
    {
        // ���� �ʿ� ���� : ȣ��Ʈ�� '���� ����'�� �� ������ ����.
    }
    protected override void OnTick(float dt)
    {
        // ���ο�� ƽ ó�� �ʿ� ����(���� ���Ҹ� ó����)
    }

    protected override void OnDetached()
    {
        // ���� ���� ����(ȣ��Ʈ�� �� ������ ���� ������ �����ϹǷ� �ڵ� ����)
    }

    public float GetSpeedMultiplier()
    {
        float v = speedMultiplier;
        return v;
    }
}
