using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
//�����̾�������Ʈ�� ���� �ش� Ŭ������ �� ���� �� ������Ʈ�� �ʿ��ϴ� ������Ʈ�� �ش� ��ũ��Ʈ�� �ٿ������� �ڵ����� ������Ʈ�� �߰��ȴ�.

public class EnemyCore : MonoBehaviour
{
    public float moveSpeed = 2.0f;
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
        if(player != null)
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

        Vector2 dir = (target.position - transform.position).normalized; // ������ ����ȭ : ������ ũ�⸦ 1�� �������. ������ ũ��(��)�� �����ϰ� ���������� �ʿ��� �� ���
        rb.velocity = dir * moveSpeed;
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }
}
