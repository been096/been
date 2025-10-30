using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.Rendering;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

/// <summary>
/// �� FSM�� ���ؽ�Ʈ���� ��ȯ��.
/// - ���� ������(������Ʈ/�Ķ����/��Ÿ�� ĳ��)�� �����Ѵ�.
/// - ���� ������ Enter/Update/Exit�� ȣ���Ѵ�.
/// - ���� ��ȯ ��û�� �����ϰ� �����Ѵ�.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class EnemyBrain : MonoBehaviour
{
    [Header("Modules")]
    public EnemySenses senses;                  // �þ� ���(�Ÿ�/��/����)
    public Health health;                       // ü��(��� Ʈ����)
    public Transform player;                    // �߰�/���� ��� Transform
    public CharacterController controller;      // ���� �̵��� CC

    [Header("Movement (Flat)")]
    public float chaseSpeed = 3.0f;             // �߰� �ӵ�(m/s)
    public float searchSpeed = 1.8f;            // ���� �ӵ�(m/s)
    public float rotateSpeed = 12.0f;           // ȸ�� ���� �ӵ�.
    public float stoppingDistance = 1.2f;       // ���� �Ÿ�.
    public float gravity = -20.0f;              // �߷� ���ӵ�(����)

    [Header("Attack")]
    public float attackRange = 1.8f;            // ���� ��Ÿ�.
    public float attackDamage = 10.0f;          // ���� ������.
    public float attackCooldown = 1.2f;         // ���� ����(��)

    [Header("Search")]
    public float searchDuration = 3.0f;         // ���� �ð�(��)

    [Header("Debug")]
    public bool drawForward = false;            // ���� ����� ����.

    //====== ��Ÿ�� ĳ��(���°� ����) ===============================================
    [HideInInspector] public Vector3 lastKnownPos; // ���������� �� �÷��̾� ��ǥ.
    [HideInInspector] public float attackTimer;     // ���� ��ٿ� ���� �ð�.
    [HideInInspector] public float searchTimer;     // ���� ���� �ð�.

    //===== ���� �ν��Ͻ� ==========================================================
    private EnemyState currentState;            // ���� ����.
    private IdleState idle;                     // Idle ���� �ν��Ͻ�.
    private ChaseState chase;                   // Chase ���� �ν��Ͻ�.
    //private AttackState attack;                 // attack ���� �ν��Ͻ�.
    //private SearchState search;                 // search ���� �ν��Ͻ�.
    private DeadState dead;                     // dead ���� �ν��Ͻ�.

    private void Awake()
    {
        // �ʼ� ������Ʈ �ڵ� ���� ����.
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }
        if (health == null)
        {
            health = GetComponent<Health>();
        }
        if (player == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null)
            {
                player = go.transform;
            }
        }

        // ���� �ν��Ͻ� ���� �� ���ؽ�Ʈ ����.
        idle = new IdleState(this);
        chase = new ChaseState(this);
        //attack = new AttackState(this);
        //search = new SearchState(this);
        dead = new DeadState(this);

        // �ʱ� ��Ÿ�� ĳ�ð� ����.
        lastKnownPos = transform.position;
        attackTimer = 0.0f;
        searchTimer = 0.0f;

        // ���� ���� ���� : Idle
        RequestStateChange(new IdleState(this));
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        // ��� üũ�� �극�ο��� �ϰ��� ����(���� ����)
        if (health != null)
        {
            if (health.GetCurrentHealth() <= 0.0f)
            {
                if (currentState != dead)
                {
                    RequestStateChange(dead);
                }
                return;
            }
        }

        // ���� �����(�ɼ�)
        if (drawForward == true)
        {
            Debug.DrawRay(transform.position + Vector3.up * 1.0f, transform.forward * 1.5f, Color.yellow, 0.02f);
        }

        // ���� ���� ������Ʈ.
        if (currentState != null)
        {
            currentState.OnUpdate(dt);
        }
    }

    /// <summary>
    /// ������ ���� ��ȯ. Exit -> ���� ��ü -> Enter ������ ȣ���Ѵ�.
    /// null ������ �����Ѵ�.
    /// </summary>
    public void RequestStateChange(EnemyState next)
    {
        if (next == null)
        {
            return;
        }

        if (currentState != null)
        {
            currentState.OnExit();
        }

        currentState = next;
        currentState.OnEnter();
    }

    // ===== ���� ��ƿ(���¿��� ȣ��) =============================================

    /// <summary>
    /// ���� ��ü���� ��ǥ���� ���� ȸ���Ѵ�(���� ����).
    /// </summary>
    public void FacePosition(Vector3 target, float dt)
    {
        Vector3 flatTarget = target;
        flatTarget.y = transform.position.y;

        Vector3 to = flatTarget - transform.position;   // ���� ����.
        to.y = 0.0f;

        if (to.sqrMagnitude > 0.0001f)
        {
            Quaternion look = Quaternion.LookRotation(to.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, dt * rotateSpeed);
        }
    }

    /// <summary>
    /// ���� ��ü���� ���� �̵�(�߷� ���� ����)
    /// </summary>
    public void MoveForward(float speed, float dt)
    {
        Vector3 move = transform.forward * speed;   // ���� �ӵ�.
        move.y = gravity;                           // ��� ����ȭ�� �߷�.

        controller.Move(move * dt);
    }

    /// <summary>
    /// �÷��̾���� ���� �Ÿ��� ��ȯ�Ѵ�.
    /// </summary>
    public float DistanceToPlayer()
    {
        if (player == null)
        {
            return float.PositiveInfinity;
        }
        float d = Vector3.Distance(transform.position, player.position);
        return d;
    }

    /// <summary>
    /// ���� ���¸� ���ڿ�(�����/HUD��).
    /// </summary>
    public string GetCurrentStateName()
    {
        if (currentState == null)
        {
            return "None";
        }
        string n = currentState.Name();
        return n;
    }
}
