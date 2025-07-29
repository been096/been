using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 점프 상태가 아닐때 -> 강체에다가 힘을 가해서 점프한다. 점프 상태가 트루가 된다.
// 
public class Dynamic : MonoBehaviour
{
    public int score;
    //public float speed = 1; // 한번에 얼마나 이동할지 지정
    //public float jumpPower = 100;
    //public bool Isjump;

    public float jumpPower = 7.0f;
    public float speed = 5.0f;
    public Rigidbody2D rb;
    bool isGrounded;
    bool CanDoubleJump;
    public PlayerHealth heartManager; // 

    public Transform groundCheck; // 발 밑 기준점 (empty 오브젝트)
    public LayerMask groundLayer; // 특정 레이어에만 충돌.
    public float checkDistance = 0.1f;

    public Gun gun;

    public Vector3 vDir;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, checkDistance, groundLayer);

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


        }
           
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            vDir = Vector3.right;
            //GetComponent<Rigidbody2D>().velocity = (Vector2.zero);
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded == true)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                isGrounded = false;
                CanDoubleJump = true;
            }

            else if (isGrounded == false && CanDoubleJump == true)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                CanDoubleJump = false;
            }

            if (Input.GetKeyDown(KeyCode.X))
                gun.Shot();
        }
    }

    private void OnDestroy()
    {
        //죽은 위치에서 부활함
        //GameObject objPlayer = Instantiate(this.gameObject);
        //objPlayer.SetActive(true);
    }

    //private void OnGUI()
    //{
    //    GUI.Box(new Rect(0, 0, 200, 20), "Score:" + score);
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            CanDoubleJump = false;
        }

        if(collision.gameObject.tag == "Obstacle")
        {
            heartManager.TakeDamage();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            isGrounded = false;
    }




}