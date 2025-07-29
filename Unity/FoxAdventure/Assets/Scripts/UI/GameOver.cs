using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject Gameover;
    public string SceneName;
    // Start is called before the first frame update
    void Start()
    {
        Gameover.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Onclick()
    {
        SceneManager.LoadScene(SceneName);
    }

}
