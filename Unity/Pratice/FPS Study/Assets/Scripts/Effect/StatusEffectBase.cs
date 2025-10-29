using UnityEngine;

/// <summary>
/// ��� ����ȿ���� ���� ��� Ŭ����(�߻�).
/// - ����/����/���Ḧ ���� ���� �ʵ�� ���� �ֱ� �� ����.
/// - StatusEffectHost�� ����/����/����/���� ȣ���� ���.
/// </summary>
public abstract class StatusEffectBase : MonoBehaviour
{
    [Header("Common")]
    public string effectName = "Effect";       // �����/ǥ�ÿ� �̸�.
    public Sprite icon;                         // UI ������(����).
    public float duration = 3.0f;               // �� ���ӽð�(��)
    public bool refreshOnReapply = true;        // ���� ȿ�� ������ �� ���ӽð� �������� ����.

    // ���� ����
    protected float timeLeft;                   // ���� �ð�(��)
    protected StatusEffectHost host;            // ������(���)�� ȣ��Ʈ.

    /// <summary>
    /// ȣ��Ʈ�� ���� �� ȣ��. �Ķ���ͷ� ȣ��Ʈ ���� �� �����ð� �ʱ�ȭ.
    /// </summary>
    /// <param name="h"></param>
    public void Attach(StatusEffectHost h)
    {
        host = h;
        timeLeft = duration;

        OnAttached();
    }

    /// <summary>
    /// �� ������(Ȥ�� ���� ƽ) ����. Host.Update���� ȣ���.
    /// </summary>
    /// <param name="dt"></param>
    public void Tick(float dt)
    {
        OnTick(dt);

        timeLeft = timeLeft - dt;
        if (timeLeft < 0.0f)
        {
            timeLeft = 0.0f;
        }
    }

    /// <summary>
    /// ���� ����(���� �ð��� 0����). Host�� �� ������ ���� ������ �Ǵ�.
    /// </summary>
    public bool IsExpired()
    {
        bool expired = timeLeft <= 0.0f;
        return expired;
    }

    /// <summary>
    /// ���� Ÿ���� ������� �� ȣ��. �⺻�� '���ӽð� ����'�� ����.
    /// �Ļ� Ŭ�������� ���� ���� �� �߰� ������ ���� ����.
    /// </summary>
    public virtual void OnReapplied()
    {
        if (refreshOnReapply == true)
        {
            timeLeft = duration;
        }
    }

    /// <summary>
    /// ȣ��Ʈ���� ���ŵ� �� ȣ��(����/����)
    /// </summary>
    public void Detach()
    {
        OnDetached();
    }

    /// <summary>�Ļ� Ŭ������ ���� �� ��. /// </summary>
    protected virtual void OnAttached() { }
    // <summary>�Ļ� Ŭ������ �� ������ ���� ��. /// </summary>
    protected virtual void OnTick(float dt) { }
    // <summary>�Ļ� Ŭ������ ���� �� ��. /// </summary>
    protected virtual void OnDetached() { }

    // <summary>UI � ǥ���� ���� �ð� �ʸ� ��ȯ. /// </summary>
    public float GetTimeLeft()
    {
        float v = timeLeft;
        return v;
    }
}
