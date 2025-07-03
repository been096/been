using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 1;
    public float jumpPower = 100;
    public bool Isjump;

    public float time;

    IEnumerator ProcessTimer()
    {
        Debug.Log("ProcessTimer() start");
        yield return new WaitForSeconds(time);
        Jump();
        Debug.Log("ProcessTimer() end");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Isjump)
            transform.position += Vector3.left * speed * Time.deltaTime;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject)
            Isjump = false;

        Debug.Log($"OnCollisionEnter2D:{collision.gameObject.name}");
        if (collision.gameObject.tag == "Player")
        {


            Destroy(collision.gameObject);
            //Destroy(this.gameObject);
        }

        StartCoroutine(ProcessTimer());
    }

    void Jump()
    {
        if (!Isjump)
        {
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * jumpPower);
            Isjump = true;
        }
    }
}
