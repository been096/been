using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class RadialLightSprite : MonoBehaviour
{
    public int size = 256;
    public float edgeSoftness = 0.8f;    // 0 ~ 1 (가장자리 부드러움, 1이면 매우 부드러움)
    public Color innerColor = new Color(1f, 1f, 0.95f, 1f); // 중심색
    public float radiusWorld = 3.5f;    // 반경.

    public SpriteRenderer sr;

    Texture2D tex;

    private void Awake()
    {
        CreateTexture();
    }

    private void OnDestroy()
    {
        if (tex != null)
        {
            Destroy(tex);

        }
    }

    void CreateTexture()
    {
        if (size < 8)
        {
            size = 8;
        }

        tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Bilinear;

        float cx = (float)(size - 1) * 0.5f;
        float cy = (float)(size - 1) * 0.5f;
        int y = 0;
        while (y < size)
        {
            int x = 0;
            while (x < size)
            {
                float dx = (float)x - cx;
                float dy = (float)y - cy;
                float dist = Mathf.Sqrt(dx * dx + dy * dy);
                float maxDist = cx;

                float r = dist / maxDist;
                if (r > 1f)
                {
                    r = 1f;
                }
                if (r < 0f)
                {
                    r = 0f;
                }

                float alpha = 1f - Mathf.Pow(r, edgeSoftness * 2f);

                Color c = new Color(innerColor.r, innerColor.g, innerColor.b, alpha);
                tex.SetPixel(x, y, c);

                x = x + 1;
            }
            y = y + 1;
        }

        tex.Apply();

        Sprite sp = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
        sr.sprite = sp;

        float worldDiameter = radiusWorld * 2f;
        transform.localScale = new Vector3(worldDiameter, worldDiameter, 1f);

        sr.color = Color.white;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
