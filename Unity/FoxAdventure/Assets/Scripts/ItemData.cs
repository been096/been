using System.ComponentModel;
using UnityEngine;

public enum Itemtype
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
    public string description;
}
