using UnityEngine;

public class FovKickEffect : MonoBehaviour, ICameraEffect
{
    [Header("Refs")]
    public LocomotionFeed feed;                 // 속도 데이터.

    [Header("Params")]
    public float acclSensitivity = 0.8f;        // 가속 -> FOV 매핑 계수(도/(m/s^2))
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

        float dt = Time.deltaTime;                          // 프레임 시간.
        float rawSpeed = feed.HorizontalSpeed;              // 원시 속도.

        //저역통과로 속도 평활화.
        float alpha = 1f - Mathf.Exp(-speedSmoothing * dt); // EMA 계수
        smoothedSpeed = Mathf.Lerp(smoothedSpeed, rawSpeed, alpha);

        // 가속도 근사(미분)
        float accel = 0f;                                   // m/s^2 근사
        accel = (smoothedSpeed - lastSmoothedSpeed) / Mathf.Max(dt, 0.0001f);
        lastSmoothedSpeed = smoothedSpeed;

        // 목표 FOV : 양의 가속에서만 상승, 아니면 0으로 복귀.
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
