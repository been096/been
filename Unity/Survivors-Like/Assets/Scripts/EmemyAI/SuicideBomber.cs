using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SuicideBomber : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2.0f;
    public float chaseRange = 5.0f;          // 플레이어 탐지 거리
    public float explodeRange = 1.5f;        // 폭발 시 거리 판정
    public float stopDistance = 0.2f;

    [Header("Explosion Settings")]
    public int explosionDamage = 40;
    public float explosionDelay = 2.0f;      // 폭발까지 대기 시간
    public float blinkSpeedStart = 0.5f;     // 점멸 시작 속도
    public float blinkSpeedEnd = 0.1f;       // 터지기 직전 속도
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
            // 플레이어 추적
            if (distance <= chaseRange)
            {
                ChasePlayer();

                // 근접 시 폭발 시퀀스 시작
                if (distance <= explodeRange)
                {
                    explodeRoutine = StartCoroutine(ExplodeSequence());
                }
            }
            else
            {
                // 탐지 범위 밖이면 정지
                _rb.velocity = Vector2.zero;
            }
        }
        else
        {
            // 폭발 도중 플레이어가 멀어지면 취소
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

            // 점점 깜빡임이 빨라짐
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
        // 폭발 데미지 판정
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explodeRange);
        foreach (var hit in hits)
        {
            Health h = hit.GetComponent<Health>();
            if (h != null)
            {
                h.TakeDamage(explosionDamage, transform.position);
            }
        }

        // 폭발 효과, 카메라 흔들림 등을 여기 추가 가능
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