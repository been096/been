using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NightFogOverlay : MonoBehaviour
{
    public Image overlayImage;              // Ǯ ��ũ�� Image
    public DayNightController dayNight;     // �� ������ ����
    public Color nightTint = new Color(0.05f, 0.08f, 0.2f, 0.4f);
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (overlayImage == null)
        {
            return;
        }
        if (dayNight == null)
        {
            return;
        }

        float nightFactor = dayNight.GetNightFactor();

        Color c = nightTint;
        c.a = Mathf.Lerp(0.0f, nightTint.a, nightFactor);
        overlayImage.color = c;
    }
}
