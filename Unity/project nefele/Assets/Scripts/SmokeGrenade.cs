using UnityEngine;

public class SmokeGrenade : MonoBehaviour
{
    [Header("Smoke Settings")]
    [SerializeField] private float duration = 3.0f; // 연막탄이 유지되는 시간

    private void Start()
    {
        // 생성되자마자 duration 초 뒤에 스스로 파괴되도록 예약합니다.
        Destroy(gameObject, duration);
    }
}