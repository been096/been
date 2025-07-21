using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image icon; // �κ��丮�� �� ���Կ� ������ �������� ������ �̹���.
    private ItemData currentItem; // ���� ���Կ� ����Ǿ��ִ� �������� ����.
    private bool equipped = false;



    /// <summary>
    /// ���Կ� ������ ������ �߰��ϴ� �Լ�.
    /// </summary>
    /// <param name="item">���� �߰��� ������ ������ ��� �ִ� ��ü.</param>
    public void AddItem(ItemData item)
    {
        currentItem = item; // ���� ���Կ� �߰��� �������� ���� ��ü�� ����.
        icon.sprite = item.icon; // ���Կ� ������ ������ �̹��� ������ ����.
        icon.enabled = true; // ������ �̹����� Ȱ��ȭ��Ų��.
    }

    /// <summary>
    /// ������ ������ �ʱ�ȭ��Ų��.
    /// </summary>
    public void ClearSlot()
    {
        //����Ǿ� �ִ� ������ �������� ��� �����ش�.
        currentItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    /// <summary>
    /// ���� ���Կ� ������ ������ �ִ��� üũ�Ѵ�.
    /// </summary>
    /// <returns></returns>
    public bool HasItem()
    {
        return currentItem != null;
    }

    /// <summary>
    /// ������ Ŭ������ �� ȣ���� �Լ�.
    /// </summary>
    public void OnClick()
    {
        if (HasItem() == true)
        {
            //������ ������ ���������� �˻�. ���� �������̸� ����, �׷��� ������ ����.
            if (IsPossbleEuqip() == true)
            {
                if(equipped == true)
                {
                    Unequip();
                }
                else
                {
                    EquipItem();
                }
            }
            else
            {
                Debug.Log("������ ��� : " + currentItem.itemName);
            }
                //Debug.Log("������ ��� : " + currentItem.itemName);
        }
    }

    public bool IsPossbleEuqip()
    {
        return currentItem.type == Itemtype.Weapon || currentItem.type == Itemtype.Armour || currentItem.type == Itemtype.Acc;
    }

    public void EquipItem()
    {
        Debug.Log("������ ���� : " + currentItem.itemName);
        equipped = true;

        PlayerStat playerStat = GameObject.FindAnyObjectByType<PlayerStat>();
        if (playerStat != null)
        {
            playerStat.UpdatePlayerStat(currentItem.Attack, currentItem.Defense);
        }
    }

    public void Unequip()
    {
        Debug.Log("������ ���� : " + currentItem.itemName);
        equipped = false;

        PlayerStat playerStat = GameObject.FindAnyObjectByType<PlayerStat>();
        if (playerStat != null)
        {
            playerStat.UpdatePlayerStat(-currentItem.Attack, -currentItem.Defense);
        }
    }
}
