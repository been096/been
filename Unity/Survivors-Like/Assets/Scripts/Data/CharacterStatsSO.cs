using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Data/Character Stats", fileName = "CharacterStats")]
public class CharacterStatsSO : ScriptableObject
{
    
    public string displayName = "Player";
    public int baseMaxHP = 100;
    public float baseMoveSpeed = 5.0f;

}
