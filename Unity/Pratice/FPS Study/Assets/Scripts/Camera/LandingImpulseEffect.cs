using UnityEngine;

public class LandingImpulseEffect : MonoBehaviour, ICameraEffect
{
    [Header("Refs")]
    public LocomotionFeed feed;                         // 접지/수직 속도.

    [Header("Mapping")]
    public float maxConsideredFallSpeed = 12f;          //정규화 상한.
    public AnimationCurve strengtByFall =               //낙하속도 ( 0 ~ 1) -> 강도 ( 0 ~ 1 )
        AnimationCurve.EaseInOut(0f, 0.6f, 1f, 1.0f);   // 지낙하도 존재감 있게.

    [Header("Impulse")]
    public float amplitudeScale = 0.14f;                // 최대 강도 시 Y 진폭(미터)
    public float damping = 10f;                         // 감쇠 계수.
    public float oscillationHz = 6.5f;                  // 잔진동 빈도.
    public float tiltDegrees = 3.5f;                    // 피치 숙임.

    private bool wasGrounded;                           // 전 프레임 접지 상태.
    private float timeSinceImpact;                      // 임펄스 경과 시간.
    private float impactStrength;                       // 0 ~ 1 강도.

    private Vector3 posOffset;                          // 위치 오프셋.
    private Vector3 rotOffset;                          // 회전 오프셋.

    public Vector3 CurrentPositionOffset { get { return posOffset; } }
    public Vector3 CurrentRotationOffsetEuler { get { return rotOffset; } }
    public float CurrentFovOffset { get { return 0f; } }

    private void Update()
    {
        if (feed == null)
        {
            return;
        }

        bool grounded = feed.IsGrounded;                // 현재 접지.

        if (grounded == true && wasGrounded == false)
        {
            float vyAbs = Mathf.Abs(feed.VerticalVelocity);             // 착지 직전 |Vy|
            float tNorm = Mathf.Clamp01(vyAbs / maxConsideredFallSpeed);
            float mapped = strengtByFall.Evaluate(tNorm);
            impactStrength = mapped;
            timeSinceImpact = 0f;
        }

        wasGrounded = grounded;

        float dt = Time.deltaTime;

        float A = amplitudeScale * impactStrength;
        float decay = Mathf.Exp(-damping * timeSinceImpact);
        float y = A * decay * (1f - Mathf.Cos(2f * Mathf.PI * oscillationHz * timeSinceImpact));

        posOffset = new Vector3(0f, -y, 0f);
        rotOffset = new Vector3(tiltDegrees * impactStrength * decay, 0f, 0f);
    }
}
