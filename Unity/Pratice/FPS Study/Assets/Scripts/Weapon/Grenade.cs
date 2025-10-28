using UnityEngine;

/// <summary>
/// ����ź : ������ �����ٴϴٰ� �ð� ���� �� ����.
/// - ���� �ݰ濡 ���� ������ ����.
/// - Raycast�� ���� ��ֹ��� ������ ������ ����.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Grenade : MonoBehaviour
{
    [Header("Fuse (Delay)")]
    public float fuseTime = 2.5f;               // ���� �ð�.

    [Header("Explosion")]
    public float radius = 6.0f;                 // ���� �ݰ�.
    public float damageMax = 120.0f;            // �߽ɿ����� �ִ� �����.
    public float damageMin = 10.0f;             // ���� �ڸ� �ּ� �����.
    public LayerMask damageMask;                // ���� ��� ���̾�.
    public LayerMask occlusionMask;             // ���� ���� ���̾�.

    [Header("Effects (Optinal)")]
    public GameObject explosionVfxPrefab;       // ���� ����Ʈ.
    public AudioSource audioSource;             // ���� ���� ����� ���� AudioSource.
    public AudioClip explosionClip;             // ���� ���� Ŭ��.
    public float vfxUpOffset = 0.1f;            // ���鿡 ����Ʈ�� �İ��� ���� �����ϱ� ���� ������.

    private Rigidbody rb;                       // ����ź�� ������ٵ�.
    private float timer;                        // ���� �ð������� Ÿ�̸� üũ�� ����.

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        timer = fuseTime;
    }

    public void Throw(Vector3 velocity, Vector3 angularVelocity)
    {
        if (rb == null)
        {
            return;
        }
        rb.linearVelocity = velocity;
        rb.angularVelocity = angularVelocity;
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        timer = timer - dt;
        if (timer <= 0.0f)
        {
            Explode();
        }
    }

    private void Explode()
    {
        // 1) ����Ʈ/���� ���.
        if (explosionVfxPrefab != null)
        {
            GameObject fx = Instantiate(explosionVfxPrefab, transform.position + Vector3.up * vfxUpOffset,
                Quaternion.identity);
        }
        if (audioSource != null)
        {
            if (explosionClip != null)
            {
                audioSource.PlayOneShot(explosionClip, 1.0f);
            }
        }

        // 2) �ݰ� ���� ��뿡�� ����� ����.
        Collider[] cols = Physics.OverlapSphere(transform.position, radius, damageMask,
            QueryTriggerInteraction.Ignore);

        for (int i = 0; i < cols.Length; i = i + 1)
        {
            Collider c = cols[i];
            if (c == null)
            {
                continue;
            }

            // IDamageable ���� �õ�.
            IDamagealbe id = c.GetComponentInParent<IDamagealbe>();
            if (id == null)
            {
                continue;
            }

            // ���� üũ : ����ź ��ġ -> ��� �߽����� ����ĳ����.
            Vector3 to = c.bounds.center - transform.position;
            float dist = to.magnitude;
            if (dist <= 0.0001f)
            {
                dist = 0.00001f;
            }
            Vector3 dir = to / dist;

            RaycastHit block;
            bool blocked = Physics.Raycast(transform.position, dir, out block, dist, occlusionMask,
                QueryTriggerInteraction.Ignore);

            if (blocked == true)
            {
                // ���󹰿� ������ ��� ó�� ����.
                continue;
            }

            // �Ÿ� ��� ���� ���� : d = 0 -> damageMax. d = radius -> damageMin.
            float t = Mathf.Clamp01(dist / radius);
            float dmg = Mathf.Lerp(damageMax, damageMin, t);

            // ��Ʈ ����/���(����) ���.
            Vector3 hp = c.ClosestPoint(transform.position);
            Vector3 n = (hp - transform.position).normalized;

            id.ApplyDamage(dmg, hp, n, transform);

           
        }
        // 3)
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // ���� �ݰ� �ð�ȭ.
    }
}
