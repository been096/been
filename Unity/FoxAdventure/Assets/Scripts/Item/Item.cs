using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//Item.cs를 새로 만들어주니 아이템에 새로운 속성을 부여하기가 굉장히 쉬워진다.

public class Item : MonoBehaviour
{
    //public int score;
    public ItemData ItemData;
    public AudioSource audioSource;
    public AudioClip getItem;
    //private ItemData currentItem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)

    {
        
       Debug.Log($"OnCollisionEnter2D:{collision.gameObject.name}");
        if (collision.gameObject.tag == "Player")
        {
            ScoreUI playerScore = GameObject.FindAnyObjectByType<ScoreUI>();
           
            playerScore.UpdateScore(ItemData.Score);

            // collision.GetComponent<Dynamic>().score += this.score;

            //Destroy(collision.gameObject);
            audioSource.PlayOneShot(getItem);
            Destroy(this.gameObject);
        }
    }
}
