using UnityEngine;

/// <summary>
/// �÷��̾� �߹����� Raycast�� �����Ͽ� ���� ��� ǥ���� Ÿ��/��ǥ/������ ����.
/// </summary>
[DisallowMultipleComponent]
public class FootstepSurfaceDetector : MonoBehaviour
{
    public Transform probeOrigin;       // ���� ������(���� CharacterController �߽� �ణ ��)
    public float probeDistance = 1.2f;  // �Ʒ��� �� �Ÿ�(�÷��̾� Ű/CC ���̿� ���� ����)
    public LayerMask groundMask;        // �ٴ� ���̾�.

    public bool TryGetSurface(out SurfaceType surface, out Vector3 point, out Vector3 normal)
    {
        surface = SurfaceType.Concrete;
        point = Vector3.zero;
        normal = Vector3.up;

        if (probeOrigin == null)
        {
            return false;
        }

        RaycastHit hit;
        bool got = Physics.Raycast(
            probeOrigin.position,           // ������.
            Vector3.down,                   // �Ʒ� ����.
            out hit,                        // ���.
            probeDistance,                  // �Ÿ�.
            groundMask,                     // �ٴ� ���̾�.
            QueryTriggerInteraction.Ignore  // Ʈ���� ����.
        );

        if (got == true)
        {
            point = hit.point;
            normal = hit.normal;

            SurfaceMaterial s = hit.collider.GetComponent<SurfaceMaterial>();
            if (s != null)
            {
                surface = s.surfaceType;
                return true;
            }
            else
            {
                // Ÿ���� ������ �⺻��(��ũ��Ʈ) ���.
                surface = SurfaceType.Concrete;
                return true;
            }
        }

        return false;
    }
    
}
