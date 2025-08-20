using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���׷��̵� ���� �˾�.
/// </summary>
public class UpgradePopupUI : MonoBehaviour
{
    public GameObject panel;
    public Image[] icons;
    public TextMeshProUGUI[] titles;
    public TextMeshProUGUI[] descs;
    public Button[] buttons;

    public UpgradeApplier applier; // ���� ���׷��̵� ������ ����� ����.

    List<UpgradeDefinitionSO> current;

    private void Awake()
    {
        if(applier == null)
        {
            applier = FindAnyObjectByType<UpgradeApplier>();
            Hide();
        }
    }

    public void ShowChoices(List<UpgradeDefinitionSO> choices)
    {
        current = choices ?? new List<UpgradeDefinitionSO>();

        for(int i = 0; i < 3; i++)
        {
            var has = (i < current.Count && current[1] != null);
            buttons[i].interactable = has;
            //icons[i].gameObject.SetActive(has);
            titles[i].gameObject.SetActive(has);
            descs[i].gameObject.SetActive(has);

            if(has == true)
            {
                var u = current[i];
                //icons[i].sprite = u.Icon;
                titles[i].text = u.displayName;
                descs[i].text = u.description;
                int idx = 1;
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].onClick.AddListener(() => OnChoose(idx));
            }
            else
            {
                titles[i].text = "-";
                descs[i].text = "";
                buttons[i].onClick.RemoveAllListeners();
            }
        }
        panel.SetActive(true);
        Time.timeScale = 0.0f;  // �Ͻ�����
    }

    void Hide()
    {
        panel.SetActive(false);
    }

    void OnChoose(int idx)
    {
        if(current != null && idx >= 0 && idx < current.Count)
        {
            applier?.Apply(current[idx]);
        }
        Hide();
        Time.timeScale = 1.0f;
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
