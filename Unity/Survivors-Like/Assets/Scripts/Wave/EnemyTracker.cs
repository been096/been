using UnityEngine;

/// <summary>
/// [7����~] �� �����ֱ� ������
/// - �ı� �� WaveManager�� ���� �� �� ���� ����
/// - [11����] StatsTracker�� óġ �� +1 ����
/// ����: ������ Stop �ÿ��� OnDestroy�� ȣ��ǹǷ�, ���� �÷��� ������ Ȯ��
/// </summary>
public class EnemyTracker : MonoBehaviour
{
    public WaveManager manager;

    public void ProcessDestroy()
    {
        // �÷��� ���� �ƴϸ�(������ ���� ��) ��� ���� ����
        if (Application.isPlaying == false)
        {
            return;
        }

        // ���� �� �� ����
        if (manager != null)
        {
            manager.NotifyEnemyDestroyed();
        }

        // óġ �� +1
        StatsTracker stats = FindAnyObjectByType<StatsTracker>();
        if (stats != null)
        {
            stats.AddKill(1);
        }
    }
}