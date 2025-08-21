using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public WaveManager manager; // WaveManager�� �ν����ͷ� �����ϰų� ���� �� �ٿ���

    void OnDestroy()
    {
        if (manager != null)
        {
            manager.NotifyEnemyDestroyed();
        }
    }
}
