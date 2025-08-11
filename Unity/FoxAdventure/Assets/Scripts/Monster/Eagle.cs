using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    public float speed;
    public bool Ismove;

    public GameObject objTarget;
    public PlayerHealth heartManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move(objTarget);
    }

    bool Move(GameObject target)
    {
        if (target == null) return false; // 이 조건문이 가장 위에 있어야 타겟이 없을때의 불필요한 절차를 수행하지 않는다.

        Vector3 vTargetpos = target.transform.position; // 타겟(플레이어)의 위치
        Vector3 vPos = this.transform.position; // 독수리의 위치

        Vector3 vDist = vTargetpos - vPos; // 타겟위치에서 독수리위치를 뺌. -> 독수리 방향에서 타겟까지의 방향과 거리.
        Vector3 vDir = vDist.normalized; // 독수리에서 타겟까지의 방향

        float fDist = vDist.magnitude; // 독수리에서 타겟까지의 순수 이동량(순수거리). 벡터의 순수 힘.
        float fMove = speed * Time.deltaTime; // 스피드 * 시간 -> 사실상 속도 -> 텔타타임을 쓴 시점에서 (틱당 or 프레임 당의) 최소 이동거리

        if (fDist > fMove) //(독수리에서 타겟까지의 벡터 상에서의 힘) > 최소 이동거리보다 멀 때 -> 상대방이 나와 멀리 있냐 없냐를 구분하는 조건문
        {
            transform.position += vDir * speed * Time.deltaTime; // 위치가 증가한다 -> 얼마나? -> 독수리에서 타겟까지의 방향으로, 속도만큼. 지금 이 수식은 수업용. fmove로 생략하는것이 좋다.
            Debug.DrawLine(vTargetpos, vPos, Color.green);
            return true;
        }
        else
        {
            Debug.DrawLine(vTargetpos, vPos, Color.red);
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            objTarget = collision.gameObject;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{gameObject.name}.OnCollisionEnter2D{collision.gameObject}/{collision.gameObject.tag})");
        if (collision.gameObject.tag == "Player")
            //Destroy(collision.gameObject);
            heartManager.TakeDamage();
    }
}
