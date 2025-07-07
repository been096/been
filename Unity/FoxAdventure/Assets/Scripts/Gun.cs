using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject objBullet;
    public GameObject prefabBullet;
    public float shotPower = 300;
    public float time;
 

    public void Shot() // 총알이 힘을 받아 조준한 방향으로 나간다.
    {
        objBullet = Instantiate(prefabBullet, transform.position, Quaternion.identity);
        Rigidbody2D rigidbodyBullet = objBullet.GetComponent<Rigidbody2D>();
        rigidbodyBullet.AddForce(Vector3.right * shotPower);
        Destroy(objBullet, 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    Shot();
        //}
    }
}
