using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Opossum : MonoBehaviour
{
    //------------------�ִϸ��̼� ����----------------------------
    public Sprite[] moveSprites;
    public Sprite[] dieSprites;

    private AnimState state = AnimState.Move;

    private int frame = -1;
    private float animtimer = 0.0f;
    public float frameRate = 0.15f;
    private bool isDead = false;

    //------------------------------------------------------------

    //----------------�̵� ����------------------------------------
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
            return; // ���� ���¸� �̵�/���� �� ����
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

        frame = (frame + 1) % curArr.Length; // ����. �ϴ��� �ܿ���. ���߿� �˾ƺ����� �����ð��� �����غ���.
        sr.sprite = curArr[frame]; // �����Ӹ��� �̹����� �ٲ��.
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
        if (col != null) col.enabled = false; // �浹 ����

        state = AnimState.Die;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        Invoke("DestroySelf", 0.5f); // ��: 0.5�� �� ����
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

}
