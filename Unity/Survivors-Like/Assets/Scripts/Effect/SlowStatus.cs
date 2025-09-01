using UnityEngine;

/// <summary>
/// Slow(����) ����
/// - ���� ��: externalSpeedMultiplier *= (1 - slowPercent/100)
/// - ���� ��: externalSpeedMultiplier /= �����ߴ� ����   (��Ȯ ����)
/// - ������: �� �� �ۼ�Ʈ�� ���� ���Ƴ����, �ƴϸ� �ð��� ��������
/// </summary>
public class SlowStatus : MonoBehaviour
{
    [Header("Runtime")]
    public float currentPercent = 0f;  // ���� ���� ���� �ۼ�Ʈ(0~100)
    public float duration = 0f;

    private float appliedFactor = 1f; // ���ص� ����(��: 0.7)
    private bool applied = false;

    private EnemyCore core;           // �̵� �ӵ��� ������ ����ϴ� ��

    void Awake()
    {
        core = GetComponent<EnemyCore>();
        // EnemyCore�� ���ٸ� ������ ������ �� ����(������ �׳� ����).
        // �ʿ��ϸ� Player���� ����ϰ� ������ �� �ִ�.
    }

    public void Apply(float newPercent, float newDuration)
    {
        if (core == null)
        {
            // ���� ��� EnemyCore�� ������ �ƹ� �͵� ���� ����
            return;
        }

        float clamped = Mathf.Clamp(newPercent, 0f, 95f); // 100%�� �����̶� ����, 95% ������ ����
        float newFactor = 1f - (clamped / 100f);

        if (applied == false)
        {
            // ó�� ����
            MultiplySpeed(newFactor);   // ���ϱ�
            appliedFactor = newFactor; // ���
            currentPercent = clamped;
            duration = newDuration;
            applied = true;
        }
        else
        {
            // �̹� � ���ο찡 ���� ��
            if (clamped > currentPercent)
            {
                // �� �� ���ο�� ��ü: ���� ���� ������ �ǵ�����, �� ������ ���Ѵ�
                DivideSpeed(appliedFactor);   // �ǵ�����(������)
                MultiplySpeed(newFactor);      // ���� ���ϱ�
                appliedFactor = newFactor;
                currentPercent = clamped;
                duration = newDuration;        // �ð� ��������
            }
            else
            {
                // �� ���ϰų� ������ �ð��� ��������
                duration = newDuration;
            }
        }
    }

    void Update()
    {
        if (applied == true)
        {
            if (duration > 0f)
            {
                duration = duration - Time.deltaTime;
            }
            else
            {
                // ����: ��Ȯ�� ����
                DivideSpeed(appliedFactor);
                applied = false;
                Destroy(this);
            }
        }
    }

    void OnDisable()
    {
        // ��Ȱ��ȭ�� ���� ������ ����(�� ��ε�/������ �ı� ���)
        if (applied == true)
        {
            DivideSpeed(appliedFactor);
            applied = false;
        }
    }

    private void MultiplySpeed(float factor)
    {
        // EnemyCore ���� '���� �̵� �ӵ�' ��� �� �Ʒ�ó�� ���� ���� �־�� �Ѵ�:
        // float speed = baseMoveSpeed * externalSpeedMultiplier;
        core.externalSpeedMultiplier = core.externalSpeedMultiplier * factor;
    }

    private void DivideSpeed(float factor)
    {
        if (factor != 0f)
        {
            core.externalSpeedMultiplier = core.externalSpeedMultiplier / factor;
        }
    }
}