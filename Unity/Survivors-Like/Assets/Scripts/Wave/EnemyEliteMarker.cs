using UnityEngine;

public class EnemyEliteMarker : MonoBehaviour
{
    [Header("Visual")]
    public Color eliteColor = new Color(0.8f, 0.2f, 1f, 1f); // ���� �迭

    // DifficultyDirector���� ����� %�� �޾� ����
    public void ApplyElite(DifficultyDirector dir)
    {
        if (dir == null)
        {
            return;
        }

        // 1) �� ����(�����ϸ�)
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = eliteColor;
        }

        DamageFlash flash = GetComponent<DamageFlash>();
        if (flash != null)
        {
            flash.SetBaseColor(eliteColor);
        }

        // 2) ü�� ��� (Health�� �Լ� ��ġ�� �ʿ�)
        Health hp = GetComponent<Health>();
        if (hp != null)
        {
            float hpPercent = dir.GetEliteHpPercent(); // ��: 60 �� +60%
            // �Ʒ� MultiplyMaxHpPercent�� Health�� �߰��� ���� �Լ�(�Ʒ��� ����)
            hp.MultiplyMaxHpPercent(hpPercent);
        }

        // 3) �̵��ӵ� ��� (EnemyCore�� �ܺ� ���� �ʵ� ��ġ �ʿ�)
        EnemyCore ai = GetComponent<EnemyCore>();
        if (ai != null)
        {
            float spdPercent = dir.GetEliteSpeedPercent();
            ai.AddSpeedMultiplierPercent(spdPercent);
        }

        // 4) ��� ���ʽ� (���� ����)
        EnemyDropper xpDrop = GetComponent<EnemyDropper>();
        if (xpDrop != null)
        {
            float xpPercent = dir.GetEliteXpPercent();
            int bonus = Mathf.RoundToInt(xpDrop.xpPerOrb * (xpPercent / 100f));
            xpDrop.xpPerOrb = xpDrop.xpPerOrb + bonus;
        }

        EnemyGoldDropper goldDrop = GetComponent<EnemyGoldDropper>();
        if (goldDrop != null)
        {
            float gPercent = dir.GetEliteGoldPercent();
            int bonus = Mathf.RoundToInt(goldDrop.goldPerCoin * (gPercent / 100f));
            goldDrop.goldPerCoin = goldDrop.goldPerCoin + bonus;
        }
    }
}
