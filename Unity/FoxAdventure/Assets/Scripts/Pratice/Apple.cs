using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : Fruit
{
    // Start is called before the first frame update
    void Start()
    {
        name = "사과";
        price = 7000;
        weight = 5;
        Debug.Log("이름" + name + "\n가격" + price + "원" + "\n무게" + weight + "kg");
        SampleMethod("사과");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
