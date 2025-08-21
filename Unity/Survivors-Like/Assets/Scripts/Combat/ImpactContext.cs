using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactContext//   : MonoBehaviour
{
    public Transform target; // �´� ����� Ʈ������
    public Vector3 hitPoint;// �´� ��ġ
    public int damage; //���ط�
    public float magnitude; // ������ ����. 1 = �⺻, 1.5 = ũ��Ƽ��, 2 = �������� ���� Ÿ�� ���. �� ���������� ������ ���⸦ ���� ����
    public Transform instigator; // �������� Ʈ������.
   

    public Vector2 KnockbackDir2D()
    {
        var dir = (target.position - hitPoint);
        return dir.sqrMagnitude > 0.0001f ? (Vector2)dir.normalized : Vector2.zero;

    }
}
