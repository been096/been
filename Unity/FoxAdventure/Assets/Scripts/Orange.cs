using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orange : Fruit
{
    // Start is called before the first frame update
    void Start()
    {
        name = "������";
        price = 5000;
        weight = 3;
        Debug.Log("�̸�" + name + "\n����" + price + "��" + "\n����" + weight + "kg");
        SampleMethod("������");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
