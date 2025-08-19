using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ����ġ�� �����ϰ� ����ġ�� ���� �������� ó���ϴ� Ŭ����.
/// </summary>
public class XpSystem : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private XpTableSO xpTable; // ����Ƽ�� Inspector���� ������ ������, �ٸ� ��ũ��Ʈ������ ���� �Ұ�.

    [Header("State (�б����� ���ٿ� ������Ƽ ����")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentXp = 0;

    public int CurrentLevel => currentLevel;
    public int CurrentXp => currentXp;
    public int MaxLevel => xpTable ? xpTable.GetMaxLevel() : 1;

    public Action<int, int, int> OnXpChanged;
    public Action<int> OnLevelUp;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        NotifyXpChaged();
    }

    public void AddExp(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        if (currentLevel >= MaxLevel)
        {
            return;
        }

        currentXp += amount;

        // ���� ������ ó��.
        while (currentLevel < MaxLevel)
        {
            int required = GetRequiredXp();
            if (currentXp < required)
            {
                break;
            }

            currentXp -= required;  // �׿� ����ġ�� ���� ������ �̿�.
            currentLevel++;
            OnLevelUp?.Invoke(currentLevel);

            if (currentLevel >= MaxLevel)
            {
                currentXp = 0;
                break;
            }
        }

        // ����ġ ��ȭ�� ����.
        NotifyXpChaged();
    }

    public int GetRequiredXp()
    {
        if (xpTable == null)
        {
            return int.MaxValue;
        }

        return xpTable.GetRequirementForLevel(currentLevel);
    }

    private void NotifyXpChaged()
    {
        OnXpChanged?.Invoke(currentXp, GetRequiredXp(), currentLevel);
    }
}