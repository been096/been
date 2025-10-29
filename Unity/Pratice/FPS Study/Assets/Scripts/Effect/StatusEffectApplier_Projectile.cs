using UnityEngine;

/// <summary>
/// �浹 �������� ����� StatusEffectHost�� ã�� 'ȿ�� ������'�� �ο��ϴ� ���� ��ƿ.
/// - �߻�ü/����/Ʈ�� ���� ��Ʈ ������ ȣ���ϸ� �ȴ�.
/// - ������ ���� ���� ȿ�� ������Ʈ�� '����'�Ͽ� ��� ���δ�.
/// </summary>
public static class StatusEffectApplier
{
    /// <summary>
    /// ��� ȿ���� �� ���� �߰�. ���� Ÿ�� ���� �� OnReapplied�� ȣ��.
    /// </summary>
    public static T ApplyTo<T>(GameObject target, T effectPreset) where T : StatusEffectBase
    {
        Debug.Log("Apply Status Effect : " + effectPreset.GetType());
        if (target == null)
        {
            return null;
        }
        if (effectPreset == null)
        {
            return null;
        }

        StatusEffectHost host = target.GetComponent<StatusEffectHost>();
        if (host == null)
        {
            // ��� ȣ��Ʈ�� ������ ȿ���� ���� �� ����(������ �� 'ȿ���� ���� �� ����' ���).
            return null;
        }

        T inst = host.AddEffect(effectPreset);
        return inst;
    }
}