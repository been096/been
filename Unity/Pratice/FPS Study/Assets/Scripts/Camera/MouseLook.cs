using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ���콺�� �þ߸� ȸ����Ű�� ��ũ��Ʈ
/// </summary>
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

    public float sentivityMultiplier = 1.0f;

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
        // performed: �б�, canceled: (�е�/Ű) ��ǥ 0���� ����
        if (ctx.performed == true || ctx.canceled == true)
        {
            lookInput = ctx.ReadValue<Vector2>();
        }
    }

    public void SetSensitivityMultiplier(float m)
    {
        // ��û�� ���� �� �纻.
        float requested = m;

        // �ּڰ� ����( �ʹ� ������ �Է��� ���� ���� ��ó�� ������ )
        if (requested < 0.01f)
        {
            requested = 0.01f;
        }

        // �ִ� ����(������ ������ ����ġ ���� ��ȸ�� ����)
        if (requested > 5.0f)
        {
            requested = 5.0f;
        }

        sentivityMultiplier = requested;
    }
}