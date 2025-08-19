using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIBinder : MonoBehaviour
{
    [Header("ÂüÁ¶")]
    public Health targetHealth;
    public Slider hpSlider;

    private void Reset()
    {
        if(targetHealth == null)
        {
            targetHealth = GetComponent<Health>();
        }

        if(hpSlider == null)
        {
            hpSlider = GetComponent<Slider>();
        }
    }

    private void OnEnable()
    {
        hpSlider.minValue = 0.0f;
        hpSlider.maxValue = Mathf.Max(hpSlider.maxValue, 1);

        if (targetHealth != null)
        {
            targetHealth.OnHPChanged += OnHPChanged;
        }
    }

    private void OnDisable()
    {
        if(targetHealth != null)
        {
            targetHealth.OnHPChanged -= OnHPChanged;
        }
    }
    void OnHPChanged(int current, int max)
    {
        hpSlider.maxValue = Mathf.Max(max, 1);
        hpSlider.value = Mathf.Clamp(current, 0, hpSlider.maxValue);
    }
}
