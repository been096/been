using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldUIBinder : MonoBehaviour
{
    public GoldSystem gold;
    public TextMeshProUGUI goldText; // "Gold: 123" ���� �ؽ�Ʈ

    void Start()
    {
        if (gold == null)
        {
            gold = FindAnyObjectByType<GoldSystem>();
        }
    }

    void Update()
    {
        //��尡 �ٲ𶧸� ������ �Ǵ°� ����� ��. ���߿� �غ���.
        if (goldText != null && gold != null)
        {
            goldText.text = "Gold: " + gold.currentGold.ToString();
        }
    }
}
