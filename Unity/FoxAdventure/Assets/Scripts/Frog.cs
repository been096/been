using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 1;
    public float jumpPower = 100;
    public bool Isjump;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (!Isjump)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpPower);
            Isjump = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject)
            Isjump = false;

        Debug.Log($"OnCollisionEnter2D:{collision.gameObject.name}");
        if (collision.gameObject.name == "player")
        {


            Destroy(collision.gameObject);
            //Destroy(this.gameObject);
        }
    }
}
