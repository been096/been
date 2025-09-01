using UnityEngine;

/// <summary>
/// �Ѿ��� ���� ����� �� Burn/Slow ���¸� '�ο�'�ϴ� ������.
/// - ProjectileBullet�� ���� ������Ʈ�� �Բ� ���δ�.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class OnHitStatusApplier : MonoBehaviour
{
    [Header("Target Filter")]
    public LayerMask targetMask; // Enemy ���̾ üũ

    [Header("Burn Settings")]
    public bool applyBurn = true;
    public int burnTickDamage = 2;
    public float burnDuration = 3f;
    public float burnTickInterval = 0.5f;

    [Header("Slow Settings")]
    public bool applySlow = true;
    public float slowPercent = 30f;   // 30% ����
    public float slowDuration = 2f;

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1) Ÿ�� ���̾����� Ȯ��
        int otherLayerBit = 1 << other.gameObject.layer;
        bool layerOk = (targetMask.value & otherLayerBit) != 0;
        if (layerOk == false)
        {
            return;
        }

        // 2) Burn �ο�
        if (applyBurn == true)
        {
            BurnStatus burn = other.GetComponent<BurnStatus>();
            if (burn == null)
            {
                burn = other.gameObject.AddComponent<BurnStatus>();
            }
            burn.Apply(burnTickDamage, burnDuration, burnTickInterval);
        }

        // 3) Slow �ο�
        if (applySlow == true)
        {
            SlowStatus slow = other.GetComponent<SlowStatus>();
            if (slow == null)
            {
                slow = other.gameObject.AddComponent<SlowStatus>();
            }
            slow.Apply(slowPercent, slowDuration);
        }
    }
}