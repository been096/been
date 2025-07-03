using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Responner : MonoBehaviour
{
    public GameObject prefabPlayer;
    public GameObject objPlayer;
    public bool IsAlive;
    public float time;
    public int Heart;

    IEnumerator ProcessTimer()
    {
        Debug.Log("ProcessTimer() start");
        yield return new WaitForSeconds(time);
        objPlayer = Instantiate(prefabPlayer, transform.position, Quaternion.identity);
        Debug.Log("ProcessTimer() end");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(objPlayer == null && IsAlive == false && Heart > 0)
        {
            IsAlive = true;
            Heart--;
            StartCoroutine(ProcessTimer());
            
        }

        if (objPlayer != null && IsAlive == true)
            IsAlive = false;
    }
}
