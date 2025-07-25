using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public TextMeshProUGUI textAttack;
    public TextMeshProUGUI textDefense;

    private int attack = 100;
    private int defense = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        textAttack.text = attack.ToString();
        textDefense.text = defense.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerStat(int itemAttack, int itemDefense)
    {
        attack += itemAttack;
        defense += itemDefense;

        textAttack.text = attack.ToString();
        textDefense.text = defense.ToString();
    }
}
