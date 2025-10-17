using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("Refs")]
    public Transform playerBody;     // Yaw ����(�÷��̾� ��ü)
    public Transform cameraPivot;    // Pitch ����(ī�޶� �θ�)

    [Header("Look")]
    public float mouseSensitivity = 2.5f; // �⺻ ����
    public float pitchMin = -89f;
    public float pitchMax = 89f;

    Vector2 lookInput;
    float yaw;
    float pitch;

    private void Awake()
    {
        if (playerBody == null)
        {
            Debug.LogError("playerBody �Ҵ��� �ʿ��մϴ�.");
        }
        if (cameraPivot == null)
        {
            Debug.LogError("cameraPivot �Ҵ��� �ʿ��մϴ�.");
        }

        // Ŀ�� ���
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        // Look �Է� �ݿ�(������ ���� ����)
        float dx = lookInput.x * mouseSensitivity * 10f * dt;
        float dy = lookInput.y * mouseSensitivity * 10f * dt;

        yaw += dx;
        pitch -= dy;

        // Pitch Ŭ����
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

    // PlayerInput �� Events���� ����
    public void OnLook(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == true || ctx.canceled == true)
        {
            lookInput = ctx.ReadValue<Vector2>();
        }
    }
}