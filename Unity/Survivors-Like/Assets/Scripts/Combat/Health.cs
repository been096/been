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

    public bool IsAlive => currentHP > 0; // currentHP�� 0���� ũ�� true ���� ���Եǰ�, 0���� ������ false ���� ���Եȴ�.

    void Awake()
    {
        currentHP = maxHP; // Mathf.clamp -> �ּڰ��� �ִ� ���̸� ���Ѵ�.
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if(OnHPChanged != null) // �̺�Ʈ �Լ��� ����� �Ǿ� ������.
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

        currentHP = Mathf.Max(currentHP - amount, 0); // ���ڰ� 2������ 2�� �߿� ū ���� ���Խ�Ų��. �ִ밪�� 0���� �������� currentHP���� amount(������)�� �޾��� ���� ������ ���� �ʰ� 0���� ������Ų��.
        Debug.Log("���ݹ���!!!!");

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
        OnHPChanged?.Invoke(currentHP, maxHP); // 3���� UI���δ��� �� �̺�Ʈ�� �������.
    }
}
