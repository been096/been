using UnityEngine;
using TMPro;

/// <summary>
/// [�뵵] ī�޶� Ʃ�� �� �ٽ� ��ǥ HUD(�ӵ�, ����, FOV, Lean ��) ǥ��.
/// [UI] Canvas�� 'CamHudLabel'(TextMeshProUGUI) ���� �� label�� ����.
/// </summary>
public class CameraDebugHud : MonoBehaviour
{
    public TextMeshProUGUI label;     // label: ��¿� �ؽ�Ʈ UI.
    public LocomotionFeed feed;       // feed: �ӵ�/���� ������.
    public Camera targetCamera;       // targetCamera: FOV ��ȸ.
    public LeanEffect lean;           // lean: Lean ���� Ȯ��(����)

    private void Awake()
    {
        if (label == null)
        {
            GameObject t = GameObject.Find("CamHudLabel"); // t: ������ ã�� TMP ������Ʈ.
            if (t != null)
            {
                label = t.GetComponent<TextMeshProUGUI>();
            }
        }

        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (label == null)
        {
            return;
        }

        float speed = 0f;        // speed: ���� �ӵ�(m/s)
        bool grounded = false;   // grounded: ���� ����.
        float fov = 0f;          // fov: ī�޶� �þ߰�(��)
        float leanDeg = 0f;      // leanDeg: �¿� ����� ��(��)

        if (feed != null)
        {
            speed = feed.HorizontalSpeed;
            grounded = feed.IsGrounded;
        }

        if (targetCamera != null)
        {
            fov = targetCamera.fieldOfView;
        }

        if (lean != null && targetCamera != null)
        {
            float z = targetCamera.transform.localRotation.eulerAngles.z; // z: ���� Z ���Ϸ�(0~360)
            if (z > 180f)
            {
                z -= 360f;
            }
            leanDeg = -z; // LeanEffect���� -currentDeg ���������Ƿ� ��ȣ ����.
        }

        label.text = $"Speed {speed:0.00} m/s | Grounded {(grounded == true ? "Y" : "N")} | FOV {fov:0.0}�� | Lean {leanDeg:0.0}��";
    }
}