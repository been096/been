using System.Collections;
using UnityEngine;

public class MineTrap : MonoBehaviour
{
    [Header("Settings")]
    public float delayBeforeExplosion = 1.5f;   // 밟고 난 뒤 폭발까지 딜레이
    public float explosionRadius = 3f;          // 데미지 반경
    public int damage = 20;                     // 폭발 데미지
    public LayerMask targetLayer;               // 적/플레이어 등 피해 대상

    [Header("Effects")]
    public GameObject explosionEffect;          // 폭발 VFX 프리팹
    //public CameraShake cameraShake;             // 카메라 쉐이크 스크립트 참조

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        // 플레이어나 적이 밟았을 때 (레이어 체크)
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            triggered = true;
            StartCoroutine(ExplodeAfterDelay());
        }
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeExplosion);

        // 폭발 이펙트 생성
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // 데미지 적용
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayer);
        foreach (Collider2D hit in hits)
        {
            // 여기서는 체력 컴포넌트를 가정
            Health hp = hit.GetComponent<Health>();
            if (hp != null)
            {
                hp.TakeDamage(damage,transform.position);
            }
        }

        //// 카메라 흔들림
        //if (cameraShake != null)
        //{
        //    StartCoroutine(cameraShake.Shake(0.2f, 0.3f));
        //}

        Destroy(gameObject); // 지뢰 오브젝트 제거
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}