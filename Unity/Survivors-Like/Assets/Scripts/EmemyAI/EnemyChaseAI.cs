using UnityEngine;

/// <summary>
/// - �÷��̾ ���� �̵�.
/// - ���� �ݰ� ���̸� �ֱ������� ���� ����.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChaseAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2.0f;           // ���� �̵��� externalSpeedMultiplier�� ���� ���� �� �̻���.
    public float stopDistance = 0.2f;        // �ʹ� ������ �� �������� ����.

    [Header("Contact Damage")]
    public int contactDamage = 5;
    public float contactRadius = 0.4f;
    public float damageCooldown = 0.6f;

    [Header("Refs")]
    public Transform player;                 // Player.
    public EnemyCore core;                   // externalSpeedMultiplier ���.

    private Rigidbody2D _rb;
    private float _damageTimer = 0f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (core == null)
        {
            core = GetComponent<EnemyCore>();
        }
    }

    void Start()
    {
        if (player == null)
        {
            PlayerCore pc = FindAnyObjectByType<PlayerCore>();
            if (pc != null)
            {
                player = pc.transform;
            }
        }
    }

    void Update()
    {
        if (_damageTimer > 0f)
        {
            _damageTimer = _damageTimer - Time.deltaTime;
        }

        if (player == null)
        {
            return;
        }

        // 1) �̵�.
        Vector2 toPlayer = player.position - transform.position;    // ������ �˾Ƴ��� ���� ����.
        float dist = toPlayer.magnitude;

        float speed = moveSpeed;
        if (core != null)
        {
            speed = moveSpeed * core.externalSpeedMultiplier;
        }

        if (dist > stopDistance)
        {
            Vector2 dir = toPlayer.normalized;  // ���� ����ȭ. ������ ũ�⸦ 1�� �����. ���� ������ ����ϱ� ���ؼ�.
            Vector2 v = dir * speed;
            _rb.velocity = v;
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }

        // 2) ���� ���� (�� �ȿ� ������)
        if (dist <= contactRadius)
        {
            if (_damageTimer <= 0f)
            {
                // �÷��̾� Health�� ����.
                Health h = player.GetComponent<Health>();
                if (h != null)
                {
                    h.TakeDamage(contactDamage, player.transform.position);
                }
                _damageTimer = damageCooldown;
            }
        }
    }
}