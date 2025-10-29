using UnityEngine;

/// <summary>
/// DoT(�ʴ� ����) ����ȿ��. ƽ ���ݸ��� IDamageable�� ���� ����.
/// - ���� �ִ�ġ ����. ������ �� ���� ���, DPS ����.
/// - refreshOnReapply = true �� ������ �� ���� �ð� ����.
/// </summary>
public class StatusEffect_DOT : StatusEffectBase
{
    [Header("DOT")]
    public float dpsPerStack = 5.0f;            // ���ô� �ʴ� ����.
    public float tickInterval = 0.5f;           // ���� ƽ ����(��)
    public int maxStacks = 5;                   // �ִ� ����.
    public bool extendDurationOnReapply = true; // ������ �� duration ��������.

    // ���� ����.
    private int stacks;                         // ���� ���� ��.
    private float tickTimer;                    // ���� ƽ���� ���� �ð�.
    private IDamagealbe damagealbe;             // ����� IDamageable ����.

    protected override void OnAttached()
    {
        // �ʱ�ȭ.
        stacks = 1;
        tickTimer = tickInterval;
        damagealbe = GetComponent<IDamagealbe>();
    }

    public override void OnReapplied()
    {
        // 1) ���� ����(���� ĸ)
        stacks = stacks + 1;
        if (stacks > maxStacks)
        {
            stacks = maxStacks;
        }

        // 2) ���ӽð� ���� �ɼ�.
        if (extendDurationOnReapply == true)
        {
            timeLeft = duration;
        }
    }

    protected override void OnTick(float dt)
    {
        // ƽ Ÿ�̸� ����.
        tickTimer = tickTimer - dt;
        if (tickTimer > 0.0f)
        {
            return;
        }

        // ƽ ���� ���� : ���� ��� �� ����.
        tickTimer = tickInterval;

        // �̹� ƽ�� ���ط�(�ʴ� ���� x ���� x ����)
        float dmg = dpsPerStack * tickInterval * stacks;

        if (damagealbe != null)
        {
            // ���� ��ǥ : ������ ��� �߽�.
            Vector3 hp = transform.position;
            Vector3 n = Vector3.up;
            damagealbe.ApplyDamage(dmg, hp, n, host != null ? host.transform : transform);
        }
    }

    protected override void OnDetached()
    {
        // DoT�� ���� ���� ����(���ο�ó�� ���� ������ �ƴ�)
       
    }

    /// <summary>���� ���� �� ��ȯ(UI ǥ�ÿ�). </summary>
    public int GetStacks()
    {
        int v = stacks;
        return v;
    }
}
