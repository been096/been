using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    public GameObject vol;
    public GameObject shop;
    // Start is called before the first frame update
    void Start()
    {
        vol.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Onclick()
    {
        vol.SetActive(true);
    }
}
