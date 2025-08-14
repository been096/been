using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackReceiver : MonoBehaviour
{
    public float damping = 12.0f; // 감쇠 정도. 이 값이 클 수록 빨리 멈춤.
    public float maxSpeed = 12.0f; // 최대 속도.

    private Rigidbody2D rb;
    private Vector2 externalVelocity; // 넉백 속도.

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    public void ApplyKnockback(Vector2 force)
    {
        externalVelocity = Vector2.ClampMagnitude(externalVelocity + force, maxSpeed); // externalVelocity + force 크기가 최소, maxSpeed가 최대를 적용시키겠다는 함수
    }

    private void FixedUpdate()
    {
        rb.velocity += externalVelocity;
        externalVelocity = Vector2.MoveTowards(externalVelocity, Vector2.zero, damping * Time.fixedDeltaTime); // fixedUpdate를 사용 중이기에 타임.델타타임이 아닌 타임.픽스드델타타임을 사용. Movetoward 함수는 앞으로 힘을 보내는 함수
    }
}
