using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Dynamic : MonoBehaviour
{
    public float speed = 1;
    public float JumpPower = 100;
    
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
            GetComponent<Rigidbody2D>().AddForce(Vector3.up * JumpPower);
    }
}
