using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PostVolumeController : MonoBehaviour
{
    [Header("Refs")]
    public Volume globalVolume;
    public Slider bloomIntensitySlider;
    public Slider exposureSlider;
    public Slider dofFocusSlider;

    Bloom bloom;
    ColorAdjustments color;
    DepthOfField dof;

    float defaultBloomIntensity = 2f;
    float defaultExposure = 0f;
    float defaultFocusDistance = 5f;

    private void Awake()
    {
        // "Global Volume" �ڵ� ���� �õ�(������ �ν����ͷ� ���� ����). bloom�� color, dof ��� ������ �������̵��̱� ������ ���� ����� ������ ������ �ļ� �۾�.
        if (globalVolume == null)
        {
            GameObject gv = GameObject.Find("Global Volume");
            if (gv != null)
            {
                globalVolume = gv.GetComponent<Volume>();
            }
        }

        //������Ʈ ĳ�� �� �⺻�� ����
        if (globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out bloom);
            globalVolume.profile.TryGet(out color);
            globalVolume.profile.TryGet(out dof);

            if (bloom != null && bloom.intensity.overrideState)
            {
                defaultBloomIntensity = bloom.intensity.value;
            }
            if (color != null && color.postExposure.overrideState)
            {
                defaultExposure = color.postExposure.value;
            }
            if (dof != null && dof.focusDistance.overrideState)
            {
                defaultFocusDistance = dof.focusDistance.value;
            }
        }

        // �����̴� ���� / �ʱⰪ / ������
        if (defaultBloomIntensity != null)
        {
            bloomIntensitySlider.minValue = 0f;
            bloomIntensitySlider.maxValue = 5f;
            bloomIntensitySlider.value = defaultBloomIntensity;
            bloomIntensitySlider.onValueChanged.AddListener(OnBloomChanged);
        }
        if (defaultExposure != null)
        {
            exposureSlider.minValue = 0f;
            exposureSlider.maxValue = 5f;
            exposureSlider.value = defaultExposure;
            exposureSlider.onValueChanged.AddListener(OnExposureChanged);
        }
        if (defaultFocusDistance != null)
        {
            dofFocusSlider.minValue = 0f;
            dofFocusSlider.maxValue = 5f;
            dofFocusSlider.value = defaultFocusDistance;
            dofFocusSlider.onValueChanged.AddListener(OnFocusChanged);
        }
        
        // ���� �� �����̴� ���� �� �� ����
        ApplyAll();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) == true)
        {
            ToggleBloom();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) == true)
        {
            ToggleColor();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) == true)
        {
            ToggleDof();
        }

        if (Input.GetKeyDown(KeyCode.R) == true)
        {
            ResetDefaults();
        }
    }

    void ToggleBloom()
    {
        if (bloom != null)
        {
            bloom.active = !bloom.active;
        }
    }
    void ToggleColor()
    {
        if (color != null)
        {
            color.active = !color.active;
        }
    }

    void ToggleDof()
    {
        if (dof != null)
        {
            dof.active = !dof.active;
        }
    }

    void ResetDefaults()
    {
        if (bloom != null)
        {
            bloom.intensity.Override(defaultBloomIntensity);
            if (bloomIntensitySlider != null) { bloomIntensitySlider.value = defaultBloomIntensity; }
        }
        if (color != null)
        {
            color.postExposure.Override(defaultExposure);
            if(exposureSlider != null) { exposureSlider.value = defaultExposure; }
        }
        if (dof != null)
        {
            dof.focusDistance.Override(defaultFocusDistance);
            if(dofFocusSlider != null) { dofFocusSlider.value = defaultFocusDistance; }
        }
    }

    void OnBloomChanged(float v)
    {
        if (bloom != null)
        {
            bloom.intensity.Override(v);
        }
    }

    void OnExposureChanged(float v)
    {
        if (color != null)
        {
            color.postExposure.Override(v);
        }
    }

    void OnFocusChanged(float v)
    {
        if (dof != null)
        {
            dof.focusDistance.Override(v);
        }
    }

    void ApplyAll()
    {
        if (bloom != null && bloomIntensitySlider != null)
        {
            bloom.intensity.Override(bloomIntensitySlider.value);
        }
        if (color != null && exposureSlider != null)
        {
            color.postExposure.Override(exposureSlider.value);
        }
        if (dof != null && dofFocusSlider != null)
        {
            dof.focusDistance.Override(dofFocusSlider.value);
        }
    }

}
