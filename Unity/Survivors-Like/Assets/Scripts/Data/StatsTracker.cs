using UnityEngine;

/// <summary>
/// - ���� �ð�(��) ����
/// - óġ �� ����(AddKill ȣ��� ����)
/// - ���� ��� ��ȸ(���â ǥ���)
/// </summary>
public class StatsTracker : MonoBehaviour
{
    [Header("State (��Ÿ�� ����)")]
    public float survivalTimeSeconds = 0f;  // ���ݱ��� ��ƾ �ð�(��)
    public int killCount = 0;             // ���� óġ ��

    [Header("References")]
    public GoldSystem gold;                 // Player�� GoldSystem

    void Start()
    {
        // gold�� ��� ������ �� �� �ڵ� Ž��(������ ���� �ν����� ���� ����)
        if (gold == null)
        {
            gold = FindAnyObjectByType<GoldSystem>();
        }
    }

    void Update()
    {
        // �ǵ� ��Ȯȭ: �Ͻ����� �߿��� �ð� ���� �� ��
        if (Time.timeScale != 0f)
        {
            survivalTimeSeconds = survivalTimeSeconds + Time.deltaTime;
        }
    }

    public void AddKill(int amount)
    {
        if (amount > 0)
        {
            killCount = killCount + amount;
        }
    }

    public string GetFormattedTime()
    {
        int total = Mathf.FloorToInt(survivalTimeSeconds);
        int minutes = total / 60;
        int seconds = total % 60;
        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public int GetCurrentGold()
    {
        if (gold == null)
        {
            return 0;
        }
        else
        {
            return gold.currentGold;
        }
    }
}
