using UnityEngine;

public class GoldSystem : MonoBehaviour
{
    [Header("State")]
    public int currentGold = 0;   // ���� ���

    // ��� ���ϱ�(���� ���� �� ȣ��)
    public void AddGold(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        currentGold = currentGold + amount;
    }

    // �� �� �ִ��� Ȯ��
    public bool CanAfford(int price)
    {
        if (price < 0)
        {
            return false;
        }

        return currentGold >= price;
    }

    // ����(�����ϸ� true)
    public bool Spend(int price)
    {
        if (CanAfford(price) == false)
        {
            return false;
        }

        currentGold = currentGold - price;
        return true;
    }
}