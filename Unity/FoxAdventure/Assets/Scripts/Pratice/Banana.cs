using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : Fruit
{
    // Start is called before the first frame update
    void Start()
    {
        name = "바나나";
        price = 4000;
        weight = 2;
        Debug.Log("이름" + name + "\n가격" + price + "원" + "\n무게" + weight + "kg");
        SampleMethod("바나나");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
