using UnityEngine;

/// <summary>
/// �׽�Ʈ��: K Ű�� ������ XP +10.
/// </summary>
public class TestXpGiver : MonoBehaviour
{
    public XpSystem xpSystem;
    public int amountPerPress = 10;

    private void Reset()
    {
        if (!xpSystem)
        {
            xpSystem = FindAnyObjectByType<XpSystem>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            xpSystem?.AddExp(amountPerPress);
        }
    }
}