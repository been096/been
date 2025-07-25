using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI textScore;

    public int score;

    // Start is called before the first frame update
    void Start()
    {
        textScore.text = score.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int itemScore)
    {
        score += itemScore;
        

        textScore.text = score.ToString();
    }
}
