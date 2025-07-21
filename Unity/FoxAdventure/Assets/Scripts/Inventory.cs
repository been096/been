using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject slotprefab;
    public Transform slotParent;
    public int slotCount = 20;
    public List<Slot> slots = new List<Slot>();
    public GameObject inventory;


    /// <summary>
    /// ������ ������ŭ �κ��丮�� ������ �����.
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
        CloseInventory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            inventory.SetActive(!inventory.activeSelf);

    }


    /// <summary>
    /// ���� �� ������ �ִ��� �˻��ؼ� �������� �߰��Ѵ�.
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

    public void CloseInventory()
    {
        inventory.SetActive(false);
    }
}
