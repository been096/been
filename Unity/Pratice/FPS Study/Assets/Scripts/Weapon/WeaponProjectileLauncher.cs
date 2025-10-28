using UnityEngine;

/// <summary>
/// �߻� ���� ���� : ī�޶�/�ѱ� �������� ProjectileBullet�� ����/�߻�.
/// - ADS/�������尡 ������ �ܺο��� '�߻� ����'�� ����� �Ѱܵ� �ǰ�,
///   ���� �������� �� ��ũ��Ʈ ���ο��� ������ ���ø��� �� �� ����.
/// </summary>
public class WeaponProjectileLauncher : MonoBehaviour
{
    [Header("Spawn")]
    public Transform muzzle;                    // �߻� ����(������ ī�޶� ��ġ ���)
    public Camera playerCamera;                 // ���� ����.
    public ProjectileBullet projectilePrefab;   // ź ������.
    public LayerMask fireMask;                  // ��� ���� ���̾� ����ũ

    [Header("Ballistics")]
    public float projectileSpeed = 120.0f;
    public bool projectileUseGravity = false;

    [Header("Spread (Optional)")]
    public float spreadDeg = 0.0f;              // ���� ����.
    public bool useConeCosineBias = true;

    public void FireOne()
    {
        // �ʼ� ���� ���
        if (projectilePrefab == null)
        {
            return;
        }
        if (playerCamera == null)
        {
            return;
        }

        // 1) �߻� ����/���� ����.
        Vector3 origin = muzzle != null ? muzzle.position : playerCamera.transform.position;
        Vector3 forward = playerCamera.transform.forward;

        // 2) ������ �ִٸ� ���� �� ���� ����.
        Vector3 shotDir = forward;
        if (spreadDeg > 0.0001f)
        {
            shotDir = SampleDirectionInCone(forward, spreadDeg, useConeCosineBias);
        }

        // 3) źȯ ����/�ʱ� �ӵ� ����.
        ProjectileBullet p = Instantiate(projectilePrefab, origin, Quaternion.LookRotation(shotDir));
        p.useGravity = projectileUseGravity;
        p.SetInitialVelocity(shotDir * projectileSpeed);
    }

    // ���� �� ���� ���ø�(9������ ���� ����)
    private Vector3 SampleDirectionInCone(Vector3 forward, float coneAngleDeg, bool cosineBias)
    {
        float halfRad = coneAngleDeg * 0.5f * Mathf.Deg2Rad;
        float tan = Mathf.Tan(halfRad);

        // ����.
        float u = Random.value;
        float v = Random.value;

        float r = u;
        if (cosineBias == true)
        {
            // �߽� �е��� ���̱� ���� ���� ����.
            r = Mathf.Pow(u, 0.35f);
        }
        float theta = 2.0f * Mathf.PI * v;

        float x = Mathf.Cos(theta) * r * tan;
        float y = Mathf.Sin(theta) * r * tan;

        // Vector3.Cross = ������ ����. �� ���Ϳ� ������ ���ο� ���͸� ���ϴ� �Լ�.
        Vector3 right = Vector3.Cross(forward, Vector3.up);
        if (right.sqrMagnitude < 0.0000001f)
        {
            right = Vector3.Cross(forward, Vector3.forward);
        }
        right.Normalize();
        Vector3 up = Vector3.Cross(right, forward).normalized;

        Vector3 dir = (forward + right * x + up * y).normalized;
        return dir;

    }
}
