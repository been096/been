using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float walkSpeed = 4.5f;          // �⺻ �ȱ� �ְ� �ӵ�(m/s)
    public float sprintSpeed = 7.5f;        // �޸��� �ְ� �ӵ�(m/s)
    public float acceleration = 15.0f;      // ����(m/s^2)
    public float deceleration = 20.0f;      // ����(m/s^2)

    [Header("Jump")]
    public float jumpHeight = 1.2f;         // ���� ����(m)
    public float gravity = -9.81f * 2.0f;   // �߷�(����)
    public float coyoteTime = 0.12f;        // �ڿ��� �ð�(��)
    public float jumpBufferTime = 0.12f;    // ���� ����(��)
    public LayerMask groundMask;            // ���� ���̾�.
    public float groundCheckRadius = 0.3f;  // ���� ��ü �ݰ�.
    public Transform groundCheck;           // ���� ������(������ ����)

    [Header("Refs")]
    public Transform cameraPivot;           // �þ� ����(���� ī�޶� �θ�)
    public CharacterController controller;  // CC (������ Awake���� �ڵ� ĳ��)

    public GroundingStabilizer stabilizer;

    private Vector2 moveInput;              // �̵� �Է�(��ƽ/Ű)
    private bool sprintHeld;                // �޸��� Ű ����.
    private float currentSpeed;             // ���� ��ǥ ���� �ӵ�(�Է� ũ�� ������)
    private Vector3 velocity;               // ���� �ӵ�(�߷� ����)

    private bool isGrounded;                // ���� ����.
    private float lastGroundedTime;         // �ֱ� ���� �ð�.
    private float lastJumpPressTime;        // �ֱ� ���� �Է� �ð�.

    private void Awake()
    {
        // ���: CC �ڵ� ĳ��.
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }

        // ���� ������ �ڵ� ����.
        if (groundCheck == null)
        {
            GameObject gc = new GameObject("GroundCheck");
            gc.transform.SetParent(transform);
            gc.transform.localPosition = new Vector3(0.0f, 0.1f, 0.0f);
            groundCheck = gc.transform;
        }
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        UpdateGrounded();
        UpdateHorizontalSpeed(dt);
        ApplyGravityAndJump(dt);

        Vector3 move = CalculateWorldMove();     // m/s

        // ��� / ��ܿ��� ���� ó��.
        if (stabilizer != null)
        {
            move = stabilizer.ProjectOnGround(move, controller, this);
            move = stabilizer.ApplyStepSmoothing(move, controller, this);
        }

        controller.Move(move * dt);              // ������ ������ ��ȯ �� �̵�.

        if (stabilizer != null)
        {
            if (controller.velocity.y <= 0.0f)
            {
                stabilizer.TrySnapDown(controller, this);
            }
        }

    }

    // ===== Input (New Input System) =====

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == true || ctx.canceled == true)
        {
            moveInput = ctx.ReadValue<Vector2>();
        }
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == true)
        {
            sprintHeld = true;
        }

        if (ctx.canceled == true)
        {
            sprintHeld = false;
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == true)
        {
            lastJumpPressTime = Time.time;
        }
    }

    // ===== Grounded =====

    private void UpdateGrounded()
    {
        bool hit = false;

        if (groundCheck != null)
        {
            Collider[] hits = Physics.OverlapSphere(
                groundCheck.position,
                groundCheckRadius,
                groundMask,
                QueryTriggerInteraction.Ignore);

            if (hits != null)
            {
                if (hits.Length > 0)
                {
                    hit = true;
                }
            }
        }

        if (hit == true)
        {
            if (isGrounded == false)
            {
                if (velocity.y < 0.0f)
                {
                    velocity.y = -2.0f; // ���� ���� �� �ް��� ����.
                }
            }

            isGrounded = true;
            lastGroundedTime = Time.time;
        }
        else
        {
            isGrounded = false;
        }
    }

    // ===== Horizontal Speed =====

    private void UpdateHorizontalSpeed(float dt)
    {
        // ������ 1: �Է� ũ�⿡ ���� '��ǥ �ӵ�'�� 0~�ִ���� ����.
        //  - �Ƴ��α� �Է��� �� "�̵��� �ʹ� �۴�" ü�� ����.
        float inputMag = moveInput.magnitude; // 0~1

        float baseTarget = walkSpeed;
        if (sprintHeld == true)
        {
            baseTarget = sprintSpeed;
        }

        float target = baseTarget * inputMag; // �Է� ũ�� �ݿ�.

        // ������
        if (currentSpeed < target)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, target, acceleration * dt);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, target, deceleration * dt);
        }
    }

    // ===== Gravity & Jump =====

    private void ApplyGravityAndJump(float dt)
    {
        bool coyote = (Time.time - lastGroundedTime) <= coyoteTime;
        bool buffered = (Time.time - lastJumpPressTime) <= jumpBufferTime;

        if ((buffered == true) && ((isGrounded == true) || (coyote == true)))
        {
            float vJump = Mathf.Sqrt(Mathf.Abs(2.0f * gravity * jumpHeight)); // v = ��(2gh)
            velocity.y = vJump;
            lastJumpPressTime = -999.0f;
        }

        velocity.y += gravity * dt;
    }

    // ===== Move Vector =====

    private Vector3 CalculateWorldMove()
    {
        // ������ 2: ī�޶� ��� ������ '�ʹ� ������' �÷��̾� ����/�������� ����.
        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;

        if (cameraPivot != null)
        {
            Vector3 camForward = cameraPivot.forward;
            Vector3 camRight = cameraPivot.right;

            camForward.y = 0.0f;
            camRight.y = 0.0f;

            float lenF = camForward.magnitude;
            float lenR = camRight.magnitude;

            if (lenF > 0.0001f)
            {
                camForward /= lenF;
            }

            if (lenR > 0.0001f)
            {
                camRight /= lenR;
            }

            // �Ӱ�ġ ����(���� ���� �þ� ��)�� ���� ����.
            if (camForward.sqrMagnitude < 0.0001f || camRight.sqrMagnitude < 0.0001f)
            {
                Vector3 bodyF = transform.forward;
                Vector3 bodyR = transform.right;

                bodyF.y = 0.0f;
                bodyR.y = 0.0f;

                if (bodyF.sqrMagnitude > 0.0f)
                {
                    bodyF.Normalize();
                }
                if (bodyR.sqrMagnitude > 0.0f)
                {
                    bodyR.Normalize();
                }

                forward = bodyF;
                right = bodyR;
            }
            else
            {
                forward = camForward;
                right = camRight;
            }
        }

        // ��� ����.
        Vector3 wish = forward * moveInput.y + right * moveInput.x;

        // ����ȭ: Ű���� �밢��(��2) ����.
        if (wish.sqrMagnitude > 1.0f)
        {
            wish.Normalize();
        }

        // ���� �ӵ� ���� = ���� �� ���� �ӵ�.
        Vector3 horizontal = wish * currentSpeed;

        // ���� �̵� �ӵ�(m/s)
        Vector3 move = new Vector3(horizontal.x, velocity.y, horizontal.z);
        return move;
    }
}