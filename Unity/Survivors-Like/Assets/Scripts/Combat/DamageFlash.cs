using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    public Color flashColor = Color.white;
    public float flashDuration = 0.1f;

    private SpriteRenderer sr;
    private Color originalColor;
    private Coroutine routine;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void PlayFlash()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }

        routine = StartCoroutine(CoFlash());
    }

    IEnumerator CoFlash()
    {
        //sr.color = flashColor;
        //yield return new WaitForSeconds(flashDuration);
        //sr.color = originalColor;
        //routine = null;

        Color before = sr.color;            // �÷��� �� ���� ���
        sr.color = flashColor;              // ��½
        yield return new WaitForSeconds(flashDuration);
        sr.color = before;                  // �ٷ� ���� ������ ����(����Ʈ �� ������)
        routine = null;
    }

    // DamageFlash.cs �� �߰�
    public void SetBaseColor(Color c)
    {
        if (sr == null)
        {
            return;
        }
        sr.color = c;
        originalColor = c; // ���� �ڵ尡 originalColor�� �����ߴٸ�, �� ������ ����
    }

}
