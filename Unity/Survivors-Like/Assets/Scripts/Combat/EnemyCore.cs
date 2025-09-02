using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class EnemyCore : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float externalSpeedMultiplier = 1f;
    public bool isBoss = false;

    private Transform target;

    private Rigidbody2D rb;
    private Health health;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        PlayerCore player = FindObjectOfType<PlayerCore>();
        if (player != null)
        {
            SetTarget(player.transform);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (health.IsAlive == false || target == null)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isBoss == false)
        {
            return;
        }

        Vector2 dir = (target.position - transform.position).normalized;    // ������ ����ȭ : ������ ũ�⸦ 1�� �������. ���������� �ʿ��� �� ���.
        //rb.velocity = dir * moveSpeed;
        float speed = moveSpeed * externalSpeedMultiplier;
        rb.velocity = dir.normalized * speed;
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    public void AddSpeedMultiplierPercent(float percent)
    {
        float m = 1f + (percent / 100f);
        if (m < 0.1f)
        {
            m = 0.1f;
        }
        externalSpeedMultiplier = externalSpeedMultiplier * m;
    }
}