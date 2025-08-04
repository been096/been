using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    protected string name;
    protected float price;
    protected float weight;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void SampleMethod(string name)
    {
        Debug.Log(name + "가 맛있어요");
    }
}
