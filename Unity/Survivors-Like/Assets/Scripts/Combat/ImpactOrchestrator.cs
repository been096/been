using UnityEngine;

/// <summary>
/// Ÿ�� �ǵ�� �Ѱ�: Health.OnDamaged�� �޾Ƽ�
/// Flash/VFX/SFX/Knockback/CameraShake�� ����.
/// �ǰ� ���(�÷��̾�/��) �����տ� ����.
/// </summary>
[RequireComponent(typeof(Health))]
public class ImpactOrchestrator : MonoBehaviour
{
    public DamageFlash flash;              // ������ ����
    public KnockbackReceiver knockback;    // ������ ����

    public CameraShake2D cameraShake;      // ���� ī�޶� ������ �� ����

    public float magnitudeByHpShare = 0.5f; // ����%�� ���� ����ũ/�˹� ������ �⿩
    public float baseKnockback = 4f;              // �⺻ �˹� �ӵ�
    public bool shakeOnHit = true;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
        if (flash == null)
        {
            flash = GetComponentInChildren<DamageFlash>();
        }

        if (knockback == null)
        {
            knockback = GetComponent<KnockbackReceiver>();
        }

        if (cameraShake == null)
        {
            cameraShake = FindAnyObjectByType<CameraShake2D>();
        }
    }

    private void OnEnable()
    {
        if (health != null)
        {
            health.OnDamaged += HandleDamaged;
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.OnDamaged -= HandleDamaged;
        }
    }

    private void HandleDamaged(int amount, Vector3 hitPoint)
    {
        // === 1) ���ؽ�Ʈ ����� ===
        var ctx = new ImpactContext
        {
            target = transform,
            hitPoint = hitPoint,
            damage = amount,
            magnitude = ComputeMagnitude(amount),
            instigator = null // �ʿ��ϸ� AutoAttack �ʿ��� ä���ֵ��� Ȯ�� ����
        };

        // === 2) �ǰ� ���� ===
        // ?�� �ش� ������ null���� �ƴ��� ����� ���� ���־� ��Ʃ����� ���ϰ� ����� �������ִ� �Լ�.
        flash?.PlayFlash();

        // === 5) �˹� ===
        if (knockback != null)
        {
            Vector2 dir = ctx.KnockbackDir2D();
            float strength = baseKnockback * ctx.magnitude;
            knockback.ApplyKnockback(dir * strength);
        }

        // === 6) ī�޶� ����ũ ===
        if (shakeOnHit && cameraShake != null)
        {
            // ������ ������ �������� ���� �� ū ����ũ
            float amp = 0.05f + 0.1f * ctx.magnitude;
            float dur = 0.07f + 0.05f * ctx.magnitude;
            cameraShake.Shake(amp, dur);
        }
    }

    /// <summary>���ط��� Ŭ���� ���� ���⸦ Ű��� ���� ��Ģ.</summary>
    private float ComputeMagnitude(int damage)
    {
        // �ִ�ü�� ��� ���� ����(0~1) * ����ġ
        // Health�� max���� �� �� ������, OnHPChanged�� ĳ���ϰų�
        // ������ ���ط� 10�� 0.1 ������ ���� ��Ģ�� ����. ���⼭�� �ܼ�ȭ:
        float byDamage = Mathf.Clamp01(damage / 20f); // 20������ = 1.0
        return Mathf.Clamp01(byDamage * magnitudeByHpShare + 0.5f * (1f - magnitudeByHpShare));
    }
}
