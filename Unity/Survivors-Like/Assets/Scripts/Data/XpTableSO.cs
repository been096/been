using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Progression/XP Table", fileName = "XpTable")]
public class XpTableSO : ScriptableObject
{
    public int[] requirements;

    /// <summary>
    /// ���� �������� �ʿ��� ����ġ�� ��ȯ.
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public int GetRequirementForLevel(int level)
    {
        int idx = Mathf.Max(0, level - 1);
        if (requirements == null || idx < 0 || idx >= requirements.Length)
        {
            // ���̺� ������ ����� �� �̻� �������� �� �� ������ ó��.
            return int.MaxValue;
        }
        return Mathf.Max(1, requirements[idx]);
    }

    /// <summary>
    /// �ִ� ������ ��ȯ�Ѵ�.
    /// </summary>
    /// <returns></returns>
    public int GetMaxLevel()
    {
        if (requirements == null)
        {
            return 1;
        }

        return requirements.Length + 1;
    }
}
