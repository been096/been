using UnityEngine;

public class XpOrb : MonoBehaviour
{
    public int xpValue = 3;             // �� ���갡 �ִ� ����ġ
    public float moveSpeed = 6f;        // ���� �� �ӵ�
    public float catchDistance = 0.25f; // �� �Ÿ� ������ ���� ���� �ɷ� ó��

    // ���� ����
    private bool attracted = false;     // �ڼ��� ������ ���ΰ�
    private Transform target;           // �ڼ� �߽�(���� Player�� Magnet)
    private XpSystem xpSystem;          // ����ġ�� �ø� ���
    private GameObject keyPrefab;

    private void Start()
    {
        Invoke("DestroyOrb", 60.0f);
    }

    void Update()
    {
        if (attracted && target != null)
        {
            // Ÿ�� ������ ���ݾ� �̵�
            Vector3 p = transform.position;
            Vector3 t = target.position;
            Vector3 next = Vector3.MoveTowards(p, t, moveSpeed * Time.deltaTime);
            transform.position = next;

            // ��������� ���� �ɷ� ó��
            float dist = Vector3.Distance(transform.position, t);
            if (dist <= catchDistance)
            {
                GiveXpAndDisappear();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // PlayerMagnet �±׸� ���� Trigger�� ������ ���� ����
        if (other.CompareTag("PlayerMagnet"))
        {
            attracted = true;
            target = other.transform;

            // �ڼ��� �θ�(�÷��̾�) �ʿ��� XpSystem ã��
            if (xpSystem == null)
            {
                xpSystem = other.GetComponentInParent<XpSystem>();
                if (xpSystem == null)
                {
                    // Ȥ�� �� ã���� ������ �� �� �˻�(����õ������ ������ġ)
                    xpSystem = FindAnyObjectByType<XpSystem>();
                }
            }
        }
    }

    void GiveXpAndDisappear()
    {
        CancelInvoke();

        if (xpSystem != null)
        {
            xpSystem.AddExp(xpValue);
        }

        // �������(Ǯ ������ Ǯ�� �ݳ� / ������ Destroy)
        DestroyOrb();
    }

    public void SetKeyPrefab(GameObject key)
    {
        keyPrefab = key;
    }

    void DestroyOrb()
    {
        if (PoolManager.Instance != null)
        {
            PoolManager.Instance.Release(keyPrefab, gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
