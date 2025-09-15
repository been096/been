using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]  // ����ȭ. ������ ���� ����.

public class RegistryEntry
{
    public string id;
    public GameObject prefab;
}

public class PrefabRegistry : MonoBehaviour
{
    public RegistryEntry[] entries;    // ����Ƽ �ν����Ϳ��� �ش� ������ �迭�� ���� �߰����ش�.

    //Dictionary<string, GameObject> dic = new Dictionary<string, GameObject>();
    List<RegistryEntry> list = new List<RegistryEntry>();
    

    // Start is called before the first frame update
    void Start()
    {
        CreatDic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreatDic()
    {
        //dic.Clear();

        list.Clear();

        //for (int i = 0; i < entries.Length; ++i)       // �迭�� �о ��ųʸ��� Ű�� ���� �߰��ϴ� ����.
        //{
        //    dic.Add(entries[i].id, entries[i].prefab);
        //}

        for (int i = 0; i < entries.Length; ++i)       // �迭�� �о ��ųʸ��� Ű�� ���� �߰��ϴ� ����.
        {
            list.Add(entries[i]);
        }
    }

    public GameObject GetPrefab(string id)
    {
        //if (string.IsNullOrEmpty(id) == true)   // IsNullorEmpty -> string�� �����ϴ� �Լ��� string�� Null���� Empty���� �Ǻ����ִ� �Լ�
        //{
        //    return null;
        //}

        //GameObject go;
        //bool ok = dic.TryGetValue(id, out go);        // TryGetValue�� Bool Ÿ�� ��ȯ. ������ ���� ȣ�� �� �ϳ��� out�� �Ἥ go�� dic�� ���� preFab�� ����־ ���� �ٲ� ���� �� ���� ������ִ� ��.
        //if (ok == true)
        //{
        //    return go;
        //}

        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i].id == id)
            {
                return list[i].prefab;
            }
        }

        return null;
    }

    public GameObject SpawnbyID(string id, Vector3 position, Quaternion rotation, Transform parent)    //  ���������� �ܺο��� ȣ���ϴ� �Լ�.
    {
        GameObject go = GetPrefab(id);          //SpawnbyID�� ȣ�� ���� �� ���ο��� �ش� ������Ʈ�� id�� �´� prefab�� ã�´�.
        if (go == null)
        {
            return null;
        }

        GameObject obj = Instantiate(go, position, rotation, parent);   // ã������ �ش� prefab�� �����Ѵ�.
        return obj;
    }

}
