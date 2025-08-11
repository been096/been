using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public enum AnimState
{
    Idle,
    Move,
    Jump,
    Die
}

// ���� ���°� �ƴҶ� -> ��ü���ٰ� ���� ���ؼ� �����Ѵ�. ���� ���°� Ʈ�簡 �ȴ�.
// 
public class Dynamic : MonoBehaviour
{
    //------------ �ִϸ��̼� ���� ������----------------
    public Sprite[] idleSprites;
    public Sprite[] moveSprites;
    public Sprite[] jumpSprites;
    public Sprite[] dieSprites;

    private AnimState state = AnimState.Idle;

    private SpriteRenderer sr;

    private int frame = -1;
    private float timer = 0.0f;
    public float frameRate = 0.15f;
    //-------------------------------------------------


    public AudioSource AudioSource;
    public AudioClip jumpClip;
    public float jumpPower = 7.0f;
    //public float knockbackPower = 3.0f;
    public float speed = 5.0f;
    public Rigidbody2D rb;
    public bool isGrounded;
    public bool CanDoubleJump;

    private float moveX;
    public PlayerHealth heartManager; // 

    public GameObject groundCheck; // �� �� ������ (empty ������Ʈ)
    public LayerMask groundLayer; // Ư�� ���̾�� �浹.
    public LayerMask monsterLayer;
    public float checkDistance = 0.1f;

    public GameObject GameClear;


    public Gun gun;

    public Vector3 vDir;

    //��ŸƮ���� ���� ȣ��Ǵ� �Լ�
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, checkDistance, groundLayer);
        isGrounded = IsGrounded();
        Attack();

        if (state == AnimState.Jump)
        {
            Debug.Log(state);
        }

        //if(isGrounded = true)
        //{
        //    state = AnimState.Idle;
        //}

        if (Input.GetKey(KeyCode.RightArrow))
        {
            state = AnimState.Move;
            transform.localScale = new Vector3(1, 1, 1);
            transform.position += Vector3.right * speed * Time.deltaTime;
            vDir = Vector3.right;
            //Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
            //velocity.x = 0;
            //GetComponent<Rigidbody2D>().velocity = velocity;

            //if (Isjump = false)
            //{
            //    Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
            //    velocity.x = 0;
            //    GetComponent<Rigidbody2D>().velocity = velocity;
            //}
            if (isGrounded == false)
            {
                state = AnimState.Jump;
            }

        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            state = AnimState.Move;
            transform.localScale = new Vector3(-1, 1, 1);
            transform.position += Vector3.left * speed * Time.deltaTime;
            vDir = Vector3.right;
            //GetComponent<Rigidbody2D>().velocity = (Vector2.zero);

            if (isGrounded == false)
            {
                state = AnimState.Jump;
            }
        }

        if ((Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) && state == AnimState.Move)
        {
            state = AnimState.Idle;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioSource.PlayOneShot(jumpClip);
            //if (isGrounded == true)
            if (isGrounded)
            {
                state = AnimState.Jump;
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                //isGrounded = false;
                CanDoubleJump = true;
            }

            //else if (isGrounded == false && CanDoubleJump == true)
            else if (CanDoubleJump)
            {
                state = AnimState.Jump;
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                CanDoubleJump = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
            gun.Shot();

        timer += Time.deltaTime;
        if(timer >= frameRate)
        {
            timer = 0.0f;
            PlayAnimation();
        }

        //Debug.Log($"isGrounded: {isGrounded}, speed: {speed}, velocity: {rb.velocity}, position: {transform.position}");

    }

   //�÷��̾� ���� �ִϸ��̼��� ���� �Լ�
    void PlayAnimation()
    {
        Sprite[] curArr = idleSprites;

        switch(state)
        {
            case AnimState.Move:
                {
                    curArr = moveSprites;
                }
                break;
            case AnimState.Jump:
                {
                    curArr = jumpSprites;
                }
                break;
            case AnimState.Die:
                {
                    curArr = dieSprites;
                }
                break;
        }

        frame = (frame + 1) % curArr.Length; // ����. �ϴ��� �ܿ���. ���߿� �˾ƺ����� �����ð��� �����غ���.
        sr.sprite = curArr[frame]; // �����Ӹ��� �̹����� �ٲ��.
    }

    private void OnDestroy()
    {
        //���� ��ġ���� ��Ȱ��
        //GameObject objPlayer = Instantiate(this.gameObject);
        //objPlayer.SetActive(true);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            //isGrounded = true;
            CanDoubleJump = false;
            Debug.Log(isGrounded);
            state = AnimState.Idle;
        }

        if(collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "Monster")
        {
            heartManager.TakeDamage();
            //state = AnimState.Die;
        }

        if (collision.gameObject.layer == monsterLayer )
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = false;
    }

    //private void FlipSprite(float direction)
    //{
    //    if (direction > 0)
    //    {
    //        transform.localScale = new Vector3(1, 1, 1);
    //    }
    //    else if (direction < 0)
    //    {
    //        transform.localScale = new Vector3(-1, 1, 1);
    //    }
    //}


    private bool IsGrounded()
    {
        float rayLength = 0.25f;
        Vector2 rayOrigin = new Vector2(groundCheck.transform.position.x, groundCheck.transform.position.y - 0.1f);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, groundLayer);
        return hit.collider != null;
        Debug.DrawRay(rayOrigin, Vector2.down * rayLength, Color.red);
    }

    private void Attack()
    {
        float monsterrayLength = 0.25f;
        Vector2 rayOrigin = new Vector2(groundCheck.transform.position.x, groundCheck.transform.position.y - 0.1f);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, monsterrayLength, monsterLayer);
        if(hit.collider != null)
        {
            if (hit.collider.tag == "Monster")
            {
                //Destroy(hit.collider.gameObject);
                Opossum monster = hit.collider.GetComponent<Opossum>();
                if(monster != null)
                {
                    monster.Die();
                }

                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
        }
        Debug.DrawRay(rayOrigin, Vector2.down * monsterrayLength, Color.red);
    }
    
    
}