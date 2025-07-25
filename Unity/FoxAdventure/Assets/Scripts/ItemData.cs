using System.ComponentModel;
using UnityEngine;

public enum Itemtype // enum -> 열거형, 데이터의 타입을 정의할 때 쓰는 것. 타입을 분류할 때 많이 쓴다.
{
    Weapon,
    Armour,
    Acc,
    Item,
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]

public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public Itemtype type;
    public int Attack;
    public int Defense;
    public int Score;
    public string description;
}
