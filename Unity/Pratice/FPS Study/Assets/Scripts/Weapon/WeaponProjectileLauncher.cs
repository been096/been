using UnityEngine;

/// <summary>
/// 발사 무기 런쳐 : 카메라/총구 방향으로 ProjectileBullet을 생성/발사.
/// - ADS/스프레드가 있으면 외부에서 '발사 방향'을 계산해 넘겨도 되고,
///   간단 버전으로 이 스크립트 내부에서 퍼짐을 샘플링할 수 도 있음.
/// </summary>
public class WeaponProjectileLauncher : MonoBehaviour
{
    [Header("Spawn")]
    public Transform muzzle;                    // 발사 원점(없으면 카메라 위치 사용)
    public Camera playerCamera;                 // 조준 기준.
    public ProjectileBullet projectilePrefab;   // 탄 프리팹.
    public LayerMask fireMask;                  // 사격 기준 레이어 마스크

    [Header("Ballistics")]
    public float projectileSpeed = 120.0f;
    public bool projectileUseGravity = false;

    [Header("Spread (Optional)")]
    public float spreadDeg = 0.0f;              // 간단 퍼짐.
    public bool useConeCosineBias = true;

    public void FireOne()
    {
        // 필수 참조 방어
        if (projectilePrefab == null)
        {
            return;
        }
        if (playerCamera == null)
        {
            return;
        }

        // 1) 발사 원점/기준 방향.
        Vector3 origin = muzzle != null ? muzzle.position : playerCamera.transform.position;
        Vector3 forward = playerCamera.transform.forward;

        // 2) 퍼짐이 있다면 원뿔 내 방향 샘플.
        Vector3 shotDir = forward;
        if (spreadDeg > 0.0001f)
        {
            shotDir = SampleDirectionInCone(forward, spreadDeg, useConeCosineBias);
        }

        // 3) 탄환 생성/초기 속도 셋팅.
        ProjectileBullet p = Instantiate(projectilePrefab, origin, Quaternion.LookRotation(shotDir));
        p.useGravity = projectileUseGravity;
        p.SetInitialVelocity(shotDir * projectileSpeed);
    }

    // 원뿔 내 방향 샘플링(9일차와 동일 원리)
    private Vector3 SampleDirectionInCone(Vector3 forward, float coneAngleDeg, bool cosineBias)
    {
        float halfRad = coneAngleDeg * 0.5f * Mathf.Deg2Rad;
        float tan = Mathf.Tan(halfRad);

        // 난수.
        float u = Random.value;
        float v = Random.value;

        float r = u;
        if (cosineBias == true)
        {
            // 중심 밀도를 높이기 위한 지수 보정.
            r = Mathf.Pow(u, 0.35f);
        }
        float theta = 2.0f * Mathf.PI * v;

        float x = Mathf.Cos(theta) * r * tan;
        float y = Mathf.Sin(theta) * r * tan;

        // Vector3.Cross = 벡터의 외적. 두 벡터에 수직인 새로운 벡터를 구하는 함수.
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
