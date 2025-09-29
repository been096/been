using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public int damage = 5;
    public float hitCooldownSec = 0.2f;

    Dictionary<Transform, float> nextTickTime = new Dictionary<Transform, float>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            TryHit(collision.transform);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            TryHit(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            if (nextTickTime.ContainsKey(collision.transform) == true)
            {
                nextTickTime.Remove(collision.transform);
            }
        }
    }

    void TryHit(Transform Player)
    {
        float now = Time.time;

        if (nextTickTime.ContainsKey(Player) == false)
        {
            nextTickTime.Add(Player, 0.0f);
        }

        if (now >= nextTickTime[Player])
        {
            Health hp = Player.GetComponent<Health>();
            if (hp != null)
            {
                hp.TakeDamage(damage, hp.transform.position);
            }

            nextTickTime[Player] = now + hitCooldownSec;
        }
    }
}
