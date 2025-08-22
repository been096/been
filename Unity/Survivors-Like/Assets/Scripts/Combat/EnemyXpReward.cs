using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyXpReward : MonoBehaviour
{
    public int rewardxp = 5;
    public XpSystem playerxp;
    public Health health;
    public GameObject xpPrefab;

    private void OnEnable()
    {
        health.OnDied += HandleDied;
        //playerxp = FindAnyObjectByType<XpSystem>();
    }

    private void OnDisable()
    {
        health.OnDied -= HandleDied;
    }

    private void HandleDied()
    {
        //playerxp?.AddExp(rewardxp);
        GameObject xpOrb = Instantiate(xpPrefab, transform.position, Quaternion.identity);
    }
}
