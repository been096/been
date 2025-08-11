using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Clear: MonoBehaviour
{
    public string SceneName;
    public GameObject GameClear;
    //public GameObject scoreTextObject;
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        GameClear.SetActive(false);
        //scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        SceneManager.LoadScene(SceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            int scoreValue = int.Parse(scoreText.text);
            if(scoreValue >= 7)
            {
                GameClear.SetActive(true);
            }
            
        }
    }
}
