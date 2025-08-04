using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    public Inventory inventory;
    public ItemData testItem;


    // Update is called once per frame
    void Update()
    {
 
    }

    private void OnTriggerEnter2D(Collider2D collision)

    {

        Debug.Log($"OnCollisionEnter2D:{collision.gameObject.name}");
        if (collision.gameObject.tag == "Player")
        {
            inventory.AddItem(testItem);
            Debug.Log("아이템 추가됨");

            Destroy(this.gameObject);
        }
    }
}
