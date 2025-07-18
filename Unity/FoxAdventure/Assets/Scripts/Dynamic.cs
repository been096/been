using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 점프 상태가 아닐때 -> 강체에다가 힘을 가해서 점프한다. 점프 상태가 트루가 된다.
// 
public class Dynamic : MonoBehaviour
{
    public int score;
    public float speed = 1; // 한번에 얼마나 이동할지 지정
    public float jumpPower = 100;
    public bool Isjump;

    public Gun gun;

    public Vector3 vDir;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
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
            if (!Isjump)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpPower);
                Isjump = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
            gun.Shot();
    }

    private void OnDestroy()
    {
        //죽은 위치에서 부활함
        //GameObject objPlayer = Instantiate(this.gameObject);
        //objPlayer.SetActive(true);
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 200, 20), "Score:" + score);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject)
            Isjump = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)

    {
        
        ///아이템이 추가될때마다 경우의 수가 늘어나고, 코드를 변경해야한다. 굉장히 귀찮음.
        //Debug.Log($"OnCollisionEnter2D:{collision.gameObject.name}");
        //if (collision.gameObject.name == "cherry")
        //{
        //    score++;
        //    Destroy(collision.gameObject);
        //    //Destroy(this.gameObject);
        //}
        
        //if (collision.gameObject.name == "gem")
        //{
        //    score += 100;
        //    Destroy(collision.gameObject);
        //    //Destroy(this.gameObject);
        //}
    }

}