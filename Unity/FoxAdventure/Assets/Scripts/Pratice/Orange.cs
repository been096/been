using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orange : Fruit
{
    // Start is called before the first frame update
    void Start()
    {
        name = "오렌지";
        price = 5000;
        weight = 3;
        Debug.Log("이름" + name + "\n가격" + price + "원" + "\n무게" + weight + "kg");
        SampleMethod("오렌지");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
