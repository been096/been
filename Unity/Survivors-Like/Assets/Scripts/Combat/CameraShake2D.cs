using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraShake2D : MonoBehaviour
{
    private Vector3 baseLocalPos;
    private Coroutine routine;

    private void Awake()
    {
        baseLocalPos = transform.localPosition;
    }

    public void Shake(float amplitude = 0.1f, float duration = 0.1f)
    {
        if(routine != null)
        {
            StopCoroutine(routine);
        }

        routine = StartCoroutine(CoShake(amplitude, duration));
    }

    IEnumerator CoShake(float amp, float dur)
    {
        float t = 0.0f;
        while(t < dur)
        {
            t += Time.deltaTime;
            //������ ������ ȿ�� (�������� �� ������).
            Vector2 rnd = Random.insideUnitCircle * amp;
            transform.localPosition = baseLocalPos + (Vector3)rnd;
            yield return null;
        }

        transform.localPosition = baseLocalPos;
        routine = null;
    }

}
