using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Numbers : MonoBehaviour
{
    public int[] numbers;
    // Start is called before the first frame update
    void Start()
    {
        numbers = new int[100];
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = i + 1;
            Debug.Log("³Ñ¹ö °ª : " + numbers[i]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
