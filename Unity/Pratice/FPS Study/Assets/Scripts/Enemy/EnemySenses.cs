using UnityEngine;

/// <summary>
/// ���� '�þ�'�� ����ϴ� ���.
/// - �Ÿ� : viewDistance �̸�.
/// - �þ߰� : viewAngle/2 �ݰ� �̳�(��Ʈ ���δ�Ʈ�� �Ǵ�)
/// - ���� : eye ��ġ���� Ÿ�� �������� ����ĳ��Ʈ -> ù ��Ʈ�� Ÿ�� ��Ʈ��� '����'
/// </summary>
public class EnemySenses : MonoBehaviour
{
    [Header("Vision")]
    public Transform eye;                   // �þ��� ������(�Ӹ� / �� ��ġ)
    public float viewDistance = 18.0f;      // �þ� �Ÿ�(����)
    public float viewAngle = 110.0f;        // ��ü �þ߰�. �ݰ� = viewAngle * 0.5
    public LayerMask visionMask;            // ����ĳ��Ʈ �浹 ����ũ(��/�ٴ�/�÷��̾� ����)
    public Transform target;                // ������ ���(�÷��̾� ��Ʈ Transform)

    [Header("Debug")]
    public bool drawDebug = false;          // ����� �� �׸��� ���

    /// <summary>
    /// Ÿ���� ���� '���̴���'�� ���.
    /// ���̸� true�� ��ȯ�ϰ�, lastKnownPos�� ���� Ÿ�� ��ġ�� �����ݴϴ�.
    /// </summary>
    /// <param name="last"></param>
    /// <returns></returns>
    public bool CanSeeTarget(out Vector3 lastKnownPos)
    {
        // ��ȯ�� �ʱ�ȭ.
        lastKnownPos = Vector3.zero;

        // �ʼ� ���� ���.
        if (eye == null)
        {
            return false;
        }
        if (target == null)
        {
            return false;
        }

        // 1) �Ÿ� ����.
        Vector3 toTarget = target.position - eye.position;  // Ÿ�ٱ����� ����(����)
        float dist = toTarget.magnitude;                    // �Ÿ�(����)
        if (dist > viewDistance)
        {
            return false;
        }

        // 2) �þ߰� ���� (��Ʈ ���δ�Ʈ = cos(��))
        Vector3 forward = eye.forward;                      // ���� ����.
        Vector3 dir = toTarget.normalized;                  // Ÿ�� ���� ���� ����.
        float dot = Vector3.Dot(forward, dir);              // cos(theta). Dot -> ������ ������ ���ϴ� �Լ�. �� ���� ������ ������ ���Ѵ�.
        float halfRad = (viewAngle * 0.5f) * Mathf.Deg2Rad; // �ݰ� ����.
        float cosHalf = Mathf.Cos(halfRad);                 // �Ӱ�ġ : cos(�ݰ�)

        // dot < cos(�ݰ�)�̸� �þ� ��
        if (dot < cosHalf)
        {
            return false;
        }

        // 3) ����(occlusion) üũ : Raycast�� ù �浹ü Ȯ��.
        Ray ray = new Ray(eye.position, dir);
        RaycastHit hit;
        bool got = Physics.Raycast(ray, out hit, viewDistance, visionMask, QueryTriggerInteraction.Ignore);

        if (got == true)
        {
            // ù ��Ʈ�� ��Ʈ�� Ÿ�� ��Ʈ�� �����ϸ� '�������� ����'
            Transform h = hit.collider.transform;
            if (IsSameRoot(h, target) == true)
            {
                lastKnownPos = target.position;
                
                if (drawDebug == true)
                {
                    Debug.DrawLine(eye.position, hit.point, Color.green, 0.1f);
                }
                return true;
            }
            else
            {
                if (drawDebug == true)
                {
                    Debug.DrawLine(eye.position, hit.point, Color.red, 0.1f);
                }
                return false;
            }
        }

        // ���̰� �ƹ��͵� ���� ������ '������ ����' ó��.
        return false;
    }

    /// <summary>
    /// �� Transform�� ������ �ֻ��� ��Ʈ���� �����մϴ�.
    /// (ĳ���� ���� �ݶ��̴� �������� '���� ����ΰ�' �Ǵܿ�)
    /// </summary>
    private bool IsSameRoot(Transform a, Transform b)
    {
        if (a == null)
        {
            return false;
        }
        if (b == null)
        {
            return false;
        }

        Transform ra = a.root;
        Transform rb = b.root;
        if (ra == rb)
        {
            return true;
        }
        return false;
    }
}
