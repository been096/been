using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ���(�÷��̾�/��) �� '����ȿ�� �����̳�'.
/// - Add/Remove/Find/Update�� ���.
/// - �ܺ� �ý���(�̵�/����/�Է�)�� ������ �� �ִ� ���� API ����.
/// </summary>
public class StatusEffectHost : MonoBehaviour
{
    // ���� Ȱ�� ȿ�� ����Ʈ.
    private List<StatusEffectBase> effects = new List<StatusEffectBase>();  // Ȱ�� �ν��Ͻ� ���.

    // ĳ�õ� ���� ��(����/������)
    private float cachedSpeedMul = 1.0f;    // ��� ���ο�/������ �ݿ��� ���� �̵��ӵ� ����.
    private bool cachedStunned = false;     // ���� ���� ������.

    private void Update()
    {
        float dt = Time.deltaTime;

        // 1) ȿ�� ƽ.
        for (int i = 0; i < effects.Count; i = i + 1)
        {
            StatusEffectBase e = effects[i];
            if (e != null)
            {
                e.Tick(dt);
            }
        }

        // 2) ���� ����(�ڿ��� ������). for���� �ݵ�� �ڿ��� ������ ����.  �տ��� �ڷ� �����ϸ� ������ ����.
        for (int i = effects.Count - 1; i >= 0; i = i - 1)
        {
            StatusEffectBase e = effects[i];
            if (e == null)
            {
                effects.RemoveAt(i);
                continue;
            }
            bool expired = e.IsExpired();
            if (expired == true)
            {
                e.Detach();
                Destroy(e);
                effects.RemoveAt(i);
            }
        }

        // 3) ���� ĳ�� ����.
        RebuildCachedQueryValues();
    }

    /// <summary>
    /// ȿ���� �߰�. ���� Ÿ���̸� OnReapplied ȣ��(Ŭ������ ������ ��Ģ ����)
    /// �������� �ν��Ͻ�ȭ�Ͽ� �� ȣ��Ʈ ��ü�� ���δ�.
    /// T : ���ø�(Template) -> �ڷ����� �̸� ������ �ʴ´�.
    /// </summary>
    public T AddEffect<T>(T effectPrefab) where T : StatusEffectBase
    {
        if (effectPrefab == null)
        {
            return null;
        }

        // ���� Ÿ���� ���� ȿ�� ã��.
        T found = FindEffect<T>();
        if (found != null)
        {
            found.OnReapplied();
            return found;
        }

        // �� �ν��Ͻ� ����.
        T inst = gameObject.AddComponent<T>();
        CopyFields(effectPrefab, inst); // ������ �� ����(���� ����)
        inst.Attach(this);
        effects.Add(inst);

        RebuildCachedQueryValues();
        return inst;
    }

    public T FindEffect<T>() where T : StatusEffectBase
    {
        for (int i = 0; i < effects.Count; i = i + 1)
        {
            T casted = effects[i] as T;
            if (casted != null)
            {
                return casted;
            }
        }
        return null;
    }

    /// <summary>
    /// ĳ�ð� ���� : �̵��ӵ� ����, ���� ���� ��.
    /// </summary>
    private void RebuildCachedQueryValues()
    {
        // ���� ������ : ��� ������.
        float speedMul = 1.0f;
        bool stunned = false;

        // 1) ��� ȿ���� �Ⱦ� ����ġ ���.
        for (int i = 0; i < effects.Count; i = i + 1)
        {
            StatusEffectBase e = effects[i];

            // ���ο� : ���� ����(���� ����) ������ �ݿ�.
            StatusEffect_Slow slow = e as StatusEffect_Slow;
            if (slow != null)
            {
                float m = slow.GetSpeedMultiplier();
                if (m < speedMul)
                {
                    speedMul = m;
                }
            }

            // ���� : �ϳ��� ������ true
            StatusEffect_Stun stun = e as StatusEffect_Stun;
            if (stun != null)
            {
                if (stun.IsStunned() == true)
                {
                    stunned = true;
                }
            }
        }

        cachedSpeedMul = speedMul;
        cachedStunned = stunned;
    }

    /// <summary>
    /// �ܺ�(�̵� �ڵ�)���� ȣ�� : �̵� �ӵ� ���� ���� ��ȯ(0~1).
    /// </summary>
    public float GetSpeedMultiplier()
    {
        float v = cachedSpeedMul;
        return v;
    }

    /// <summary>
    /// �ܺ�(����/�Է�)���� ȣ�� : ���� �������� ����.
    /// </summary>
    public bool IsStunned()
    {
        bool v = cachedStunned;
        return v;
    }

    /// <summary>
    /// ������ �ʵ� ����(������ -> �ν��Ͻ�). �ʿ� �ʵ常 ����.
    /// </summary>
    private void CopyFields(StatusEffectBase src, StatusEffectBase dst)
    {
        if (src == null)
        {
            return;
        }
        if (dst == null)
        {
            return;
        }

        dst.effectName = src.effectName;
        dst.icon = src.icon;
        dst.duration = src.duration;
        dst.refreshOnReapply = src.refreshOnReapply;
    }

    /// <summary>
    /// ���� Ȱ�� ȿ�� ����Ʈ�� �б�(������ UI ǥ�� ��).
    /// </summary>
    public List<StatusEffectBase> GetActiveEffects()
    {
        List<StatusEffectBase> list = new List<StatusEffectBase>(effects);
        return list;
    }
}
