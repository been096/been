using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    [SerializeField]private CharacterStatsSO stats;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float baseMoveSpeed;             // �⺻ �ӵ�.
    private float movespeedMultiplier = 1.0f;      //  �ռ� ����.(1 = ��ȭ ����)
    public float currentSpeedMagnitude
    {
        get;
        private set;
    }

    
    private int currentHP;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        baseMoveSpeed = stats ? stats.baseMoveSpeed : 5.0f;
        currentHP = stats.baseMaxHP;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentSpeedMagnitude = rb.velocity.magnitude;
    }

    private void FixedUpdate()
    {
        float effective = baseMoveSpeed * movespeedMultiplier;
        rb.velocity = moveInput * effective;
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    public void AddMoveSpeedMultiplier(float addPercent)
    {
        float m = 1.0f + Mathf.Max(-0.9f, addPercent / 100.0f);
        movespeedMultiplier *= m;
    }

    //PlayerCore.cs �ȿ� ���� ���� �߰�
    public void Heal(int amount)
    {
        Health h = GetComponent<Health>();
        if (h != null)
        {
            h.Heal(amount);
        }
    }
}
