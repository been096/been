using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Opossum : MonoBehaviour
{
    //------------------애니메이션 변수----------------------------
    public Sprite[] moveSprites;
    public Sprite[] dieSprites;

    private AnimState state = AnimState.Move;

    private int frame = -1;
    private float animtimer = 0.0f;
    public float frameRate = 0.15f;
    private bool isDead = false;

    //------------------------------------------------------------

    //----------------이동 변수------------------------------------
    public float speed = 1.0f;
    public float timer = 0.0f;
    public float switchtime = 2.0f;
    private float dir = 1f;
    //------------------------------------------------------------
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    public PlayerHealth heartManager;

    // Start is called before the first frame update

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            animtimer += Time.deltaTime;
            if (animtimer >= frameRate)
            {
                animtimer = 0.0f;
                PlayAnimation();
            }
            return; // 죽은 상태면 이동/로직 다 멈춤
        }

        state = AnimState.Move;
        timer += Time.deltaTime;
        animtimer += Time.deltaTime;

        transform.Translate(Vector2.right * dir * speed * Time.deltaTime);

        if(timer >= switchtime)
        {
            //transform.localScale = new Vector3(-1, 1, 1);
            dir = dir *  -1f;
            sr.flipX = dir > 0;
            timer = 0.0f;
        }

        if (animtimer >= frameRate)
        {
            animtimer = 0.0f;
            PlayAnimation();
        }

        
        
    }

    void PlayAnimation()
    {
        Sprite[] curArr = moveSprites;

        switch (state)
        {
            case AnimState.Die:
                {
                    curArr = dieSprites;
                }
                break;
        }

        frame = (frame + 1) % curArr.Length; // 공식. 일단은 외우자. 나중에 알아보던가 질문시간에 질문해보자.
        sr.sprite = curArr[frame]; // 프레임마다 이미지가 바뀐다.
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"OnCollisionEnter2D:{collision.gameObject.name}");
        if (collision.gameObject.tag == "Player")
        {

            //heartManager.TakeDamage();
            //Destroy(collision.gameObject);
            //Destroy(this.gameObject);
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;
        frame = -1;
        animtimer = 0f;

        if(rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false; // 충돌 제거

        state = AnimState.Die;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        Invoke("DestroySelf", 0.5f); // 예: 0.5초 후 삭제
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

}
