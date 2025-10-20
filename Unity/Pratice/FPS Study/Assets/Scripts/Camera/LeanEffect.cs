using UnityEngine;

/// <summary>
/// ��/�� ���ӵ� ��� Lean(��). �ڳʸ�/�޼�ȸ���� ü���� Ȯ �ö�.
/// </summary>
public class LeanEffect : MonoBehaviour, ICameraEffect
{
    [Header("Refs")]
    public LocomotionFeed feed;                     // ���� �ӵ� ���� ����.

    [Header("Params")]
    public float degreesPerMps2 = 0.9f;             // 1 m/s^2 �� ����� ��(��)
    public float maxDegrees = 14.0f;                // �ִ� �����(��)
    public float accelSmooth = 10f;                 // ���ӵ� �������.
    public float response = 14f;                    // ���� ���� EMA ���� �ӵ�.

    private Vector3 lastVel;                        // ���� ������ ���� �ӵ� ����.
    private Vector3 smoothedAccel;                  // ��Ȱȭ�� ���ӵ� ����.
    private float currentDeg;                       // ���� �� ��(��)

    public Vector3 CurrentPositionOffset { get { return Vector3.zero; } }
    public Vector3 CurrentRotationOffsetEuler { get { return new Vector3(0f, 0f, -currentDeg); } }
    public float CurrentFovOffset { get { return 0f; } }
    
    private void Update()
    {
        if (feed == null)
        {
            return;
        }

        float dt = Time.deltaTime;
        Vector3 v = feed.HorizontalVelocity;
        Vector3 a = Vector3.zero;

        a = (v - lastVel) / Mathf.Max(dt, 0.0001f);
        lastVel = v;

        // ���ӵ� �������.
        float alpha = 1f - Mathf.Exp(-accelSmooth * dt); // EMA ���.
        smoothedAccel = Vector3.Lerp(smoothedAccel, a, alpha);

        // ���� ������ ����.
        Vector3 right = feed.PlayerRight;
        float lateralAccel = Vector3.Dot(smoothedAccel, right); // �¿� ���� ����.

        float targetDeg = degreesPerMps2 * lateralAccel;        // ���� -> ���� ����.
        targetDeg = Mathf.Clamp(targetDeg, -maxDegrees, maxDegrees);

        float k = 1f - Mathf.Exp(-response * dt);       //
        currentDeg = Mathf.Lerp(currentDeg, targetDeg, k);
    }
}
