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
        // 플레이어 찾기 (태그로)
        if (target == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
        }

        if (target == null) return;

        // 일정 반경 안에 플레이어가 있는지 체크
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectRadius, LayerMask.GetMask("Player")); // 순서대로 옵젝의 위치, 옵젝 기준 반경, 플레이어의 레이어 감지
        if (hit != null)
        {
            // 플레이어 방향으로 이동
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
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
    }

    private void OnDisable()
    {
        //health.OnDied -= HandleDied;
    }
    public void SetTarget(Transform t)
    {
        target = t;
    }

}
