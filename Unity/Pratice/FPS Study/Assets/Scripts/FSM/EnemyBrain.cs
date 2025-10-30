using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.Rendering;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

/// <summary>
/// 적 FSM의 컨텍스트이자 전환기.
/// - 공용 데이터(컴포넌트/파라미터/런타임 캐시)를 보관한다.
/// - 현재 상태의 Enter/Update/Exit를 호출한다.
/// - 상태 전환 요청을 안전하게 수행한다.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class EnemyBrain : MonoBehaviour
{
    [Header("Modules")]
    public EnemySenses senses;                  // 시야 모듈(거리/각/가림)
    public Health health;                       // 체력(사망 트리거)
    public Transform player;                    // 추격/공격 대상 Transform
    public CharacterController controller;      // 평지 이동용 CC

    [Header("Movement (Flat)")]
    public float chaseSpeed = 3.0f;             // 추격 속도(m/s)
    public float searchSpeed = 1.8f;            // 수색 속도(m/s)
    public float rotateSpeed = 12.0f;           // 회전 보간 속도.
    public float stoppingDistance = 1.2f;       // 멈춤 거리.
    public float gravity = -20.0f;              // 중력 가속도(음수)

    [Header("Attack")]
    public float attackRange = 1.8f;            // 공격 사거리.
    public float attackDamage = 10.0f;          // 공격 데미지.
    public float attackCooldown = 1.2f;         // 공격 간격(초)

    [Header("Search")]
    public float searchDuration = 3.0f;         // 수색 시간(초)

    [Header("Debug")]
    public bool drawForward = false;            // 전방 디버그 레이.

    //====== 런타임 캐시(상태간 공유) ===============================================
    [HideInInspector] public Vector3 lastKnownPos; // 마지막으로 본 플레이어 좌표.
    [HideInInspector] public float attackTimer;     // 공격 쿨다운 남은 시간.
    [HideInInspector] public float searchTimer;     // 수색 남은 시간.

    //===== 상태 인스턴스 ==========================================================
    private EnemyState currentState;            // 현재 상태.
    private IdleState idle;                     // Idle 상태 인스턴스.
    private ChaseState chase;                   // Chase 상태 인스턴스.
    //private AttackState attack;                 // attack 상태 인스턴스.
    //private SearchState search;                 // search 상태 인스턴스.
    private DeadState dead;                     // dead 상태 인스턴스.

    private void Awake()
    {
        // 필수 컴포넌트 자동 참조 보완.
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

        // 상태 인스턴스 생성 및 컨텍스트 주입.
        idle = new IdleState(this);
        chase = new ChaseState(this);
        //attack = new AttackState(this);
        //search = new SearchState(this);
        dead = new DeadState(this);

        // 초기 런타임 캐시값 세팅.
        lastKnownPos = transform.position;
        attackTimer = 0.0f;
        searchTimer = 0.0f;

        // 최초 상태 진입 : Idle
        RequestStateChange(new IdleState(this));
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        // 사망 체크는 브레인에서 일괄로 감시(상태 무시)
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

        // 전방 디버그(옵션)
        if (drawForward == true)
        {
            Debug.DrawRay(transform.position + Vector3.up * 1.0f, transform.forward * 1.5f, Color.yellow, 0.02f);
        }

        // 현재 상태 업데이트.
        if (currentState != null)
        {
            currentState.OnUpdate(dt);
        }
    }

    /// <summary>
    /// 안전한 상태 전환. Exit -> 상태 교체 -> Enter 순서로 호출한다.
    /// null 전달은 무시한다.
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

    // ===== 공용 유틸(상태에서 호출) =============================================

    /// <summary>
    /// 평지 전체에서 목표점을 향해 회전한다(수평 보정).
    /// </summary>
    public void FacePosition(Vector3 target, float dt)
    {
        Vector3 flatTarget = target;
        flatTarget.y = transform.position.y;

        Vector3 to = flatTarget - transform.position;   // 수평 방향.
        to.y = 0.0f;

        if (to.sqrMagnitude > 0.0001f)
        {
            Quaternion look = Quaternion.LookRotation(to.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, dt * rotateSpeed);
        }
    }

    /// <summary>
    /// 평지 전체에서 전진 이동(중력 보정 포함)
    /// </summary>
    public void MoveForward(float speed, float dt)
    {
        Vector3 move = transform.forward * speed;   // 전진 속도.
        move.y = gravity;                           // 경계 안정화용 중력.

        controller.Move(move * dt);
    }

    /// <summary>
    /// 플레이어와의 현재 거리를 반환한다.
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
    /// 현재 상태명 문자열(디버그/HUD용).
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
