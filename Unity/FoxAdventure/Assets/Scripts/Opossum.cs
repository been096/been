using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossum : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"OnCollisionEnter2D:{collision.gameObject.name}");
        if (collision.gameObject.name == "player")
        {
            

            Destroy(collision.gameObject);
            //Destroy(this.gameObject);
        }
    }
}
