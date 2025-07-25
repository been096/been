using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image icon; // 인벤토리의 각 슬롯에 보여줄 아이템의 아이콘 이미지.
    private ItemData currentItem; // 현재 슬롯에 저장되어있는 아이템의 정보.



    /// <summary>
    /// 슬롯에 아이템 정보를 추가하는 함수.
    /// </summary>
    /// <param name="item">새로 추가할 아이템 정보가 담겨 있는 객체.</param>
    public void AddItem(ItemData item)
    {
        currentItem = item; // 현재 슬롯에 추가할 아이템의 정보 객체를 저장.
        icon.sprite = item.icon; // 슬롯에 보여줄 아이콘 이미지 정보를 저장.
        icon.enabled = true; // 아이콘 이미지를 활성화시킨다.
    }

    /// <summary>
    /// 슬롯의 정보를 초기화시킨다.
    /// </summary>
    public void ClearSlot()
    {
        //저장되어 있던 아이템 정보들을 모두 지워준다.
        currentItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    /// <summary>
    /// 현재 슬롯에 아이템 정보가 있는지 체크한다.
    /// </summary>
    /// <returns></returns>
    public bool HasItem()
    {
        return currentItem != null;
    }

    /// <summary>
    /// 슬롯을 클릭했을 때 호출할 함수.
    /// </summary>
    public void OnClick()
    {
        if (HasItem() == true)
        {
            Debug.Log("아이템 사용 : " + currentItem.itemName);
        }
    }
}
