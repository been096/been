using UnityEngine;

public class FovKickEffect : MonoBehaviour, ICameraEffect
{
    [Header("Refs")]
    public LocomotionFeed feed;                 // �ӵ� ������.

    [Header("Params")]
    public float acclSensitivity = 0.8f;        // ���� -> FOV ���� ���(��/(m/s^2))
    public float maxKick = 8.0f;
    public float riseTime = 0.10f;
    public float fallTime = 0.18f;
    public float speedSmoothing = 8f;

    private float smoothedSpeed;
    private float lastSmoothedSpeed;
    private float current;
    private float velocity;

    public Vector3 CurrentPositionOffset { get { return Vector3.zero; } }
    public Vector3 CurrentRotationOffsetEuler { get { return Vector3.zero; } }
    public float CurrentFovOffset { get { return current; } }

    private void Update()
    {
        if (feed == null)
        {
            return;
        }

        float dt = Time.deltaTime;                          // ������ �ð�.
        float rawSpeed = feed.HorizontalSpeed;              // ���� �ӵ�.

        //��������� �ӵ� ��Ȱȭ.
        float alpha = 1f - Mathf.Exp(-speedSmoothing * dt); // EMA ���
        smoothedSpeed = Mathf.Lerp(smoothedSpeed, rawSpeed, alpha);

        // ���ӵ� �ٻ�(�̺�)
        float accel = 0f;                                   // m/s^2 �ٻ�
        accel = (smoothedSpeed - lastSmoothedSpeed) / Mathf.Max(dt, 0.0001f);
        lastSmoothedSpeed = smoothedSpeed;

        // ��ǥ FOV : ���� ���ӿ����� ���, �ƴϸ� 0���� ����.
        float target = 0f;

        if (accel > 0f)
        {
            target = Mathf.Clamp(accel * acclSensitivity, 0f, maxKick);
        }
        else
        {
            target = 0f;
        }

        float smoothTime = 0.15f;

        if (target > current)
        {
            smoothTime = riseTime;
        }
        else
        {
            smoothTime = fallTime;
        }

        current = Mathf.SmoothDamp(current, target, ref velocity, smoothTime, Mathf.Infinity, dt);
    }
}
