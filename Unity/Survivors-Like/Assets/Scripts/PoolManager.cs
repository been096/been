using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    private Dictionary<GameObject, Queue<GameObject>> pools = new();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot, Transform parent = null)
    {
        if(prefab == null)
        {
            return null;
        }

        if(pools.TryGetValue(prefab, out var q) == false) // ��ųʸ� �ȿ� �ش� Ű�� ��ġ�Ǵ� ���� �ִ����� üũ�ؼ� �װ� �������� �Լ�.
        {
            q = new Queue<GameObject>();
            pools[prefab] = q;
        }

        GameObject go = (q.Count > 0) ? q.Dequeue() : Instantiate(prefab); // Dequeue -> gameObject �ȿ� ������ ������ ���´ٴ� �ǹ��� �Լ�
        if(parent != null)
        {
            go.transform.SetParent(parent, false);
            
        }

        go.transform.SetPositionAndRotation(pos, rot);
        go.SetActive(true);
        return go;
    }


    public void Release(GameObject prefab, GameObject instance)
    {
        if(prefab == null || instance == null)
        {
            return;
        }

        instance.SetActive(false);
        instance.transform.SetParent(transform, false);
        if(pools.TryGetValue(prefab, out var q) == false)
        {
            q = new Queue<GameObject>();
            pools[prefab] = q;
        }
        
        q.Enqueue(instance);
    }
}
