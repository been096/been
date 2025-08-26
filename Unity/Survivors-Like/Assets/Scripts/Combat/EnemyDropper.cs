using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyDropper : MonoBehaviour
{
    public GameObject xpOrbPrefab; // XP ���� ������
    public int minCount = 1;       // �ּ� ����
    public int maxCount = 3;       // �ִ� ����
    public int xpPerOrb = 3;       // ���� �ϳ��� XP
    public float spreadRadius = 0.5f; // ��� ���� �ݰ�

    //void OnDestroy()
    public void CreateOrb()
    {
        // ���� ����� ���� OnDestroy�� �ҷ���, ������ Stop �� ����� �� ����.
        // �׷� ��Ȳ�� ���ϰ� ������ ������ �ϳ� �� �ɾ ��.
        if (xpOrbPrefab == null)
        {
            return;
        }

        int count = Random.Range(minCount, maxCount + 1);
        int i = 0;
        while (i < count)
        {
            Vector2 offset = Random.insideUnitCircle * spreadRadius;
            Vector3 pos = transform.position + new Vector3(offset.x, offset.y, 0f);

            GameObject orb;

            // Ǯ �Ŵ����� ������ ���(����)
            if (PoolManager.Instance != null)
            {
                orb = PoolManager.Instance.Spawn(xpOrbPrefab, pos, Quaternion.identity, null);
                XpOrb xpOrb = orb.GetComponent<XpOrb>();
                if (xpOrb != null)
                {
                    xpOrb.SetKeyPrefab(xpOrbPrefab);
                }
            }
            else
            {
                orb = Instantiate(xpOrbPrefab, pos, Quaternion.identity);
            }

            // ���꿡 XP �� ����
            XpOrb xo = orb.GetComponent<XpOrb>();
            if (xo != null)
            {
                xo.xpValue = xpPerOrb;
            }

            i = i + 1;
        }
    }
}
