using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : Fruit
{
    // Start is called before the first frame update
    void Start()
    {
        name = "�ٳ���";
        price = 4000;
        weight = 2;
        Debug.Log("�̸�" + name + "\n����" + price + "��" + "\n����" + weight + "kg");
        SampleMethod("�ٳ���");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
