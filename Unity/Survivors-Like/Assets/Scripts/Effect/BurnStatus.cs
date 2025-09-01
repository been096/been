using UnityEngine;

/// <summary>
/// Burn(���� ����) ����
/// - tickInterval���� tickDamage�� �ش�.
/// - duration�� ������ ���¸� �����Ѵ�.
/// - ������ ��: duration ���� ����, tickDamage�� �� ū �� ����.
/// </summary>
public class BurnStatus : MonoBehaviour
{
    public SpriteRenderer sr;
    public Color color;
    
    [Header("Runtime")]
    public int tickDamage = 1;
    public float duration = 0f;
    public float tickInterval = 0.5f;

    private float _tickTimer = 0f;

    public void Apply(int newTickDamage, float newDuration, float newTickInterval)
    {
        // ƽ ������� �� ū �� ����
        if (newTickDamage > tickDamage)
        {
            tickDamage = newTickDamage;
        }

        // ƽ ������ �� �� ���(���� �ܼ�ȭ�� ����)
        tickInterval = newTickInterval;

        // ���� �ð��� ���� ����(��������)
        duration = newDuration;

        // �ٷ� ƽ�� ���� �ʰ�, Ÿ�̸Ӹ� �ʱ�ȭ
        _tickTimer = tickInterval;
    }

    void Update()
    {
       
        if (duration > 0f)
        {
            if (sr == null)
            {
                sr = GetComponent<SpriteRenderer>();
                if (sr != null)
                    color = sr.color;
            }

            duration = duration - Time.deltaTime;

            _tickTimer = _tickTimer - Time.deltaTime;
            if (_tickTimer <= 0f)
            {
                // ������ 1ƽ
                Health h = GetComponent<Health>();
                if (h != null)
                {
                    h.TakeDamage(tickDamage, h.transform.position);
                    if (sr != null)
                        sr.color = Color.red;
                }
                _tickTimer = tickInterval;
                Invoke("RestoreColor", 0.5f);
            }
        }
        else
        {
            // �ð� ���� �� ���� ����
            if (sr != null)
            {
                sr.color = color;
            }
            
            Destroy(this);
        }
    }

    public void RestoreColor()
    {
        if (sr != null)
        {
            sr.color = color;
        }
    }
}