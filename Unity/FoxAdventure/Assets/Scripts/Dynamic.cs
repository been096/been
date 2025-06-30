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
    public bool Isground;
    public bool Isjump;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            transform.position += Vector3.right * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.position += Vector3.left * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!Isjump)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpPower);
                Isjump = true;
            }
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 200, 20), "Score:" + score);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"OnCollisionEnter2D:{collision.gameObject.name}");
        if (collision.gameObject.name == "cherry-1")
        {
            score++;
            Destroy(gameObject);
        }
        if (collision.gameObject)
            Isjump = false;
            
    }

}