using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject shopPanel;  // ���� �г�(�Ѱ�/����)
    public TextMeshProUGUI infoText;         // "���� �Ϸ�!" �Ǵ� "��� ����!" ǥ��

    [Header("Prices")]
    public int priceMoveSpeed = 30;
    public int priceAttackSpeed = 40;
    public int priceHeal = 20;

    [Header("Effects")]
    public float moveSpeedPercent = 10f;   // +10%
    public float attackSpeedPercent = 15f; // +15%
    public int healAmount = 20;            // +20 HP

    [Header("Targets")]
    public GoldSystem gold;
    public PlayerCore player;
    public AutoAttackController autoAttack;

    void Start()
    {
        if (gold == null)
        {
            gold = FindAnyObjectByType<GoldSystem>();
        }

        if (player == null)
        {
            player = FindAnyObjectByType<PlayerCore>();
        }

        if (autoAttack == null)
        {
            autoAttack = FindAnyObjectByType<AutoAttackController>();
        }

        CloseShop();
    }

    void Update()
    {
        // B Ű�� ����/�ݱ�
        if (Input.GetKeyDown(KeyCode.B) == true)
        {
            if (shopPanel.activeSelf == true)
            {
                CloseShop();
            }

            else OpenShop();
        }
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        Time.timeScale = 0f; // �Ͻ�����
        if (infoText != null)
        {
            infoText.text = "���ϴ� ��ȭ�� �����ϼ���.";
        }
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        Time.timeScale = 1f; // �簳
    }

    // ��ư�� �Լ� 3�� (�ν����Ϳ��� ���� ����)
    public void BuyMoveSpeed()
    {
        if (gold == null)
        {
            CloseShop();
            return;
        }

        if (gold.CanAfford(priceMoveSpeed) == false)
        {
            if (infoText != null)
            {
                infoText.text = "��尡 �����մϴ�.";
            }

            return;
        }
        if (gold.Spend(priceMoveSpeed) == false)
        {
            if (infoText != null)
            {
                infoText.text = "���� ����!";
            }
            CloseShop();
            return;
        }
        if (player != null)
        {
            player.AddMoveSpeedMultiplier(moveSpeedPercent);
        }

        if (infoText != null)
        {
            infoText.text = "�̵��ӵ� +" + moveSpeedPercent + "% ����!";
        }

        CloseShop();
    }

    public void BuyAttackSpeed()
    {
        if (gold == null)
        {
            CloseShop();
            return;
        }

        if (gold.CanAfford(priceAttackSpeed) == false)
        {
            if (infoText != null)
            {
                infoText.text = "��尡 �����մϴ�.";
            }

            CloseShop();
            return;
        }
        if (gold.Spend(priceAttackSpeed) == false)
        {
            if (infoText != null)
            {
                infoText.text = "���� ����!";
            }

            CloseShop();
            return;
        }

        if (autoAttack != null)
        {
            autoAttack.AddAttackSpeedMultiplier(attackSpeedPercent);
        }

        if (infoText != null)
        {
            infoText.text = "���ݼӵ� +" + attackSpeedPercent + "% ����!";
        }

        CloseShop();
    }

    public void BuyHeal()
    {
        if (gold == null)
        {
            CloseShop();
            return;
        }

        if (gold.CanAfford(priceHeal) == false)
        {
            if (infoText != null)
            {
                infoText.text = "��尡 �����մϴ�.";
            }

            CloseShop();
            return;
        }
        if (gold.Spend(priceHeal) == false)
        {
            if (infoText != null)
            {
                infoText.text = "���� ����!";
            }

            CloseShop();
            return;
        }
        if (player != null)
        {
            player.Heal(healAmount);
        }

        if (infoText != null)
        {
            infoText.text = "ü�� +" + healAmount + " ����!";
        }

        CloseShop();
    }

    // ���� ����(����): ���� Ȯ�� �� ���� �� ȿ�� ���� �� �ȳ� �� �ݱ�
    void TryBuy(int price, string label, System.Action apply)
    {
        if (gold == null)
        {
            return;
        }

        if (!gold.CanAfford(price))
        {
            if (infoText != null)
            {
                infoText.text = "��尡 �����մϴ�.";
            }

            return;
        }

        bool ok = gold.Spend(price);
        if (!ok)
        {
            if (infoText != null)
            {
                infoText.text = "���� ����!";
            }

            return;
        }

        // ȿ�� ����
        if (apply != null)
        {
            apply();
        }

        if (infoText != null)
        {
            infoText.text = label + " ����!";
        }

        CloseShop();
    }
}