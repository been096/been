using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

/// <summary>
/// ���� �̺�Ʈ �� ���� ���޽��� ������ ī�޶� Pivot�� ���� ª�� ����.
/// Cinemachine ���̵� ������ ��� ����(�ֹ� ���� : ����/�ð��� ª�� ����).
/// </summary>
[DisallowMultipleComponent]
public class SimpleCameraImpulse : MonoBehaviour
{
    public Transform target;                // ��� ���(���� CameraPivot)
    public float positionAmplitude = 0.02f; // �ִ� ��ġ ������(m)
    public float rotationAmplitude = 0.6f;  // �ִ� ȸ�� ������(��)
    public float duration = 0.08f;          // ���޽� ���� �ð�(��)

    private float timeLift;                 // ���� �ð�.
    private Vector3 baseLocalPos;           // ���� ���� ��ġ ���.
    private Quaternion baseLocalRot;        // ���� ���� ȸ�� ���.
    private bool initialized;               // �ʱ�ȭ ����.

    private void Awake()
    {
        if (target == null)
        {
            target = transform; // �ڱ� �ڽ��� Pivot�� ���� ����.
        }
    }

    private void OnEnable()
    {
        if (target != null)
        {
            baseLocalPos = target.localPosition;
            baseLocalRot = target.localRotation;
            initialized = true;
        }
    }

    private void Update()
    {
        if (initialized == false)
        {
            return;
        }

        // �ð��� ���� ������ �����ϸ� ����.
        if (timeLift > 0.0f)
        {
            float t = timeLift / duration;          // 1 -> 0
            float falloff = Mathf.SmoothStep(0.0f, 1.0f, t);    // �ε巯�� ����.

            // ������ ���� ��� ��鸲(�����Ӹ��� �޶���)
            Vector3 posJitter = new Vector3(
                Random.Range(-positionAmplitude, positionAmplitude),
                Random.Range(-positionAmplitude, positionAmplitude),
                Random.Range(-positionAmplitude, positionAmplitude)
            ) * falloff;

            Vector3 rotJitterEuler = new Vector3(
                Random.Range(-rotationAmplitude, rotationAmplitude),
                Random.Range(-rotationAmplitude, rotationAmplitude),
                0.0f
            ) * falloff;

            target.localPosition = baseLocalPos + posJitter;
            target.localRotation = baseLocalRot * Quaternion.Euler(rotJitterEuler);

            timeLift -= Time.deltaTime;
            if (timeLift <= 0.0f)
            {
                // ���� �� ����ġ ����
                target.localPosition = baseLocalPos;
                target.localRotation = baseLocalRot;
            }
        }
    }

    /// <summary>
    /// �ܺο��� ȣ�� : ���޽��� 1ȸ Ʈ����.
    /// </summary>
    public void Pulse()
    {
        // ���� ���� �� ����.
        if (initialized == true)
        {
            timeLift = duration;
        }
    }
}
