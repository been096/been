using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    public GameObject ghost;

    public float timer;
    public float resettimer = 0.1f;

    private void Awake()
    {
        ghost.SetActive(false);

        timer = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= resettimer)
        {
            ToggleGhost();
            timer = 0f;
        }
        
    }

    void ToggleGhost()
    {
        ghost.SetActive(!ghost.activeSelf);

    }
}
