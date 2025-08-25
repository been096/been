using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AutoAttackController : MonoBehaviour
{
    public WeaponMount weaponMount;
    public Transform attackOrigin; // ������ ������.

    public float attackRange = 1.5f;
    public LayerMask targetLayers; // �� ���̾ Ÿ�����ϱ� ����.

    private float attackTimer;
    private float attackSpeedMultiplier = 1.0f;

    private void Reset()
    {
        if(weaponMount == null)
        {
            weaponMount = GetComponent<WeaponMount>();
        }

        if(attackOrigin == null)
        {
            attackOrigin = transform;
        }
    }

    private void Awake()
    {
        if(attackOrigin == null)
        {
            attackOrigin = transform;
        }

        attackTimer = GetCurrentInterval();
    }

    private float GetCurrentInterval()
    {
        if(weaponMount != null && weaponMount.weapon != null)
        {
            float baseInterval = Mathf.Max(0.05f, weaponMount.weapon.attackInterval);
            float speedMul = Mathf.Max(0.1f, attackSpeedMultiplier);
            return baseInterval / speedMul;
        }

        return 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float interval = GetCurrentInterval();
        if (interval <= 0.0f)
        {
            return;
        }

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0.0f)
        {
            //���� ó��.
            PerformAttack();
            attackTimer = interval;
        }
    }

    private void PerformAttack()
    {
        if(attackOrigin == null)
        {
            attackOrigin = transform;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackOrigin.position, attackRange, targetLayers);

        int damage = GetCurrentDamage();

        foreach(var col in hits) // OverlapCircleAll�� ������ ��� �� ������ŭ ���ڴٴ� �ǹ�
        {
            var damageable = col.GetComponent<IDamageable>(); // Ŭ������ �������� ���� �˾ƺ���.
            if(damageable == null || damageable.IsAlive == false)
            {
                continue;
            }

            Vector3 hitpoint = col.ClosestPoint(attackOrigin.position);

            if (damageable != null)
            {
                damageable.TakeDamage(damage, hitpoint);
            }
            //damageable.TakeDamage(damage, hitpoint);

        }
    }

    private int GetCurrentDamage()
    {
        if(weaponMount != null && weaponMount.weapon != null)
        {
            return Mathf.Max(1, weaponMount.weapon.baseDamage);
        }

        return 1;
    }

    public void AddAttackSpeedMultiplier(float addPercent)
    {
        float m = 1.0f + Mathf.Max(-0.9f, addPercent / 100.0f);
        attackSpeedMultiplier *= m;
    }
}
