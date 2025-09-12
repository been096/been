using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueTest : MonoBehaviour
{
    Queue<int> queue = new Queue<int>();
    // Start is called before the first frame update
    void Start()
    {
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        queue.Enqueue(4);
        queue.Enqueue(5);

        int data = queue.Dequeue();     //Dequeue -> 꺼내서 확인하고 삭제.
        int data2 = queue.Peek();       //Peek -> 스택과 마찬가지로 꺼내서 확인만 하고 삭제하지 않음.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
