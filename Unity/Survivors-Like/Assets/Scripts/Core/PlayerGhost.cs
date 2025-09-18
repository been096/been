using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGhost : MonoBehaviour
{
    public PlayerDash dash;
    public SpriteRenderer sourceSprite;
    public float spawnIntervalSeconds = 0.05f;
    public float ghostLifetimeSeconds = 0.2f;
    //public GameObject sourceSprite;

    private float lasSpawnTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dash.GetIsDasing() == true)
        {
            if (Time.time - lasSpawnTime >= spawnIntervalSeconds)
            {
                SpawnGhost();
                lasSpawnTime = Time.time;
            }
        }
    }

    void SpawnGhost()       // 플레이어와 똑같은 오브젝트를 생성하는 함수
    {
        GameObject ghost = new GameObject("PlayerGhost");
        SpriteRenderer sr = ghost.AddComponent<SpriteRenderer>();
        sr.sprite = sourceSprite.sprite;
        sr.flipX = sourceSprite.flipX;
        sr.color = new Color(sourceSprite.color.r, sourceSprite.color.g, sourceSprite.color.b, sourceSprite.color.a);
        sr.sortingLayerID = sourceSprite.sortingLayerID;
        sr.sortingOrder = sourceSprite.sortingOrder - 1;

        ghost.transform.position = sourceSprite.transform.position;
        ghost.transform.rotation = sourceSprite.transform.rotation;
        ghost.transform.localScale = sourceSprite.transform.localScale;

        PlayerGhostDestroyer pgd = ghost.AddComponent<PlayerGhostDestroyer>();
        pgd.SetLifetime(ghostLifetimeSeconds);
    }

    //void SpawnTrail()
    //{
    //    TrailRenderer trail = this.AddComponent<TrailRenderer>();

    //}
}
