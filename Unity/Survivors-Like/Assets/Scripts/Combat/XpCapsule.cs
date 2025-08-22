using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class XpCapsule : MonoBehaviour
{
    public float detectRadius = 3f;
    public float moveSpeed = 2.0f;
    private Transform target;

    public int rewardxp = 5;
    public XpSystem playerxp;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerxp = FindAnyObjectByType<XpSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �÷��̾� ã�� (�±׷�)
        if (target == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
        }

        if (target == null) return;

        // ���� �ݰ� �ȿ� �÷��̾ �ִ��� üũ
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectRadius, LayerMask.GetMask("Player"));
        if (hit != null)
        {
            // �÷��̾� �������� �̵�
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            //Vector2 dir = (target.position - transform.position).normalized;
            //rb.velocity = dir * moveSpeed;
        }
    }

    private void FixedUpdate()
    {
        //Vector2 dir = (target.position - transform.position).normalized; // ������ ����ȭ : ������ ũ�⸦ 1�� �������. ������ ũ��(��)�� �����ϰ� ���������� �ʿ��� �� ���
        //rb.velocity = dir * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        { 

            playerxp?.AddExp(rewardxp);
            Destroy(gameObject);
        }

    }

    private void OnEnable()
    {
        PlayerCore player = FindObjectOfType<PlayerCore>();
        if (player != null)
        {
            SetTarget(player.transform);
        }

        //health.OnDied += HandleDied;
        //playerxp = FindAnyObjectByType<XpSystem>();
    }

    private void OnDisable()
    {
        //health.OnDied -= HandleDied;
    }
    public void SetTarget(Transform t)
    {
        target = t;
    }

    //private void HandleDied()
    //{
    //    playerxp?.AddExp(rewardxp);
    //    //GameObject xpOrb = Instantiate(xpPrefab, transform.position, Quaternion.identity);
    //}


}
