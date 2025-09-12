using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StackTest : MonoBehaviour
{
    Stack<int> stacks = new Stack<int>();

    // Start is called before the first frame update
    void Start()
    {
        stacks.Push(0);
        stacks.Push(1);
        stacks.Push(2);
        stacks.Push(3);
        stacks.Push(4);
        Debug.Log("data count = " + stacks.Count);
        stacks.Pop();       //���� �������� ���� �����͸� ������ �����ϴ� ������
        stacks.Peek();      //Pop�̶� �ٸ��� �������� �ʰ� �����⸸ ��.
        Debug.Log("data count = " + stacks.Count);
        foreach ( int i in stacks)
        {
            Debug.Log(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
