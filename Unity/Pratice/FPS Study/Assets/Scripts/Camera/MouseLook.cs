using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 마우스로 시야를 회전시키는 스크립트
/// </summary>
public class MouseLook : MonoBehaviour
{
    [Header("Refs")]
    public Transform playerBody;     // Yaw 적용(플레이어 본체)
    public Transform cameraPivot;    // Pitch 적용(카메라 부모)

    [Header("Look")]
    public float mouseSensitivity = 2.5f; // 기본 감도
    public float pitchMin = -89f;
    public float pitchMax = 89f;

    Vector2 lookInput;
    float yaw;
    float pitch;

    public float sentivityMultiplier = 1.0f;

    private void Awake()
    {
        if (playerBody == null)
        {
            Debug.LogError("playerBody 할당이 필요합니다.");
        }
        if (cameraPivot == null)
        {
            Debug.LogError("cameraPivot 할당이 필요합니다.");
        }

        // 커서 잠금
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        // Look 입력 반영(프레임 독립 보정)
        float dx = lookInput.x * mouseSensitivity * 10f * dt;
        float dy = lookInput.y * mouseSensitivity * 10f * dt;

        yaw += dx;
        pitch -= dy;

        // Pitch 클램프
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        if (playerBody != null)
        {
            Quaternion yRot = Quaternion.Euler(0f, yaw, 0f);
            playerBody.rotation = yRot;
        }
        if (cameraPivot != null)
        {
            Quaternion xRot = Quaternion.Euler(pitch, 0f, 0f);
            cameraPivot.localRotation = xRot;
        }

    }

    // PlayerInput → Events에서 연결
    public void OnLook(InputAction.CallbackContext ctx)
    {
        // performed: 읽기, canceled: (패드/키) 좌표 0으로 복귀
        if (ctx.performed == true || ctx.canceled == true)
        {
            lookInput = ctx.ReadValue<Vector2>();
        }
    }

    public void SetSensitivityMultiplier(float m)
    {
        // 요청된 배율 값 사본.
        float requested = m;

        // 최솟값 보정( 너무 작으면 입력이 거의 멈춘 것처럼 느껴짐 )
        if (requested < 0.01f)
        {
            requested = 0.01f;
        }

        // 최댓값 보정(과도한 배율은 예기치 않은 급회전 유발)
        if (requested > 5.0f)
        {
            requested = 5.0f;
        }

        sentivityMultiplier = requested;
    }
}