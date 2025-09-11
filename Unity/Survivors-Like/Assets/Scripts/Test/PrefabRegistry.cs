using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]  // 직렬화. 변수로 쓰기 가능.

public class RegistryEntry
{
    public string id;
    public GameObject prefab;
}

public class PrefabRegistry : MonoBehaviour
{
    public RegistryEntry[] entries;    // 유니티 인스펙터에서 해당 적들을 배열에 직접 추가해준다.

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

        //for (int i = 0; i < entries.Length; ++i)       // 배열을 읽어서 딕셔너리에 키와 값을 추가하는 과정.
        //{
        //    dic.Add(entries[i].id, entries[i].prefab);
        //}

        for (int i = 0; i < entries.Length; ++i)       // 배열을 읽어서 딕셔너리에 키와 값을 추가하는 과정.
        {
            list.Add(entries[i]);
        }
    }

    public GameObject GetPrefab(string id)
    {
        //if (string.IsNullOrEmpty(id) == true)   // IsNullorEmpty -> string가 제공하는 함수로 string이 Null인지 Empty인지 판별해주는 함수
        //{
        //    return null;
        //}

        //GameObject go;
        //bool ok = dic.TryGetValue(id, out go);        // TryGetValue는 Bool 타입 반환. 참조에 의한 호출 중 하나인 out를 써서 go에 dic의 값인 preFab를 집어넣어서 값을 바꾼 다음 그 값을 출력해주는 것.
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

    public GameObject SpawnbyID(string id, Vector3 position, Quaternion rotation, Transform parent)    //  실질적으로 외부에서 호출하는 함수.
    {
        GameObject go = GetPrefab(id);          //SpawnbyID를 호출 했을 때 내부에서 해당 오브젝트에 id에 맞는 prefab을 찾는다.
        if (go == null)
        {
            return null;
        }

        GameObject obj = Instantiate(go, position, rotation, parent);   // 찾았으면 해당 prefab을 생성한다.
        return obj;
    }

}
