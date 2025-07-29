using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image[] Hearts;
    public int currentHealth;
    public GameObject Gameover;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = Hearts.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        if (currentHealth <= 0) return;
        currentHealth--;
        Hearts[currentHealth].enabled = false;

        if (currentHealth <= 0)
        {
            Debug.Log("Game Over!");// 게임 오버 처리 추가 가능
            Gameover.SetActive(true);
        }
    }

    public void RestoreHealth()
    {
        if (currentHealth >= Hearts.Length) return;

        Hearts[currentHealth].enabled = true;
        currentHealth++;
    }
}
