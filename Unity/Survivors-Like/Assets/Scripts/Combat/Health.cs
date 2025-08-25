using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    public int maxHP = 100;
    public int currentHP;

    public Action<int, int> OnHPChanged;
    public Action<int, Vector3> OnDamaged;
    public Action OnDied;

    public bool IsAlive => currentHP > 0; // currentHP가 0보다 크면 true 값이 대입되고, 0보다 작으면 false 값이 대입된다.

    void Awake()
    {
        currentHP = maxHP; // Mathf.clamp -> 최솟값과 최댓값 사이를 비교한다.
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if(OnHPChanged != null) // 이벤트 함수가 등록이 되어 있으면.
        {
            OnHPChanged.Invoke(currentHP, maxHP);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        if(amount <= 0 || IsAlive == false)
        {
            return;
        }

        currentHP = Mathf.Max(currentHP - amount, 0); // 인자가 2개여서 2개 중에 큰 값을 대입시킨다. 최대값을 0으로 고정시켜 currentHP에서 amount(데미지)를 받았을 때에 음수가 되지 않고 0으로 고정시킨다.
        Debug.Log("공격받음!!!!");

        if(OnHPChanged != null)
        {
            OnHPChanged.Invoke(currentHP, maxHP);
        }
        
        if(OnDamaged != null)
        {
            OnDamaged.Invoke(amount, hitPoint);
        }

        if(currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if(OnDied != null)
        {
            OnDied.Invoke();
        }

        Destroy(gameObject);
    }

    public void Heal(int amount)
    {
        if(amount <= 0 || IsAlive)
        {
            return;
        }

        currentHP = Mathf.Min(currentHP + amount, maxHP);
        OnHPChanged?.Invoke(currentHP, maxHP); // 3일차 UI바인더가 이 이벤트를 듣고있음.
    }
}
