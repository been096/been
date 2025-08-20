using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �� ���׷��̵� īŻ�α׿��� 3���� �̾Ƽ� �˾� UI�� ����
/// </summary>

public class LevelupPicker : MonoBehaviour
{
    public XpSystem xpSystem;
    public UpgreadCatalogSO catalog;
    public UpgradePopupUI popupUI;

    private System.Random rng = new System.Random();

    private void Awake()
    {
        if(xpSystem == null)
        {
            xpSystem = FindAnyObjectByType<XpSystem>();
        }

        if(popupUI == null)
        {
            popupUI = FindAnyObjectByType<UpgradePopupUI>();
        }
    }

    private void OnEnable()
    {
        if(xpSystem != null)
        {
            xpSystem.OnLevelUp += HandlelevelUp;
        }
    }

    private void OnDisable()
    {
        if(xpSystem != null)
        {
            xpSystem.OnLevelUp -= HandlelevelUp;
        }
    }

    void HandlelevelUp(int newLevel)
    {
        var picks = PickUnique(3);
        popupUI?.ShowChoices(picks);
    }

    List<UpgradeDefinitionSO> PickUnique(int count)
    {
        var result = new List<UpgradeDefinitionSO>(count);
        int guard = 50;

        while(result.Count < count && guard-- > 0)
        {
            var u = catalog.RollOne(rng);
            if(u == null)
            {
                break;
            }
            if(result.Contains(u) == false)
            {
                result.Add(u);
            }
        }

        return result;
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
