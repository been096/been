using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// ���� Ȱ�� ����ȿ���� ������ ������ ����Ʈ�� ǥ��.
/// - Horizontal Layout �ȿ� '������ ���� ������'�� ����.
/// - �� ����: Image(������) + TextMeshProUGUI(���� �ð�, ����)
/// </summary>
public class StatusEffectHUD : MonoBehaviour
{
    [Header("Refs")]
    public StatusEffectHost host;           // ��� ȣ��Ʈ.
    public RectTransform content;           // ���Ե��� ��ġ�� �θ�(���� ���̾ƿ� ����)
    public GameObject slotPrefab;           // ���� ������(�̹���+�ؽ�Ʈ)

    private readonly List<GameObject> slots = new List<GameObject>(); // ������ ����.

    private void Awake()
    {
        if (host == null)
        {
            host = FindAnyObjectByType<StatusEffectHost>();
        }
    }

    private void LateUpdate()
    {
        if (host == null)
        {
            return;
        }

        List<StatusEffectBase> list = host.GetActiveEffects();

        // ���� �� ���߱�.
        while (slots.Count < list.Count)
        {
            GameObject s = Instantiate(slotPrefab, content);
            slots.Add(s);
        }
        while (slots.Count > list.Count)
        {
            int last = slots.Count - 1;
            Destroy(slots[last]);
            slots.RemoveAt(last);
        }

        // ������ ���ε�.
        for (int i = 0; i < list.Count; i = i + 1)
        {
            StatusEffectBase e = list[i];
            GameObject s = slots[i];

            Image iconImg = s.GetComponentInChildren<Image>();
            if (iconImg != null)
            {
                iconImg.sprite = e.icon;
                iconImg.enabled = e.icon != null;
            }

            TextMeshProUGUI txt = s.GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null)
            {
                // ���� �ð�(����) + ����: ����.
                int left = Mathf.CeilToInt(e.GetTimeLeft());
                string extra = "";

                StatusEffect_DOT dot = e as StatusEffect_DOT;
                if (dot != null)
                {
                    extra = $" x{dot.GetStacks()}";
                }

                txt.text = $"{e.effectName} {left}s{extra}";
            }
        }
    }
}