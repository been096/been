using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldUIBinder : MonoBehaviour
{
    public GoldSystem gold;
    public TextMeshProUGUI goldText; // "Gold: 123" 같은 텍스트

    void Start()
    {
        if (gold == null)
        {
            gold = FindAnyObjectByType<GoldSystem>();
        }
    }

    void Update()
    {
        //골드가 바뀔때만 갱신이 되는게 좋기는 함. 나중에 해보자.
        if (goldText != null && gold != null)
        {
            goldText.text = "Gold: " + gold.currentGold.ToString();
        }
    }
}
