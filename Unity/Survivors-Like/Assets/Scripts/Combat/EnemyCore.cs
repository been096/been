using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
//리콰이어컴포넌트를 쓰면 해당 클래스를 쓸 때에 이 컴포넌트가 필요하다 오브젝트에 해당 스크립트를 붙여넣으면 자동으로 컴포넌트가 추가된다.

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

        Vector2 dir = (target.position - transform.position).normalized; // 벡터의 정규화 : 벡터의 크기를 1로 만들어줌. 벡터의 크기(힘)을 제거하고 방향정보만 필요할 때 사용
        rb.velocity = dir * moveSpeed;
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }
}
