using UnityEngine;

/// <summary>
/// ��� �ǰ� ���� ��ü�� ������ ���� �������̽�.
/// ����� �� �������̽��� ���� �������� ����.
/// </summary>
public interface IDamagealbe
{
    /// <summary>
    /// �������� ����.
    /// </summary>
    /// <param name="amount">������ ������</param>
    /// <param name="hitPoint">���� ���� ��ǥ</param>
    /// <param name="hitNormal">���� ǥ�� ����</param>
    /// <param name="source">������ Ʈ������</param>
    void ApplyDamage(float amount, Vector3 hitPoint, Vector3 hitNormal, Transform source);
}
