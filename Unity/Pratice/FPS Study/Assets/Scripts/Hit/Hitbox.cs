using UnityEngine;

/// <summary>
/// ������ ������ ����� �����ϴ� ��Ʈ�ڽ�.
/// ��) Head 2.0, Body 1.0, Limbs 0.8 ��
/// </summary>
public class Hitbox : MonoBehaviour
{
    public float damageMultiplier = 1.0f;       // �� ������ ���(Head 2.0 ��)
    public Health owner;                        // �� ��Ʈ�ڽ��� ���� ĳ������ Health

    private void Reset()
    {
        Health h = GetComponent<Health>();
        if (h != null)
        {
            owner = h;
        }
    }
}
