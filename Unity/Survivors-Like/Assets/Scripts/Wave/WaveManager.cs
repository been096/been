using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform[] spawnPoints;          // ���� ���� ��ġ��

    [Header("Enemies")]
    public GameObject[] enemyPrefabs;        // �����ų �� ����(1���� �־ ��)
    public int[] enemyWeights;               // ����ġ(���� 2�� �̻��� ���� ���, ���̴� enemyPrefabs�� ����)

    [Header("Spawn Rules")]
    public float spawnInterval = 3f;         // ó�� ���� ����(��)
    public float minSpawnInterval = 0.7f;    // �ƹ��� ���� �� �� �Ʒ��δ� �� ������
    public int spawnCountPerBatch = 1;       // �� ���� �̴� ���� ��(ó�� 1)
    public int maxAlive = 30;                // ���ÿ� ����ִ� ���� ����

    [Header("Difficulty")]
    public float difficultyStepSeconds = 20f;// �� �ð����� ���̵� ���
    public float intervalStepDelta = 0.2f;   // ���̵� ��� �� ������ �پ��� ��
    public int batchStepEveryN = 2;        // ���̵� N�� �ø� ������ �� ���� �̴� �� +1

    [Header("References (Optional)")]
    public Transform enemiesParent;          // ������ �� ������ ����(��� ����)

    [Header("HUD (Optional)")]
    public TextMeshProUGUI waveText;     // "Wave 3" ���� ǥ��
    public TextMeshProUGUI timerText;    // "Next: 1.2s" ���� ǥ��
    public TextMeshProUGUI aliveText;    // "Alive: 12/30" ���� ǥ��

    // ���� ����(���� �ʵ常 ��)
    private float nextSpawnTimer = 0f;
    private float elapsed = 0f;
    private int difficultyStepCount = 0;
    private int alive = 0;
    private int waveNumber = 1;            // ������ �ѹ� �߻��� ������ +1 �Ϸ��� ���

    void Start()
    {
        // Ÿ�̸Ӹ� ó�� �������� ä���
        nextSpawnTimer = spawnInterval;

        // ����ġ �迭�� ����ų� ���̰� ���� ������, �յ� Ȯ���� ���
        if (enemyWeights == null || enemyWeights.Length != enemyPrefabs.Length)
        {
            enemyWeights = new int[enemyPrefabs.Length];
            int i = 0;
            while (i < enemyWeights.Length)
            {
                enemyWeights[i] = 1; // ���� 1��(�յ�)
                i++;
            }
        }
    }

    void Update()
    {
        // �ð� �긮��
        float dt = Time.deltaTime;
        elapsed += dt;
        nextSpawnTimer -= dt;

        // HUD ����(�ɼ�) ? ������ �̺�Ʈ �� ���� �׳� �� ������ �� ����
        UpdateHud();

        // ���̵� ��� üũ
        if (elapsed >= difficultyStepSeconds * (difficultyStepCount + 1))
        {
            difficultyStepCount = difficultyStepCount + 1;

            // ���� ���̱�(�ٴڰ� ����)
            spawnInterval = spawnInterval - intervalStepDelta;
            if (spawnInterval < minSpawnInterval) spawnInterval = minSpawnInterval;

            // �� ���� �� ���� ���� ũ�� +1
            if (batchStepEveryN > 0)
            {
                if (difficultyStepCount % batchStepEveryN == 0)
                {
                    spawnCountPerBatch = spawnCountPerBatch + 1;
                }
            }
        }

        // ���� Ÿ�̹� ����
        if (nextSpawnTimer <= 0f)
        {
            TrySpawnBatch();
            nextSpawnTimer = spawnInterval; // Ÿ�̸� ����
            waveNumber = waveNumber + 1;    // "���̺�" ������ ī����
        }
    }

    void TrySpawnBatch()
    {
        // ������ ������ �ƹ��͵� �� ��
        int room = maxAlive - alive;
        if (room <= 0) return;

        // �̹��� �� ���� �������� ����(���� ���)
        int toSpawn = spawnCountPerBatch;
        if (toSpawn > room) toSpawn = room;

        int i = 0;
        while (i < toSpawn)
        {
            SpawnOne();
            i = i + 1;
        }
    }

    void SpawnOne()
    {
        // ���� ����Ʈ�� ������ ���
        if (spawnPoints == null || spawnPoints.Length == 0) return;
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        // ���� ����Ʈ �̱�
        int spIndex = Random.Range(0, spawnPoints.Length);
        Transform sp = spawnPoints[spIndex];
        Vector3 pos = sp.position;

        // �� ���� ����ġ �̱�
        int eIndex = WeightedPickIndex();
        GameObject prefab = enemyPrefabs[eIndex];

        // �ν��Ͻ� ����(Ǯ ������ Ǯ ���, ������ Instantiate)
        GameObject go;
        if (PoolManager.Instance != null)
        {
            go = PoolManager.Instance.Spawn(prefab, pos, Quaternion.identity, enemiesParent);
        }
        else
        {
            if (enemiesParent != null)
                go = Instantiate(prefab, pos, Quaternion.identity, enemiesParent);
            else
                go = Instantiate(prefab, pos, Quaternion.identity);
        }

        // �� �� ���� ���� EnemyTracker �ޱ�(������ �ٿ��ֱ�)
        EnemyTracker tracker = go.GetComponent<EnemyTracker>();
        if (tracker == null) tracker = go.AddComponent<EnemyTracker>();
        tracker.manager = this;

        // �ö� �� �� �ݿ�
        alive = alive + 1;
    }

    int WeightedPickIndex()
    {
        // ���� ����ġ �� + ����
        int total = 0;
        int i = 0;
        while (i < enemyWeights.Length)
        {
            int w = enemyWeights[i];
            if (w < 0) w = 0;
            total = total + w;
            i = i + 1;
        }
        if (total <= 0) return 0;

        int pick = Random.Range(0, total);
        int acc = 0;
        int j = 0;
        while (j < enemyWeights.Length)
        {
            int w = enemyWeights[j];
            if (w < 0) w = 0;
            acc = acc + w;
            if (pick < acc) return j;
            j = j + 1;
        }
        return 0;
    }

    public void NotifyEnemyDestroyed()
    {
        // ���� �ı��� �� EnemyTracker�� �� �Լ��� �ҷ���
        alive = alive - 1;
        if (alive < 0) alive = 0;
    }

    void UpdateHud()
    {
        if (waveText != null)
        {
            waveText.text = "Wave " + waveNumber.ToString();
        }
        if (timerText != null)
        {
            // ���� �ð� ǥ�ÿ�(�Ҽ� 1�ڸ�)
            float t = nextSpawnTimer;
            if (t < 0f) t = 0f;
            timerText.text = "Next: " + t.ToString("F1") + "s";
        }
        if (aliveText != null)
        {
            aliveText.text = "Alive: " + alive.ToString() + " / " + maxAlive.ToString();
        }
    }
}
