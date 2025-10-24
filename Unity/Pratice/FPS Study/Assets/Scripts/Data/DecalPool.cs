using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ������ ���ӿ�����Ʈ Ǯ(��Į/ ����ũ �� ����).
/// </summary>
public class DecalPool : MonoBehaviour
{
    public int PrewarmCount = 50;               // ���� �� �̸� ���� ����.
    public float autoReturnAfter = 10.0f;       // �ڵ� ��ȯ �ð�(��)

    private List<GameObject> pooled = new List<GameObject>();       // Ǯ���� ��� ��.
    private List<GameObject> inUse = new List<GameObject>();        // ���� ��� ��.

    private void Awake()
    {
        // �̸� Ǯ������ ����(�������� �پ��ϹǷ� ��Ÿ�� ���� ��û �� ����)
    }

    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        GameObject go = null;

        // ���� ���������� �̸� ���� ������Ʈ�� ������ ����(�ܼ�ȭ�� ���� Ÿ�� �̰���)
        if (pooled.Count > 0)
        {
            go = pooled[pooled.Count - 1];
            pooled.RemoveAt(pooled.Count - 1);
            if (go != null)
            {
                go.transform.position = pos;
                go.transform.rotation = rot;
                go.SetActive(true);
                inUse.Add(go);
                StartCoroutine(AutoReturn(go, autoReturnAfter));
                return go;
            }
        }

        // ������ ���� ����.
        go = Instantiate(prefab, pos, rot);
        inUse.Add(go);
        StartCoroutine(AutoReturn(go, autoReturnAfter));
        return go;
    }

    public void Despawn(GameObject go)
    {
        if (go == null)
        {
            return;
        }
        bool removed = inUse.Remove(go);
        if (removed == true)
        {
            go.SetActive(false);
            go.transform.SetParent(transform, true);
            pooled.Add(go);
        }
    }

    private IEnumerator AutoReturn(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        Despawn(go);
    }
}
