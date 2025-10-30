using UnityEngine;

/// <summary>
/// ��� �� ������ �߻� ��� Ŭ����.
/// �� ���´� Enter/Update/Exit ���� �ֱ⸦ ���� �����ϸ�,
/// EnemyBrain(���ؽ�Ʈ)�� ���� �����͸� ����Ѵ�.
/// </summary>
public abstract class EnemyState
{
    /// <summary>
    /// ���°� ����� ���ؽ�Ʈ(���� �����Ϳ� ��ƿ�� �޼��� ����).
    /// </summary>
    protected EnemyBrain brain; // ���� �� ��ü�� �극��(����/����/���� �� ����)

    /// <summary>
    /// ���� �ν��Ͻ��� ����� ���ٽ�Ʈ�� �����Ѵ�.
    /// ��� ���´� ��ȯ �ÿ� ���� �극���� �����Ѵ�.
    /// </summary>
    public void SetContext(EnemyBrain b)
    {
        brain = b;
    }

    /// <summary>
    /// ���� ���� �� ȣ��ȴ�. �ִ�/����/�ʱ� Ÿ�̸� ���� ���� �����Ѵ�.
    /// </summary>
    public virtual void OnEnter()
    {
        // �Ļ� Ŭ�������� �ʿ� �� ����.
    }

    /// <summary>
    /// �� ������ ȣ��ȴ�. ���� ������ ������ �����ϰ�,
    /// �ʿ� �� brain.ReQuestStateChange(...)�� ���� ��û�� �Ѵ�.
    /// <summary>
    public virtual void OnUpdate(float dt)
    {     
        // �Ļ� Ŭ�������� ����.
    }

    /// <summary>
    /// ���� ���� ������ ȣ��ȴ�. Ÿ�̸�/�÷��׸� �����Ѵ�.
    /// </summary>
    public virtual void OnExit()
    {
        // �Ļ� Ŭ�������� �ʿ� �� ����.
    }

    /// <summary>
    /// ����׿� ���¸� ��ȯ.
    /// </summary>
    public abstract string Name();
}
