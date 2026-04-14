using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Refs")]
    private CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;

    [Header("Movement")]
    public float walkSpeed = 12f;
    public float gravity = -20f; // 건파이어 리본은 중력이 강해 착지감이 빠름
    public float jumpHeight = 2.5f;

    [Header("Dash")]
    public float dashDistance = 7f;
    public float dashCooldown = 1.5f;
    private float lastDashTime = -100f;

    private Vector3 velocity;
    private Vector2 moveInput;
    private bool isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // 1. 지면 체크 (작은 구체로 바닥 충돌 감지)
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.4f, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 바닥에 붙어있도록 살짝 누름
        }

        // 2. 이동 로직
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * walkSpeed * Time.deltaTime);

        // 3. 중력 적용
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Input System Callbacks
    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && Time.time >= lastDashTime + dashCooldown)
        {
            // 이동 방향으로 순간 이동 (간단한 대시 로직)
            Vector3 dashDir = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;
            if (dashDir == Vector3.zero) dashDir = transform.forward; // 입력 없을 시 정면

            controller.Move(dashDir * dashDistance);
            lastDashTime = Time.time;
        }
    }
}