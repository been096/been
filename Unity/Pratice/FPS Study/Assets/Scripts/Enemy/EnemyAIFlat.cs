using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// NavMash ���� '���� ���� �̵�'������ �߰�/����/������ �����ϴ� ���� FSM(���� �ӽ�).
/// - Idle : ���(�߰� �� Chase ��ȯ)
/// - Chase : ���������� �� ��ġ�� ���� ����(���� ȸ�� ����)
/// - Attack : ��Ÿ� �̳��� �� ��ٿ��� ��Ű�� �÷��̾ ������ �ο�.
/// - Search : �þ� ��� �� ������ ��ġ �ٹ濡�� ��� Ž�� �� Idle ����.
/// - Dead : Health 0 ������ �� ����(���� �ִ�/�ı��� Health.onDeath�� ó�� ����)
/// </summary>
public class EnemyAIFlat : MonoBehaviour
{
    public enum State { Idle, Chase, Attack, Search, Dead }

    [Header("Modules")]
    public EnemySenses senses;              // �þ� ���(�߰�/��� ����)
    public Health health;                   // ü��(��� ���� ���� Ʈ����)
    public Transform player;                // �߰�/���� ��� Transform.
    public PlayerHealth playerDamagealbe;   // ����� IDamagealbe(���� ������ ����)
    public CharacterController controller;  // �ܼ� �̵��� ���� CC(�浹/��� ����ȭ)

    [Header("Movement (Flat)")]
    public float chaseSpeed = 3.0f;         // �߰� �ӵ�(m/s). ���� ���� �ӵ�.
    public float rotateSpeed = 12.0f;       // ȸ�� ���� �ӵ�(�������� ����)
    public float stoppingDistance = 1.2f;   // �� �Ÿ� �̳��� ������ ����(���߰Ÿ� ����)
    public float gravity = -20.0f;          // �̼��� ���� ����ȭ�� ���� �߷� ���ӵ�(����)

    [Header("Attack")]
    public float attackRange = 1.8f;        // ���� �ߵ� ��Ÿ�(����)
    public float attackDamage = 10.0f;      // ���� ��������.
    public float attackCooldown = 1.2f;     // ���� ����(��). 0�̸� ��Ÿ�� �ǹǷ� ���� X.

    [Header("Search")]
    public float searchDuration = 3.0f;     // �þ� ��� �� ������ ��ġ �ٹ濡�� Ž���ϴ� �ð�(��)

    [Header("Debug")]
    public bool drawForward = false;        // ���� ����� ���� ǥ��.

    public StatusEffectHost host;

    // ===== ���� ���� �ʵ�(���� �ּ�) =====================================================================
    private State state;                    // ���� FSM ����.
    private float attackTimer;              // ���� ��ٿ� ī����(0�̸� ����)
    private float searchTimer;              // ���� �ܿ� �ð�.
    private Vector3 lastKnownPos;           // ���������� '�ô�' �÷��̾� ��ǥ(�þ� ��� ���)

    private void Awake()
    {
        // �ʼ� ������Ʈ �ڵ� ���� ����.
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }

        // �ʱ� ����/Ÿ�̸� ����.
        state = State.Idle;
        attackTimer = 0.0f;
        searchTimer = 0.0f;
        lastKnownPos = transform.position;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        // 1) ���� ��ٿ� ����.
        if (attackTimer > 0.0f)
        {
            attackTimer = attackTimer - dt;
            if (attackTimer < 0.0f)
            {
                attackTimer = 0.0f;
            }
        }

        // 2) ��� ���·� ����(Health 0 ����)
        if (health != null)
        {
            if (health.GetCurrentHealth() <= 0.0f)
            {
                if (state != State.Dead)
                {
                    EnterDead();
                }
                return; // Dead�� �� ó������ ����.
            }
        }

        // 3) �þ� üũ : �����ϸ� lastKnownPos ����.
        bool seen = false;
        Vector3 seenPos = Vector3.zero;
        if (senses != null)
        {
            bool can = senses.CanSeeTarget(out seenPos);
            if (can == true)
            {
                seen = true;
                lastKnownPos = seenPos;
            }
        }

        // 4) ���� ������Ʈ.
        switch (state)
        {
            case State.Idle:
                {
                    UpdateIdle(seen);
                }
                break;
            case State.Chase:
                {
                    UpdateChase(seen);
                }
                break;
            case State.Attack:
                {
                    UpdateAttack(seen);
                }
                break;
            case State.Search:
                {
                    UpdateSearch(seen);
                }
                break;
        }
    }

    private void EnterIdle()
    {
        state = State.Idle;
        // ���� ���� : Idle���� �ϴ� ���� ����(�߰� �� �ٷ� Chase)
    }

    private void UpdateIdle(bool seen)
    {
        // �߰� ��� �߰� ����.
        if (seen == true)
        {
            EnterChase();
            return;
        }
        // Idle ����.
    }

    private void EnterChase()
    {
        state = State.Chase;
        // ���� �ʱ�ȭ�� ����.UpdateChase���� ȸ��/���� ó��.
    }

    private void UpdateChase(bool seen)
    {
        // 1) ȸ�� : ����鿡�� ������ ��ǥ�� �ٶ󺸵��� ����.
        Vector3 targetPos = lastKnownPos;
        Vector3 flatTarget = targetPos;
        flatTarget.y = transform.position.y;    // ���� ���� : ���� Y ����ȭ.

        Vector3 to = flatTarget - transform.position;   // �̵�/ȸ�� ���� ����.
        to.y = 0.0f;                                    // ���� ���.

        if (to.sqrMagnitude > 0.0001f)
        {
            Quaternion look = Quaternion.LookRotation(to.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * rotateSpeed);
        }

        // 2) �����Ÿ�/����.
        float dist = Vector3.Distance(transform.position, flatTarget);

        if (dist > stoppingDistance)
        {
            float speedMul = 1.0f;
            if (host != null)
            {
                speedMul = host.GetSpeedMultiplier();
            }
            Vector3 move = transform.forward * (chaseSpeed * speedMul);
            
            // ���� ����(���� ���� ����)
            //Vector3 move = transform.forward * chaseSpeed;

            // CC ����ȭ�� ���� �߷� ����(���������� ��迡�� ������ ����)
            move.y = gravity;

            controller.Move(move * Time.deltaTime);
        }

        // 3) ���� ��Ÿ� ���� -> Attack
        if (player != null)
        {
            float d2 = Vector3.Distance(transform.position, player.position);
            if (d2 <= attackRange)
            {
                EnterAttack();
                return;
            }
        }

        // 4) �þ� ��� ���¿��� ������ ��ǥ ���� -> Search
        if (seen == false)
        {
            if (dist <= stoppingDistance)
            {
                EnterSearch();
                return;
            }
        }
    }

    private void EnterAttack()
    {
        state = State.Attack;
        // ���� ���¿����� ����/ȸ��/��ٿ� ��� ������ ����.
    }

    private void UpdateAttack(bool seen)
    {
        //// 0) ���� ���̸� ����.
        //if (host != null)
        //{
        //    if (host.IsStunned() == true)
        //    {
        //        // �̵�/����/���콺�Է� ó�� �ߴ�.
        //        return;
        //    }
        //}

        // 1) ��Ÿ� ������ ������ Chase�� ����.
        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist > attackRange)
            {
                EnterChase();
                return;
            }
        }


        // 2) �þ߰� ������ ����� Search��.
        if (seen == false)
        {
            EnterSearch();
            return;
        }

        // 3) ���� ����(��ٿ�)
        if (attackTimer <= 0.0f)
        {
            DoAttack();
            attackTimer = attackCooldown;
        }

        // 4) ���� �߿��� õõ�� �÷��̾ ���� ȸ��(�ð��� �ڿ�������)
        if (player != null)
        {
            Vector3 dir = player.position - transform.position;
            dir.y = 0.0f;
            if (dir.sqrMagnitude > 0.0001f)
            {
                Quaternion look = Quaternion.LookRotation(dir.normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * rotateSpeed);
            }
        }
    }

    private void EnterSearch()
    {
        state = State.Search;
        searchTimer = searchDuration;
    }

    private void UpdateSearch(bool seen)
    {
        // 1) ������ ��ġ�� ���� õõ�� ����(ȸ�� + ���� �ӵ� ����)
        Vector3 flatTarget = lastKnownPos;
        flatTarget.y = transform.position.y;

        Vector3 to = flatTarget - transform.position;
        to.y = 0.0f;

        float dist = to.magnitude;
        
        if (dist > stoppingDistance)
        {
            Quaternion look = Quaternion.LookRotation(to.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, Time.deltaTime * rotateSpeed);

            Vector3 move = transform.forward * chaseSpeed * 0.6f;   // ������ �߰ݺ��� ������.
            move.y = gravity;
            controller.Move(move * Time.deltaTime);
        }

        // 2) �ٽ� ���̸� ��� Chase��.
        if (seen == true)
        {
            EnterChase();
            return;
        }

        // 3) Ÿ�Ӿƿ� �� Idle ����.
        searchTimer = searchTimer - Time.deltaTime;
        if (searchTimer <= 0.0f)
        {
            EnterIdle();
        }
    }

    private void EnterDead()
    {
        state = State.Dead;
        // ���⼭�� ������. ���� �ı�/���/�ִϴ� Health.onDeath���� ó�� ����.
    }

    /// <summary>
    /// ���� �������� �÷��̾�(IDamageable)�� ����.
    /// </summary>
    private void DoAttack()
    {
        if (playerDamagealbe == null)
        {
            return;
        }

        // ����� ���� : hitPoint�� �÷��̾� ��ġ, ������ Vector.up ���.
        Vector3 hp = player.position;
        Vector3 n = Vector3.up;

        playerDamagealbe.ApplyDamage(attackDamage, hp, n, transform);
    }
}
