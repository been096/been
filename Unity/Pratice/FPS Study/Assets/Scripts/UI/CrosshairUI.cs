using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 4���� �̹���(��/��/��/��)�� ����� Ȯ���� �ð�ȭ.
/// �̵� �ӵ��� ��� �̺�Ʈ�� ������ Ȯ�갪�� ����/����.
/// </summary>
public class CrosshairUI : MonoBehaviour
{
    [Header("Refs")]
    public CharacterController controller;  // �̵��ӵ� ����.
    public RectTransform topPart;
    public RectTransform bottomPart;
    public RectTransform leftPart;
    public RectTransform rightPart;

    [Header("Spread")]
    public float baseSpread = 6f;           // �⺻ ����(px)
    public float moveSpreadFactor = 10f;    // �̵� �ӵ��� ����(px per m/s)
    public float fireKick = 12f;            // �߻� �� ��� ����(px)
    public float decaySpeed = 40f;          // �ʴ� ����(px/s)
    public float maxSpread = 40f;           // �ִ� Ȯ��(px)

    private float currentSpread;            // ���� Ȯ��(px)

    private void Awake()
    {
        if (controller == null)
        {
            controller = FindAnyObjectByType<CharacterController>();
        }
        currentSpread = baseSpread;
    }
    
    // Update is called once per frame
    void Update()
    {
        // 1) �̵� �ӵ� ��� ��ǥ Ȯ��.
        float planarSpeed = 0.0f;
        if (controller != null)
        {
            Vector3 v = controller.velocity;
            v.y = 0.0f;
            planarSpeed = v.magnitude;
        }

        float targetSpread = baseSpread + moveSpreadFactor * planarSpeed;

        // 2) ���� Ȯ���� targetSpread ������ ���� �̵�.
        if (currentSpread > targetSpread)
        {
            currentSpread -= decaySpeed * Time.deltaTime;
            if (currentSpread < targetSpread)
            {
                currentSpread = targetSpread;
            }
        }
        else
        {
            currentSpread += decaySpeed * Time.deltaTime;
            if (currentSpread > targetSpread)
            {
                currentSpread = targetSpread;
            }
        }

        if (currentSpread > maxSpread)
        {
            currentSpread = maxSpread;
        }

        // 3) UI �ݿ�.
        ApplySpread(currentSpread);
    }

    public void PulseFireSpread()
    {
        currentSpread += fireKick;
        if (currentSpread > maxSpread)
        {
            currentSpread = maxSpread;
        }
    }

    private void ApplySpread(float s)
    {
        if (topPart != null)
        {
            topPart.anchoredPosition = new Vector2(0.0f, s);
        }
        if (bottomPart != null)
        {
            bottomPart.anchoredPosition = new Vector2(0.0f, -s);
        }
        if (leftPart != null)
        {
            leftPart.anchoredPosition = new Vector2(-s, 0.0f);
        }
        if (rightPart != null)
        {
            rightPart.anchoredPosition = new Vector2(s, 0.0f);
        }
    }
}
