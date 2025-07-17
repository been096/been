using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Inventory inventory;
    public ItemData testItem;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) == true)
        {
            inventory.AddItem(testItem);
            Debug.Log("아이템 추가됨");
        }
    }
}
