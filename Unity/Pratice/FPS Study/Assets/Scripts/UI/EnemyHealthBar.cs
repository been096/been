using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �� �Ӹ� �� ���� ���� Slider�� HP ǥ��.
/// - LateUpdate���� ī�޶� �ٶ󺸵��� ȸ��(������)
/// - Health.GetCurrentHealth()�� �ֱ������� ����(�̺�Ʈ ��� Ȯ�� ����)
/// </summary>
public class EnemyHealthBar : MonoBehaviour
{
    public Health health;           // ��� Health(�θ𿡼� �ڵ� Ž�� ����)
    public Slider slider;           // ���� ���� �����̴�.
    public Transform followTarget;  // �� Transform�� ī�޶� ������ ȸ��(���� ���� ��Ʈ)
    public Camera mainCamera;       // ���� ī�޶�(�Ϲ������� Main Camera)

    private float maxValue;         // �ִ� ü�� ĳ��(�����̴� ���� ����)

    private void Awake()
    {
        // �ʵ� �ڵ� ����.
        if (health == null)
        {
            health = GetComponentInParent<Health>();
        }
        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>();
        }
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // �����̴� ���� �ʱ�ȭ.
        maxValue = health != null ? health.maxHealth : 100.0f;
        if (slider != null)
        {
            slider.minValue = 0.0f;
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }
    }

    private void LateUpdate()
    {
        // 1) HP �� �ݿ�
        if (health != null && slider != null)
        {
            slider.value = health.GetCurrentHealth();
        }

        // 2) ī�޶� �׻� �ٶ󺸵��� ȸ��(������)
        if (followTarget != null && mainCamera != null)
        {
            Vector3 toCam = mainCamera.transform.position - followTarget.position;
            if (toCam.sqrMagnitude > 0.0001f)
            {
                Quaternion look = Quaternion.LookRotation(toCam.normalized);
                followTarget.rotation = look;
            }
        }
    }
}