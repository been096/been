using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//Item.cs�� ���� ������ִ� �����ۿ� ���ο� �Ӽ��� �ο��ϱⰡ ������ ��������.

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
