using UnityEngine;

public class DifficultyDirector : MonoBehaviour
{
    [Header("Time �� Difficulty")]
    public float stepSeconds = 20f;      // �� �ð��� ���� ������ ���̵� +1
    public int maxDifficulty = 10;     // ����(���� ��� ����)

    [Header("Elite Chance (0~1)")]
    public float eliteChanceStart = 0.05f;  // 5%
    public float eliteChancePerLevel = 0.03f;
    public float eliteChanceMax = 0.40f;    // 40%

    [Header("Elite Multipliers")]
    public float eliteHpPercentPerLevel = 20f;    // ���� 1�� HP +20%
    public float eliteSpeedPercentPerLevel = 8f;  // ���� 1�� �̼� +8%
    public float eliteXpPercentPerLevel = 15f;    // ���� 1�� XP ��� +15%
    public float eliteGoldPercentPerLevel = 15f;  // ���� 1�� ��� ��� +15%

    [Header("Boss")]
    public int bossEveryNWaves = 5;  // 5���̺긶�� ����

    // ���� ����
    public int difficultyLevel = 0;
    private float elapsed = 0f;

    void Update()
    {
        float dt = Time.deltaTime;
        elapsed = elapsed + dt;

        // ���̵� ���
        int targetLevel = Mathf.FloorToInt(elapsed / stepSeconds);
        if (targetLevel > difficultyLevel)
        {
            difficultyLevel = targetLevel;
            if (difficultyLevel > maxDifficulty)
            {
                difficultyLevel = maxDifficulty;
            }
        }
    }

    public bool IsBossWave(int waveNumber)
    {
        if (bossEveryNWaves <= 0)
        {
            return false;
        }
        if (waveNumber <= 0)
        {
            return false;
        }
        if (waveNumber % bossEveryNWaves == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool RollIsElite()
    {
        float chance = eliteChanceStart + (eliteChancePerLevel * difficultyLevel);
        if (chance > eliteChanceMax)
        {
            chance = eliteChanceMax;
        }
        float roll = Random.value; // 0~1
        if (roll < chance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // ���� ���̵������� ���(%)�� ����� �ش�.
    public float GetEliteHpPercent()
    {
        float v = eliteHpPercentPerLevel * difficultyLevel;
        return v;
    }

    public float GetEliteSpeedPercent()
    {
        float v = eliteSpeedPercentPerLevel * difficultyLevel;
        return v;
    }

    public float GetEliteXpPercent()
    {
        float v = eliteXpPercentPerLevel * difficultyLevel;
        return v;
    }

    public float GetEliteGoldPercent()
    {
        float v = eliteGoldPercentPerLevel * difficultyLevel;
        return v;
    }
}
