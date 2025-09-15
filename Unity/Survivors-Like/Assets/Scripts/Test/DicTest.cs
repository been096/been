using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo
{
    public int id;
    public string name;
    public int price;
    public int itemtype;
}

public class DicTest : MonoBehaviour
{
    Dictionary<int, ItemInfo> dic;
    // Start is called before the first frame update
    void Start()
    {
        dic = new Dictionary<int, ItemInfo>();
        ItemInfo item = new ItemInfo();
        item.id = 11111;
        item.name = "Sword";
        item.price = 1000;
        item.itemtype = 0;
        dic.Add(item.id, item);

        ItemInfo item2 = new ItemInfo();
        item.id = 22222;
        item.name = "Gun";
        item.price = 1500;
        item.itemtype = 1;
        dic.Add(item.id, item2);

        //ItemInfo item3 = dic[11111];
        ItemInfo item3;
        dic.TryGetValue(11111, out item3);  // 훨씬 안전하게? 값을 가져오는 방법.
        Debug.Log("item id" + item3.id);
        Debug.Log("item name" + item3.name);
        Debug.Log("item price" + item3.price);
        Debug.Log("item itemtype" + item3.itemtype);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
