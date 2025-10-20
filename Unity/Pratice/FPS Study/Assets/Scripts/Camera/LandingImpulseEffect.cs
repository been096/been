using UnityEngine;

public class LandingImpulseEffect : MonoBehaviour, ICameraEffect
{
    [Header("Refs")]
    public LocomotionFeed feed;                         // ����/���� �ӵ�.

    [Header("Mapping")]
    public float maxConsideredFallSpeed = 12f;          //����ȭ ����.
    public AnimationCurve strengtByFall =               //���ϼӵ� ( 0 ~ 1) -> ���� ( 0 ~ 1 )
        AnimationCurve.EaseInOut(0f, 0.6f, 1f, 1.0f);   // �����ϵ� ���簨 �ְ�.

    [Header("Impulse")]
    public float amplitudeScale = 0.14f;                // �ִ� ���� �� Y ����(����)
    public float damping = 10f;                         // ���� ���.
    public float oscillationHz = 6.5f;                  // ������ ��.
    public float tiltDegrees = 3.5f;                    // ��ġ ����.

    private bool wasGrounded;                           // �� ������ ���� ����.
    private float timeSinceImpact;                      // ���޽� ��� �ð�.
    private float impactStrength;                       // 0 ~ 1 ����.

    private Vector3 posOffset;                          // ��ġ ������.
    private Vector3 rotOffset;                          // ȸ�� ������.

    public Vector3 CurrentPositionOffset { get { return posOffset; } }
    public Vector3 CurrentRotationOffsetEuler { get { return rotOffset; } }
    public float CurrentFovOffset { get { return 0f; } }

    private void Update()
    {
        if (feed == null)
        {
            return;
        }

        bool grounded = feed.IsGrounded;                // ���� ����.

        if (grounded == true && wasGrounded == false)
        {
            float vyAbs = Mathf.Abs(feed.VerticalVelocity);             // ���� ���� |Vy|
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
