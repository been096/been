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

        if(pools.TryGetValue(prefab, out var q) == false) // 딕셔너리 안에 해당 키에 매치되는 값이 있는지를 체크해서 그걸 가져오는 함수.
        {
            q = new Queue<GameObject>();
            pools[prefab] = q;
        }

        GameObject go = (q.Count > 0) ? q.Dequeue() : Instantiate(prefab); // Dequeue -> gameObject 안에 수량이 있으면 빼온다는 의미의 함수
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
