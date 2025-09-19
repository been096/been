using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class Lightning : MonoBehaviour
{
    [Header("Visual")]
    public float skyHeight = 8.0f;
    public int segments = 10;   // 번개의 모양은 지그재그, 그 지그재그의 각 꼭짓점의 갯수
    public float laterallJitter = 0.8f;      // 번개의 각 지그재그의 길이
    public float life = 0.2f;       //번개가 남아있는 시간
    public Color colorStart = Color.white;
    public Color colorEnd = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    public float startWidth = 0.2f;     // 시작 부분의 너비
    public float endWidth = 0.24f;      // 끝 부분의 너비

    [Header("Damage")]
    public float impactradius = 2.0f;
    public int damage = 30;
    public LayerMask targetMask;

    public LineRenderer lr;
    float timer = 0.0f;
    bool active = false;

    private void Awake()
    {
        lr.positionCount = 0;
        lr.widthMultiplier = 1.0f;
        AnimationCurve wc = new AnimationCurve();
        wc.AddKey(0.0f, startWidth);    // 시작 너비 지정
        wc.AddKey(1.0f, endWidth);  // 끝 너비 지정
        lr.widthCurve = wc;

        //----------------------------------------------번개 그라데이션 효과----------------------------------------------------
        Gradient g = new Gradient();
        g.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(colorStart.a, 0.0f), new GradientAlphaKey(colorEnd.a, 1.0f) }
                );
        lr.colorGradient = g;
        lr.enabled = false;
        //--------------------------------------------------------------------------------------------------------------------

    }

    public void StirkeAt(Vector2 target)
    {
        Vector3 start = new Vector3(target.x, target.y + skyHeight, 0.0f);
        Vector3 end = new Vector3(target.x, target.y, 0.0f);

        if (segments < 2)
        {
            segments = 2;
        }

        lr.positionCount = segments;

        for (int i = 0; i < segments; ++i)
        {
            float t = 0.0f;
            if (segments > 1)
            {
                t = (float)i / (float)(segments - 1);
            }

            Vector3 p = Vector3.Lerp(start, end, t);

            float attenuation = 1.0f - t;
            float jitter = Random.Range(-laterallJitter, laterallJitter * attenuation);
            p.x = p.x + jitter;

            lr.SetPosition(i, p);
        }

        lr.enabled = true;
        timer = life;
        active = true;

        ApplyDamage(end);
    }

    void ApplyDamage(Vector2 center)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, impactradius, targetMask);

        for (int i = 0; i < hits.Length; ++i)
        {
            Collider2D c = hits[i];
            Health hp = c.GetComponent<Health>();
            if (hp != null)
            {
                hp.TakeDamage(damage, c.transform.position);
            }
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active == true)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                lr.enabled = false;
                active = false;
            }
        }
    }
}
