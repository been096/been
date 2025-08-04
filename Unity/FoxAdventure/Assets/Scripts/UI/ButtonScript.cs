using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SceneManagement;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject cubeObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        Instantiate(cubeObject, Vector3.zero, Quaternion.identity);
    }
}
