using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  ī�� ���� ����� ���� ���� ������Ʈ�� ����.
/// </summary>
public class UpgradeApplier : MonoBehaviour
{
    public PlayerCore player;
    public AutoAttackContorller autoAttack;

    private void Awake()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerCore>();
        }

        if (autoAttack == null)
        {
            autoAttack = FindAnyObjectByType<AutoAttackContorller>();
        }
    }

    public void Apply(UpgradeDefinitionSO def)
    {
        if (def == null)
        {
            return;
        }

        switch(def.kind)
        {
            case UpgradeKind.MoveSpeedPercent:
                {
                    player?.AddMoveSpeedMultiplier(def.value);
                    Debug.Log("Upgrade MoveSpeed");
                }
                break;

            case UpgradeKind.AttackSpeedPercent:
                {
                    autoAttack.AddAttackSpeedMultiplier(def.value);
                    Debug.Log("Upgrade AttackSpeed");
                }
                break;

            default:
                {
                    Debug.Log("���� ��.....");
                }
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
