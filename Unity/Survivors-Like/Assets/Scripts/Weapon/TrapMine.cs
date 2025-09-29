using System.Collections;
using UnityEngine;

public class MineTrap : MonoBehaviour
{
    [Header("Settings")]
    public float delayBeforeExplosion = 1.5f;   // ��� �� �� ���߱��� ������
    public float explosionRadius = 3f;          // ������ �ݰ�
    public int damage = 20;                     // ���� ������
    public LayerMask targetLayer;               // ��/�÷��̾� �� ���� ���

    [Header("Effects")]
    public GameObject explosionEffect;          // ���� VFX ������
    //public CameraShake cameraShake;             // ī�޶� ����ũ ��ũ��Ʈ ����

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        // �÷��̾ ���� ����� �� (���̾� üũ)
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            triggered = true;
            StartCoroutine(ExplodeAfterDelay());
        }
    }

    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeExplosion);

        // ���� ����Ʈ ����
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // ������ ����
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayer);
        foreach (Collider2D hit in hits)
        {
            // ���⼭�� ü�� ������Ʈ�� ����
            Health hp = hit.GetComponent<Health>();
            if (hp != null)
            {
                hp.TakeDamage(damage,transform.position);
            }
        }

        //// ī�޶� ��鸲
        //if (cameraShake != null)
        //{
        //    StartCoroutine(cameraShake.Shake(0.2f, 0.3f));
        //}

        Destroy(gameObject); // ���� ������Ʈ ����
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}