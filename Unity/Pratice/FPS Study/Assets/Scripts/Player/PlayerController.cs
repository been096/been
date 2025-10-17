using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float walkSpeed = 4.5f;
    public float sprintSpeed = 7.5f;
    public float accleration = 15.0f;
    public float deceleration = 20.0f;

    [Header("Jump")]
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f * 2.0f;
    public float coyotetime = 0.12f;        // 공중에 떠있는 시간.
    public float jumpBufferTime = 0.12f;    // 점프 선입력 허용시간
    public LayerMask groundMask;
    public float groundCheckRadius = 0.3f;     // 지면 감지 반지름
    public Transform groundCheck;

    [Header("refs")]
    public Transform cameraPivot;
    public CharacterController controller;

    Vector2 moveInput;
    bool sprintHeld;    // 스프린트키가 누려있냐 안눌려있냐를 체크하기 위한 변수
    float currentSpeed;
    Vector3 velocity;

    bool isGrounded;
    float lastGroundedTime;
    float lasJumpPressTime;

    private void Awake()
    {
        if (groundCheck != null)
        {
            GameObject gc = new GameObject("GroundCheck");
            gc.transform.SetParent(transform);
            gc.transform.localPosition = new Vector3(0.0f, 0.1f, 0.0f);
            groundCheck = gc.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        UpdateGrounded();
        UpdateHorizontalSpeed(dt);
        ApplyGravityAndJump(dt);

        Vector3 move = CalculateWorldMove();
        controller.Move(move * dt);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == true || ctx.canceled == true)
        {
            moveInput = ctx.ReadValue<Vector2>();       // 이전에 무브엑시즈를 썼던 것 처럼 입력값 받아오기
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
            lasJumpPressTime = Time.time;
        }
    }

    public void UpdateGrounded()
    {
        bool hit = false;
        
        if (groundCheck != null)
        {
            Collider[] hits = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);   
            // QueryTriggerInteraction = 트리거 충돌 무시
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
                    velocity.y = -2.0f;
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

    void UpdateHorizontalSpeed(float dt)
    {
        float target = walkSpeed;

        if (sprintHeld == true)
        {
            target = sprintSpeed;
        }

        if (currentSpeed < target)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, target, accleration * dt);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, target, deceleration * dt);
        }
    }

    void ApplyGravityAndJump(float dt)
    {
        bool coyote = (Time.time - lastGroundedTime) <= coyotetime;

        bool buffered = (Time.time - lasJumpPressTime) <= jumpBufferTime;

        if (((buffered == true) && ((isGrounded == true) || (coyote == true))))
        {
            velocity.y = Mathf.Sqrt(Mathf.Abs(2.0f * gravity * jumpHeight));
            lasJumpPressTime = -999.0f;
        }

        velocity.y += gravity * dt;
    }

    Vector3 CalculateWorldMove()
    {
        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;

        if (cameraPivot != null)
        {
            Vector3 camForward = cameraPivot.forward;
            Vector3 camRight = cameraPivot.right;

            camForward.y = 0.0f;
            camRight.y = 0.0f;

            if (camForward.sqrMagnitude > 0.0f)
            {
                camForward.Normalize();
            }

            if (camRight.sqrMagnitude > 0.0f)
            {
                camRight.Normalize();
            }

            forward = camForward;
            right = camRight;
        }

        Vector3 wish = forward * moveInput.y + right * moveInput.x;
        if (wish.sqrMagnitude > 1.0f)
        {
            wish.Normalize();
        }

        Vector3 horizontal = wish * currentSpeed;
        Vector3 move = new Vector3(horizontal.x, velocity.y, horizontal.z);
        return move;
    }
}
