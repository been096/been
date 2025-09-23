using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HomingAndSplitTest : MonoBehaviour
{
    public GameObject homingPrefab;
    public GameObject splitPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) == true)
        {
            GameObject go = Instantiate(homingPrefab, transform.position, Quaternion.identity);
            BulletHoming hb = go.GetComponent<BulletHoming>();
            hb.Fire(transform.position, Vector2.right);
        }

        if (Input.GetKeyDown(KeyCode.T) == true)
        {
            GameObject go = Instantiate(splitPrefab, transform.position, Quaternion.identity);
            BulletSplit sb = go.GetComponent<BulletSplit>();
            sb.Fire(transform.position, Vector2.right);
        }
    }
}
