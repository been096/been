using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossum : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1.0f;
    public float timer = 0.0f;
    public float switchtime = 2.0f;
    private float dir = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        transform.Translate(Vector2.left * dir * speed * Time.deltaTime);

        if(timer >= switchtime)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            dir = dir *  -1f;
            timer = 0.0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"OnCollisionEnter2D:{collision.gameObject.name}");
        if (collision.gameObject.tag == "Player")
        {
            

            Destroy(collision.gameObject);
            //Destroy(this.gameObject);
        }
    }
}
