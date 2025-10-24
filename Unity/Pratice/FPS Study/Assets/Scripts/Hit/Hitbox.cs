using UnityEngine;

/// <summary>
/// 부위별 데미지 배수를 제공하는 히트박스.
/// 예) Head 2.0, Body 1.0, Limbs 0.8 등
/// </summary>
public class Hitbox : MonoBehaviour
{
    public float damageMultiplier = 1.0f;       // 이 부위의 배수(Head 2.0 등)
    public Health owner;                        // 이 히트박스가 속한 캐릭터의 Health

    private void Reset()
    {
        Health h = GetComponent<Health>();
        if (h != null)
        {
            owner = h;
        }
    }
}
