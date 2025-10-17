using UnityEngine;
using UnityEngine.InputSystem;

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
        if (ctx.performed == true || ctx.canceled == true)
        {
            lookInput = ctx.ReadValue<Vector2>();
        }
    }
}