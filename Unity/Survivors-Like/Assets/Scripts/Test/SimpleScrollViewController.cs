using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleScrollViewController : MonoBehaviour
{
    [Header("References")]
    public ScrollRect scrollRect;        // ��ũ�� ������ ����ϴ� ScrollRect.
    public RectTransform content;        // �����۵��� ���� Content(RectTransform).
    public GameObject itemPrefab;        // �ϳ��� ����Ʈ ������ ������(�ؽ�Ʈ ǥ�� �����ؾ� ��).

    [Header("Initial Items")]
    public int initialItemCount = 20;    // ���� �� ������ ������ ����.

    private void Start()
    {
        // �ʱ� ������ ���� �� ǥ��.
        Populate(initialItemCount);
    }

    /// <summary>
    /// Content �Ʒ��� ���� �������� ���� �����մϴ�.
    /// </summary>
    public void Clear()
    {
        if (content == null)
        {
            return;
        }

        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Transform child = content.GetChild(i);
            if (child != null)
            {
                Destroy(child.gameObject);
            }
        }

        // ��� ���̾ƿ� �������� ȭ�� �ݿ�;
        Canvas.ForceUpdateCanvases();
    }

    /// <summary>
    /// count ������ŭ �������� �����Ͽ� ǥ��.    
    /// </summary>
    public void Populate(int count)
    {
        if (content == null || itemPrefab == null)
        {
            return;
        }

        Clear();

        for (int i = 0; i < count; i++)
        {
            string label = $"Item {i}";
            AddItem(label);
        }

        // �� ���� �÷��� ù �������� ���̵���.
        if (scrollRect != null)
        {
            scrollRect.verticalNormalizedPosition = 1.0f;
        }
    }

    /// <summary>
    /// ���� �������� ������ Content ������ �߰��ϰ� ǥ��.
    /// </summary>
    public void AddItem(string label)
    {
        if (content == null || itemPrefab == null)
        {
            return;
        }

        // ������ �ν��Ͻ� ���� �� Content�� �ڽ����� ��ġ.
        GameObject go = Instantiate(itemPrefab, content);
        go.name = $"Item_{content.childCount - 1}";

        // ������ ������ �ؽ�Ʈ ������Ʈ�� �� �ݿ�.
        ApplyLabelToItem(go, label);

        // ��� ���̾ƿ� �������� ȭ�� �ݿ�.
        Canvas.ForceUpdateCanvases();
    }

    /// <summary>
    /// ���޵� GameObject(������ �ν��Ͻ�)���� �ؽ�Ʈ ������Ʈ�� ã�� label�� �ݿ�.
    /// </summary>
    private void ApplyLabelToItem(GameObject go, string label)
    {
        if (go == null)
        {
            return;
        }

        TMP_Text tmp = go.GetComponentInChildren<TMP_Text>(true);
        if (tmp != null)
        {
            tmp.text = label;
            return;
        }
    }
}