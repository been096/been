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
        if (target == null) return false; // �� ���ǹ��� ���� ���� �־�� Ÿ���� �������� ���ʿ��� ������ �������� �ʴ´�.

        Vector3 vTargetpos = target.transform.position; // Ÿ��(�÷��̾�)�� ��ġ
        Vector3 vPos = this.transform.position; // �������� ��ġ

        Vector3 vDist = vTargetpos - vPos; // Ÿ����ġ���� ��������ġ�� ��. -> ������ ���⿡�� Ÿ�ٱ����� ����� �Ÿ�.
        Vector3 vDir = vDist.normalized; // ���������� Ÿ�ٱ����� ����

        float fDist = vDist.magnitude; // ���������� Ÿ�ٱ����� ���� �̵���(�����Ÿ�). ������ ���� ��.
        float fMove = speed * Time.deltaTime; // ���ǵ� * �ð� -> ��ǻ� �ӵ� -> ��ŸŸ���� �� �������� (ƽ�� or ������ ����) �ּ� �̵��Ÿ�

        if (fDist > fMove) //(���������� Ÿ�ٱ����� ���� �󿡼��� ��) > �ּ� �̵��Ÿ����� �� �� -> ������ ���� �ָ� �ֳ� ���ĸ� �����ϴ� ���ǹ�
        {
            transform.position += vDir * speed * Time.deltaTime; // ��ġ�� �����Ѵ� -> �󸶳�? -> ���������� Ÿ�ٱ����� ��������, �ӵ���ŭ. ���� �� ������ ������. fmove�� �����ϴ°��� ����.
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
