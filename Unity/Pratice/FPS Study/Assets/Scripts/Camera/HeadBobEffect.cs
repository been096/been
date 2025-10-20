using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;
public class HeadBobEffect : MonoBehaviour, ICameraEffect
{
    [Header("Refs")]
    public LocomotionFeed feed;                         // �ӵ� / ���� ������.

    [Header("Shape")]
    public float frequencyPerMps = 2.2f;                // 1 m/s�� ���ļ� ������.
    public AnimationCurve amplitudeBySpeed =            // �ӵ� -> ���� ������ � (0 ~ 10m/s ����)
        AnimationCurve.EaseInOut(0f, 1.2f, 6f, 1.0f);   //���ӿ��� ����, ��ӿ��� �ϸ�.

    public float baseAmplitudeX = 0.05f;                //�⺻ �¿� ����(����)
    public float baseAmplitudeY = 0.035f;               //�⺻ ���� ����(����)

    public float runSpeedThreshold = 4.5f;              //�޸��� �Ӱ�.
    public float airborneDamping = 6f;
    public float smooth = 16f;

    [Header("Step Events")]
    public UnityEvent onStepLeft;
    public UnityEvent onStepRight;

    private float phase;
    private Vector3 offset;
    private bool leftFootNext = true;
    private float lastCycle;

    public Vector3 CurrentPositionOffset { get { return offset; } }
    public Vector3 CurrentRotationOffsetEuler { get { return Vector3.zero; } }
    public float CurrentFovOffset { get { return 0f; } }

    
    // Update is called once per frame
    void Update()
    {
        if (feed == null)
        {
            return;
        }

        float dt = Time.deltaTime;                      // ������ �ð�.
        float speed = feed.HorizontalSpeed;             // ���� �ӵ�.

        float freq = frequencyPerMps * speed;           // �ӵ� ��� ���ļ�(hz ����)
        phase += freq * dt * Mathf.PI * 2f;             // ���� ������Ʈ.

        // ����Ŭ �ε���(0..1..2..): ���� Ÿ�̹� ���.
        float cycle = phase / (Mathf.PI * 2f);          // ���� ���� ����Ŭ ��.
        if (Mathf.FloorToInt(cycle) > Mathf.FloorToInt(lastCycle))
        {
            if (leftFootNext == true)
            {
                if (onStepLeft != null)
                {
                    onStepLeft.Invoke();
                }
            }
            else
            {
                if (onStepRight != null)
                {
                    onStepRight.Invoke();
                }
            }
            leftFootNext = !leftFootNext;
        }
        lastCycle = cycle;

        //�ӵ� ��� ���� ������.
        float ampScale = 1f;
        float curveEval = amplitudeBySpeed.Evaluate(speed); // �ӵ� -> ������.
        ampScale *= curveEval;

        if (speed > runSpeedThreshold)
        {
            ampScale *= 1.15f;  //�޸��⿡�� ��¦ ����.
        }

        Vector3 target = Vector3.zero;                      // ��ǥ ������.

        if (feed.IsGrounded == true)
        {
            float x = Mathf.Sin(phase) * baseAmplitudeX * ampScale;             // �¿�
            float y = Mathf.Abs(Mathf.Sin(phase * 2f)) * baseAmplitudeY * ampScale; // ����(�йڰ�)
            target = new Vector3(x, -y, 0f);
        }
        else
        {
            target = Vector3.zero;
        }

        if (feed.IsGrounded == true)
        {
            float k = 1f - Mathf.Exp(-smooth * dt);         // ���� ���� ���.  Exp = �ŵ� ���� ��ȯ �Լ�
            offset = Vector3.Lerp(offset, target, k);
        }
        else
        {
            float k = 1f - Mathf.Exp(-airborneDamping * dt);    // ���� ���� ���.
            offset = Vector3.Lerp(offset, Vector3.zero, k);
        }
    }
}
