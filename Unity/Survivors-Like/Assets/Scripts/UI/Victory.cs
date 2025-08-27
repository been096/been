using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public float timer;
    public float survivetiemr = 60.0f;

    public int kills = 0;
    //public int gold = 0;

    public GameObject Result;

    public TextMeshProUGUI resultmessage;
    public TextMeshProUGUI resulttime;
    public TextMeshProUGUI resultkill;
    public TextMeshProUGUI resultgold;

    public GoldSystem gold;

    private void Awake()
    {
        gold = FindAnyObjectByType<GoldSystem>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Result.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= survivetiemr)
        {
            Time.timeScale = 0.0f;
            Result.SetActive(true);
            resultmessage.text = "VICTORY!!";
            resulttime.text = "Survived Time : " + timer.ToString("F1") + "s";
            resultgold.text = "Gold : " + gold.currentGold.ToString();
            resultkill.text = "Kills : " + kills.ToString();

        }
    }

    public void AddKill()
    {
        kills++;
    }

    //public void AddGold(int amount)
    //{
    //    gold += amount;
    //}

    public void Onclick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
