using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UpgradeKind
{
    MoveSpeedPercent,
    AttackSpeedPercent,
}

[CreateAssetMenu(menuName = "Game/Upgrade/Definition", fileName = "Upgrade")]

public class UpgradeDefinitionSO : ScriptableObject
{
    public string displayName;  // �̸�.
    public string description;  // ����.
    public Sprite Icon;

    public UpgradeKind kind;
    public float value;

    public int weight = 1;

}
