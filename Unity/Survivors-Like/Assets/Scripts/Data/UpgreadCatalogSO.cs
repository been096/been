using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

[CreateAssetMenu(menuName = "Game/Upgrade/Catalog", fileName = "UpgradeCatalog")]

public class UpgreadCatalogSO : ScriptableObject
{
    public UpgradeDefinitionSO[] all;

    /// ����ġ ���� ����.
    /// 

    public int TotalWeight()
    {
        int sum = 0;
        if(all != null)
        {
            foreach (var u in all)
            {
                if(u != null && u.weight > 0)
                {
                    sum += u.weight;
                }
            }
        }

        return Mathf.Max(1, sum);
    }

    /// <summary>
    /// ����ġ ������� 1���� �̴´�.
    /// </summary>
    /// <param name="rng"></param>
    /// <returns></returns>
    public UpgradeDefinitionSO RollOne(System.Random rng)
    {
        if (all == null || all.Length == 0)
        {
            return null;
        }

        int total = TotalWeight();
        int pick = rng.Next(0, total);
        int acc = 0;

        foreach (var u in all)
        {
            if (u == null || u.weight <= 0)
            {
                continue;
            }

            acc += u.weight;
            if(pick < acc)
            {
                return u;
            }
        }

        return all[0];
    }
}
