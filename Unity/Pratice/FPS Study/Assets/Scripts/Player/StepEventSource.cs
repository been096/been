using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �̵� �ӵ��� ���� ���� ������ ����ϰ�, �޹�/������ �̺�Ʈ�� ����� �߻�.
/// PlayerController + CharacterController�� �Բ� ����.
/// </summary>
[DisallowMultipleComponent]
public class StepEventSource : MonoBehaviour
{
    [Header("Refs")]
    public CharacterController controller; // ���� �ӵ� ��ȸ��.
    public PlayerController player;        // �ȱ�/�޸��� ���� ����(������ �ӵ������� �Ǵ�)

    [Header("Step Timing")]
    public float walkStepsPerSecond = 1.8f;   // �ȱ� ����(Hz)
    public float sprintStepsPerSecond = 2.6f; // �޸��� ����(Hz)
    public float minSpeedForSteps = 1.0f;     // �� �ӵ� �̸��̸� ���� �߻� X(����/���� ���� �̵� ����)
    public float groundedGraceTime = 0.08f;   // ���� ��Ż �� ª�� �ð� ������ ���� ���(���/�� ����)

    [Header("Events")]
    public UnityEvent onStepLeft;   // �޹�.
    public UnityEvent onStepRight;  // ������.

    private bool leftNext = true;   // ���� ��: true=�޹�, false=������.
    private float nextStepTime;     // ���� ���� �߻� ���� �ð�(����ð�)
    private float lastGroundedTime; // �ֱ� ���� �ð�(������ �ڿ��� �뵵)

    private void Awake()
    {
        // ���� �ڵ� ĳ��(���� ���ᵵ ����)
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }
        if (player == null)
        {
            player = GetComponent<PlayerController>();
        }
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        float now = Time.time;

        bool grounded = false;
        if (controller != null)
        {
            grounded = controller.isGrounded;
        }

        if (grounded == true)
        {
            lastGroundedTime = now;
        }

        // ���� ���� �ӵ�(���� ���� ����)
        float planarSpeed = 0.0f;
        if (controller != null)
        {
            Vector3 v = controller.velocity;
            v.y = 0.0f;
            planarSpeed = v.magnitude;
        }

        // �ּ� �ӵ� �̸��̸� Ÿ�̸� ����(���� ����)
        if (planarSpeed < minSpeedForSteps)
        {
            if (now > nextStepTime)
            {
                nextStepTime = now; // ��� ���� ����.
            }
            return;
        }

        // �ȱ�/�޸��� ���� ����.
        float stepsHz = walkStepsPerSecond;
        if (player != null)
        {
            // ������Ʈ ���δ� PlayerController ���� ���·� �Ǵ�(�ʵ尡 ������ �ӵ� ����)
            // player�� sprintHeld�� private�̸� �ӵ� �������θ� �����ص� ���.
        }
        if (planarSpeed > (minSpeedForSteps + 2.0f))
        {
            stepsHz = sprintStepsPerSecond;
        }

        // ���� ���� �ð� �ʱ�ȭ(���� ���� ��)
        if (nextStepTime <= 0.0f)
        {
            nextStepTime = now + (1.0f / stepsHz);
        }

        // ���� �Ǵ� �ڿ��� �ð� �������� ������ �߻�.
        bool canStep = false;
        if (grounded == true)
        {
            canStep = true;
        }
        else
        {
            if ((now - lastGroundedTime) <= groundedGraceTime)
            {
                canStep = true;
            }
        }

        if (canStep == true)
        {
            if (now >= nextStepTime)
            {
                if (leftNext == true)
                {
                    if (onStepLeft != null)
                    {
                        onStepLeft.Invoke();
                    }
                    leftNext = false;
                }
                else
                {
                    if (onStepRight != null)
                    {
                        onStepRight.Invoke();
                    }
                    leftNext = true;
                }

                nextStepTime = now + (1.0f / stepsHz);
            }
        }
    }
}
