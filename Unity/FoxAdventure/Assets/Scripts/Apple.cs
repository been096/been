using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : Fruit
{
    // Start is called before the first frame update
    void Start()
    {
        name = "���";
        price = 7000;
        weight = 5;
        Debug.Log("�̸�" + name + "\n����" + price + "��" + "\n����" + weight + "kg");
        SampleMethod("���");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
