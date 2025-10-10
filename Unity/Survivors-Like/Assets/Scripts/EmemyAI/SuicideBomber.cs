using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SuicideBomber : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2.0f;
    public float chaseRange = 5.0f;          // �÷��̾� Ž�� �Ÿ�
    public float explodeRange = 1.5f;        // ���� �� �Ÿ� ����
    public float stopDistance = 0.2f;

    [Header("Explosion Settings")]
    public int explosionDamage = 40;
    public float explosionDelay = 2.0f;      // ���߱��� ��� �ð�
    public float blinkSpeedStart = 0.5f;     // ���� ���� �ӵ�
    public float blinkSpeedEnd = 0.1f;       // ������ ���� �ӵ�
    public Color warningColor = Color.red;

    [Header("Refs")]
    public Transform player;
    public SpriteRenderer spriteRenderer;
    public EnemyCore core;

    private Rigidbody2D _rb;
    private bool isExploding = false;
    private Coroutine explodeRoutine;

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
                player = pc.transform;
        }

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (!isExploding)
        {
            // �÷��̾� ����
            if (distance <= chaseRange)
            {
                ChasePlayer();

                // ���� �� ���� ������ ����
                if (distance <= explodeRange)
                {
                    explodeRoutine = StartCoroutine(ExplodeSequence());
                }
            }
            else
            {
                // Ž�� ���� ���̸� ����
                _rb.velocity = Vector2.zero;
            }
        }
        else
        {
            // ���� ���� �÷��̾ �־����� ���
            if (distance > chaseRange)
            {
                StopCoroutine(explodeRoutine);
                StartCoroutine(ResetColor());
                isExploding = false;
            }
        }
    }

    void ChasePlayer()
    {
        Vector2 toPlayer = player.position - transform.position;
        float speed = moveSpeed;
        if (core != null)
            speed *= core.externalSpeedMultiplier;

        if (toPlayer.magnitude > stopDistance)
        {
            _rb.velocity = toPlayer.normalized * speed;
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }

    IEnumerator ExplodeSequence()
    {
        isExploding = true;
        _rb.velocity = Vector2.zero;

        float elapsed = 0f;
        float blinkTimer = 0f;
        float currentBlinkSpeed = blinkSpeedStart;
        Color originalColor = spriteRenderer.color;
        bool isRed = false;

        while (elapsed < explosionDelay)
        {
            elapsed += Time.deltaTime;

            // ���� �������� ������
            currentBlinkSpeed = Mathf.Lerp(blinkSpeedStart, blinkSpeedEnd, elapsed / explosionDelay);
            blinkTimer += Time.deltaTime;

            if (blinkTimer >= currentBlinkSpeed)
            {
                blinkTimer = 0f;
                isRed = !isRed;
                spriteRenderer.color = isRed ? warningColor : originalColor;
            }

            yield return null;
        }

        Explode();
    }

    void Explode()
    {
        // ���� ������ ����
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explodeRange);
        foreach (var hit in hits)
        {
            Health h = hit.GetComponent<Health>();
            if (h != null)
            {
                h.TakeDamage(explosionDamage, transform.position);
            }
        }

        // ���� ȿ��, ī�޶� ��鸲 ���� ���� �߰� ����
        // CameraShake.Instance.Shake(0.3f, 0.4f);

        Destroy(gameObject);
    }

    IEnumerator ResetColor()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = originalColor;
        yield return null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explodeRange);
    }
}