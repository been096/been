using UnityEngine;

/// <summary>
/// ǥ�� ����ü(�Ѿ�) ��ũ��Ʈ
/// - ���� ������ ���(�̺�Ʈ/���� ����)
/// - �̵�: ������ ���� * �ӵ��� ����
/// - ����: ���� �̵� �Ÿ� >= maxDistance �̸� �Ҹ�
/// - �浹: Enemy�� ������ ������ �ְ� pierce(����) 1 ����
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ProjectileBullet : MonoBehaviour
{
    [Header("Runtime State (�ʱ�ȭ�� ����)")]
    public int damage = 10;           // ������
    public float speed = 8f;          // �̵� �ӵ�(����/��)
    public float maxDistance = 8f;    // �ִ� ��Ÿ�
    public int pierce = 1;            // �� �� ���� ��������(1�̸� 1�� ���߰� �Ҹ�)
    public Vector2 dir = Vector2.right;// ���� ����(����ȭ ����)
    public LayerMask hitMask;         // ���� ���̾�(Enemy)

    private Vector3 _startPos;        // ���� ��ġ(�Ÿ� ����)
    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// �߻� ���Ŀ� �������� ä���ֱ� ���� �ʱ�ȭ �Լ�
    /// </summary>
    public void Init(Vector3 startPos, Vector2 direction, int dmg, float spd, float range, int pierceCount, LayerMask targetMask)
    {
        _startPos = startPos;
        transform.position = startPos;

        dir = direction;
        damage = dmg;
        speed = spd;
        maxDistance = range;
        pierce = pierceCount;
        hitMask = targetMask;

        // ���⿡ ���� ��¦ ȸ��(����)
        if (dir.sqrMagnitude > 0f)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    void Update()
    {
        // 1) ������ �̵�
        Vector3 delta = (Vector3)(dir.normalized * speed * Time.deltaTime);
        transform.position = transform.position + delta;

        // 2) ��Ÿ� üũ
        float traveled = Vector3.Distance(_startPos, transform.position);
        if (traveled >= maxDistance)
        {
            Despawn();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1) ���� ����� hitMask�� ���ϴ��� Ȯ��(���̾� ���)
        int otherLayerBit = 1 << other.gameObject.layer;
        bool layerOk = (hitMask.value & otherLayerBit) != 0;

        if (layerOk == false)
        {
            // ������ �� �Ǵ� ���(��/������/�÷��̾� ��) �� ����
            return;
        }

        // 2) Health�� ã�� ������ �ֱ�(���� ���� ȣ��)
        Health h = other.GetComponent<Health>();
        if (h != null)
        {
            // ���� ������ ����
            h.TakeDamage(damage, other.transform.position);
        }

        // 3) ���� �� ����
        pierce = pierce - 1;

        if (pierce <= 0)
        {
            Despawn();
        }
    }

    void Despawn()
    {
        // Ǯ �Ŵ����� ������ Ǯ ��ȯ, ������ Destroy
        if (PoolManager.Instance != null)
        {
            gameObject.SetActive(false); // �ܼ� ����(Ű ���� Release�� ���Ŀ�)
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
