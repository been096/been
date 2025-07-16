using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image icon; // �κ��丮�� �� ���Կ� ������ �������� ������ �̹���.
    private ItemData currentItem; // ���� ���Կ� ����Ǿ��ִ� �������� ����.



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
        if(HasItem() == true)
        {
            Debug.Log("������ ��� : " + currentItem.itemName);
        }
    }
}
