using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallTest : MonoBehaviour
{
    GameObject obja;
    GameObject objb;

    // Start is called before the first frame update
    void Start()
    {
        int[] a = new int[100];
        int[] b = new int[101];
        for(int i=0; i<a.Length; ++i)
        {
            b[i] = a[i];
        }

        a = b;

        List<int> listA = new List<int>();
        listA.Add(1); // ������ �߰�
        listA.Add(2);
        listA.Add(3);
        listA.Add(4);
        listA.Add(5);
        listA.Add(6);
        listA.Add(7);
        listA.Add(8);
        listA.Remove(3);   // ������ ����
        listA.RemoveAt(1); // ������ ������ �ִ� �����͸� ����
        listA.Clear();    //  ����Ʈ ��ü ����.
        listA.Insert(3, 10);  // ������ ������ ������ ����.
        int index = listA.IndexOf(6);       // ������ �����Ͱ� ��� ° ������ �ִ��� ��ȯ.
        bool result = listA.Contains(7);  // ������ �����Ͱ� ����Ʈ�� ���ԵǾ��ִ��� ���θ� ��ȯ.
    }

    void Function(ref int a,ref int b)
    {
        a = 30;
        b = 40;
    }

    void Add(out int a)
    {
        a = 1 + 2;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
