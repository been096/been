using UnityEngine;

/// <summary>
/// - ���� ��Ÿ� �ȿ��� ���߰� �ֱ������� �߻�.
/// - �ʹ� ������ �ڷ� ��¦ ������.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyRangedAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 1.6f;
    public float desiredRange = 5.0f;        // �� �Ÿ����� ����.
    public float tooCloseRange = 2.2f;       // �̺��� ������ ����.
    public float stopBand = 0.3f;            // ���� �����׸��ý�.

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public LayerMask projectileHitMask;
    public int projectileDamage = 6;
    public float projectileSpeed = 7f;
    public float projectileRange = 7f;
    public int projectilePierce = 1;
    public float fireCooldown = 1.25f;

    [Header("Refs")]
    public Transform player;
    public EnemyCore core;

    private Rigidbody2D _rb;
    private float fireTimer = 0f;

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
        if (fireTimer > 0f)
        {
            fireTimer = fireTimer - Time.deltaTime;
        }

        if (player == null)
        {
            _rb.velocity = Vector2.zero;
            return;
        }

        Vector2 toPlayer = player.position - transform.position;
        float dist = toPlayer.magnitude;

        float speed = moveSpeed;
        if (core != null)
        {
            speed = moveSpeed * core.externalSpeedMultiplier;
        }

        // 1) �Ÿ� ��� �̵�.
        if (dist > desiredRange + stopBand)
        {
            // �ʹ� �ִ� -> ����.
            _rb.velocity = toPlayer.normalized * speed;
        }
        else if (dist < tooCloseRange)
        {
            // �ʹ� ������ -> ����.
            _rb.velocity = (-toPlayer.normalized) * speed;
        }
        else
        {
            // ���� �Ÿ��� -> ����.
            _rb.velocity = Vector2.zero;
        }

        // 2) ���.
        if (dist <= desiredRange + 0.1f)
        {
            if (fireTimer <= 0f)
            {
                FireOnce(toPlayer);
                fireTimer = fireCooldown;
            }
        }
    }

    void FireOnce(Vector2 toPlayer)
    {
        Vector2 dir = Vector2.right;
        if (toPlayer.sqrMagnitude > 0.0001f)
        {
            dir = toPlayer.normalized;
        }

        Vector3 spawnPos;
        if (firePoint != null)
        {
            spawnPos = firePoint.position;
        }
        else
        {
            spawnPos = transform.position;
        }

        GameObject go = null;
        if (PoolManager.Instance != null)
        {
            go = PoolManager.Instance.Spawn(projectilePrefab, spawnPos, Quaternion.identity, null);
        }
        else
        {
            go = GameObject.Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        }

        ProjectileBullet b = go.GetComponent<ProjectileBullet>();
        if (b == null)
        {
            b = go.AddComponent<ProjectileBullet>();
        }
        b.Init(spawnPos, dir, projectileDamage, projectileSpeed, projectileRange, projectilePierce, projectileHitMask);
    }
}