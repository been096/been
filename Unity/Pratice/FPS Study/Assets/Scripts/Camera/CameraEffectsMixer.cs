using UnityEngine;

public interface ICameraEffect
{
    Vector3 CurrentPositionOffset { get; }
    Vector3 CurrentRotationOffsetEuler { get; }
    float CurrentFovOffset { get; }
}

public class CameraEffectsMixer : MonoBehaviour
{
    [Header("Target")]
    public Camera targetcamera;                                 // ����� ������ ī�޶�

    [Header("Effects")]
    public MonoBehaviour[] effectBehaviours;                   // ICameraEffect ���� ������Ʈ��.

    [Header("Master Intensity")]
    [Range(0f, 3f)] public float positionIntensity = 1.25f;   // ��ġ ������ ��ü ����.
    [Range(0f, 3f)] public float rotationIntensity = 1.25f;   // ȸ�� ������ ��ü ����.
    [Range(0f, 3f)] public float fovIntensity = 1.10f;        // 

    private Vector3 baseLocalPosition;
    private Quaternion baseLocalRotation;
    private float baseFov;

    private ICameraEffect[] effectes;

    private void Awake()
    {
        if (targetcamera = null)
        {
            targetcamera = GetComponent<Camera>();
        }

        if (targetcamera == null)
        {
            Debug.LogError("CameraEffectsMixer : targetCamera�� �ʿ��մϴ�.");
        }

        baseLocalPosition = transform.localPosition;
        baseLocalRotation = transform.localRotation;

        if (targetcamera != null)
        {
            baseFov = targetcamera.fieldOfView;
        }

        if (effectBehaviours != null)
        {
            effectes = new ICameraEffect[effectBehaviours.Length];

            for (int i = 0; i < effectBehaviours.Length; i++)
            {
                ICameraEffect eff = effectBehaviours[i] as ICameraEffect;   // ȿ�� ĳ���� ���.
                if (eff != null)
                {
                    effectes[i] = eff;
                }
            }
        }
    }

    private void LateUpdate()
    {
        Vector3 posOffset = Vector3.zero;
        Vector3 rotEulerOffset = Vector3.zero;
        float fovOffset = 0f;

        if (effectes != null)
        {
            for (int i = 0; i < effectes.Length; i++)
            {
                if (effectes[i] != null)
                {
                    posOffset += effectes[i].CurrentPositionOffset;
                    rotEulerOffset += effectes[i].CurrentRotationOffsetEuler;
                    fovOffset += effectes[i].CurrentFovOffset;
                }
            }
        }

        // ������ ���� ����
        posOffset *= positionIntensity;
        rotEulerOffset *= rotationIntensity;
        fovOffset *= fovIntensity;

        transform.localPosition = baseLocalPosition + posOffset;

        Quaternion rotOffsetQuat = Quaternion.Euler(rotEulerOffset); // ���Ϸ� -> ���ʹϾ�
        transform.localRotation = baseLocalRotation * rotOffsetQuat;

        if (targetcamera != null)
        {
            targetcamera.fieldOfView = baseFov + fovOffset;
        }
    }
}
