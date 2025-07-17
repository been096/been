using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject slotprefab;
    public Transform slotParent;
    public int slotCount = 20;
    public List<Slot> slots = new List<Slot>();


    /// <summary>
    /// 정해진 개수만큼 인벤토리에 슬롯을 만든다.
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < slotCount; ++i)
        {
            GameObject go = Instantiate(slotprefab, slotParent);
            Slot s = go.GetComponent<Slot>();
            s.ClearSlot();
            slots.Add(s);
        }
    }


    /// <summary>
    /// 현재 빈 슬롯이 있는지 검사해서 아이템을 추가한다.
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(ItemData item)
    {
        foreach (Slot s in slots)
        {
            if (s.HasItem() == false)
            {
                s.AddItem(item);
                break;
            }
        }
    }
}
