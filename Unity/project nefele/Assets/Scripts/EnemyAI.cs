using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    // 적의 상태를 정의하는 열거형(Enum)입니다.
    public enum EnemyState { Patrol, Chase, Confused, Attack }

    [Header("Current State")]
    public EnemyState currentState = EnemyState.Patrol;

    [Header("Movement Settings (이동 설정)")]
    [SerializeField] private float patrolSpeed = 2.0f;     // 경계 모드 이동 속도
    [SerializeField] private float chaseSpeed = 4.5f;      // 추적 모드 이동 속도
    [SerializeField] private float patrolDistance = 5.0f;  // 시작 지점으로부터 왔다 갔다 할 거리

    [Header("Vision Settings (시야 설정)")]
    [SerializeField] private float visionRange = 6.0f;     // 시야 거리
    [SerializeField] private float visionAngle = 90.0f;    // 시야각 (위아래 45도씩 총 90도)
    [SerializeField] private LayerMask playerLayer;        // 플레이어 레이어
    [SerializeField] private LayerMask obstacleLayer;      // 벽, 바닥 등 시야를 가리는 레이어
    //[SerializeField] private LayerMask groundLayer;      // 벽, 바닥 등 시야를 가리는 레이어
    [SerializeField] private Vector3 eyeOffset = new Vector3(0, 0.5f, 0); // 눈높이 (레이저 발사 위치)

    [Header("Combat Settings (전투 설정)")]
    [SerializeField] private float attackRange = 1.0f;     // 공격 사거리
    [SerializeField] private float attackCooldown = 1.5f;  // 공격 쿨타임
    private float lastAttackTime;

    [Header("UI & Effect (느낌표/물음표)")]
    [SerializeField] private GameObject exclamationMark;   // ! 아이콘 (추적)
    [SerializeField] private GameObject questionMark;      // ? 아이콘 (혼란)

    // 내부 상태 변수들
    private Rigidbody2D rb;
    private Transform playerTransform;
    private Vector2 startPosition;
    private bool movingRight = true;
    private float confusionEndTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position; // 시작 위치 기억

        rb.freezeRotation = true; // Z축 회전을 고정하여 꼿꼿하게 서 있도록 만듭니다.

        // 시작할 때 느낌표, 물음표는 숨겨둡니다.
        ToggleIcon(null);
    }

    private void Update()
    {
        // 현재 상태에 따라 다른 행동(함수)을 실행합니다.
        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolBehavior();
                CheckPlayerInSight();
                break;

            case EnemyState.Chase:
                ChaseBehavior();
                CheckAttackRange();
                break;

            case EnemyState.Confused:
                ConfusedBehavior();
                break;

            case EnemyState.Attack:
                AttackBehavior();
                break;
        }
    }

    // ==========================================
    // 1. 상태별 행동 (Behaviors)
    // ==========================================

    private void PatrolBehavior()
    {
        // 1. 처음 설정한 이동 한계 지점에 도달했는지 확인
        float leftLimit = startPosition.x - patrolDistance;
        float rightLimit = startPosition.x + patrolDistance;

        bool reachedLimit = (movingRight && transform.position.x >= rightLimit) ||
                            (!movingRight && transform.position.x <= leftLimit);

        // 2. 바로 앞에 장애물(벽)이 있는지 레이저로 확인 (새로 추가된 로직!)
        Vector2 facingDirection = movingRight ? Vector2.right : Vector2.left;
        float wallCheckDistance = 0.6f; // 적의 크기에 맞춰 벽 감지 거리를 조절하세요 (기본 0.6)

        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, facingDirection, wallCheckDistance, obstacleLayer);

        // [핵심 수정!] 앞에 닿은 것이 있더라도, 그것이 연막탄 같은 '트리거(통과 가능)'라면 무시하고 지나갑니다!
        bool hitSolidWall = wallHit.collider != null && !wallHit.collider.isTrigger;

        // 한계 거리에 도달했거나, 앞에 벽(장애물)이 막혀있다면 방향을 반대로 뒤집습니다.
        if (reachedLimit || wallHit.collider != null)
        {
            Flip();
        }

        rb.linearVelocity = new Vector2((movingRight ? 1 : -1) * patrolSpeed, rb.linearVelocity.y);
    }

    private void ChaseBehavior()
    {
        if (playerTransform == null) return;

        // 추적 중에도 눈높이를 기준으로 가려졌는지 확인
        Vector3 myEyePos = transform.position + eyeOffset;
        Vector3 targetEyePos = playerTransform.position + eyeOffset;

        // 1. 플레이어가 아직 시야에 있는지(벽에 숨었는지) 확인합니다.
        // [핵심 수정!] 추적할 때도 장애물 판단을 위해 방향과 거리를 눈(EyePos) 기준으로 계산합니다!
        float distanceToPlayer = Vector2.Distance(myEyePos, targetEyePos);
        Vector2 directionToPlayer = (targetEyePos - myEyePos).normalized;

        // 디버그 팁: 추적 중일 때도 빨간색 레이저를 그려줍니다.
        Debug.DrawRay(myEyePos, directionToPlayer * distanceToPlayer, Color.red);

        // 플레이어가 시야 반경(visionRange) 밖으로 아예 벗어났는가?
        bool isTooFar = distanceToPlayer > visionRange;

        // 적과 플레이어 사이에 시야를 가리는 장애물(obstacleLayer)이 있는가?
        bool isHiddenByWall = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);

        // 만약 너무 멀어졌거나 벽 뒤로 숨었다면?
        if (isTooFar || isHiddenByWall)
        {
            Debug.Log("적: 타겟을 놓쳤다!"); // 확인용 메시지
            playerTransform = null;         // 쫓던 타겟을 잊어버림
            TriggerConfusion(2.0f);         // 2초 동안 '?'를 띄우고 두리번거림 (이후 자동으로 순찰 모드로 돌아감)
            return; // 아래의 이동 코드는 실행하지 않고 함수를 빠져나갑니다.
        }

        // 2. 플레이어가 아직 잘 보인다면 기존처럼 빠르게 쫓아갑니다.
        float directionX = playerTransform.position.x - transform.position.x;

        // [핵심 수정!] 플레이어와 위치가 거의 똑같을 때(겹쳤을 때) 미친듯이 방향을 바꾸며 바보가 되는 것을 막습니다.
        if (Mathf.Abs(directionX) > 0.1f)
        {
            if (directionX > 0 && !movingRight) Flip();
            else if (directionX < 0 && movingRight) Flip();
        }

        // --- [핵심 수정!] 추적 중일 때도 몸 앞에 벽이 있는지 확인합니다 ---
        Vector2 facingDirection = movingRight ? Vector2.right : Vector2.left;
        float wallCheckDistance = 0.6f;
        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, facingDirection, wallCheckDistance, obstacleLayer);
        bool hitSolidWall = wallHit.collider != null && !wallHit.collider.isTrigger;

        if (hitSolidWall)
        {
            // 몸 앞에 벽이 가로막고 있다면, 헛걸음질을 하지 않고 그 자리에 멈춰 서서 위를 쳐다보게 합니다.
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        else
        {
            // 길을 막는 벽이 없다면 정상적으로 쫓아갑니다.
            rb.linearVelocity = new Vector2((movingRight ? 1 : -1) * chaseSpeed, rb.linearVelocity.y);
        }

        rb.linearVelocity = new Vector2((movingRight ? 1 : -1) * chaseSpeed, rb.linearVelocity.y);
    }

    private void ConfusedBehavior()
    {
        // 혼란 상태일 때는 제자리에 멈춰서 두리번거립니다.
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // 정해진 혼란 시간이 끝나면 다시 경계 모드로 돌아갑니다.
        if (Time.time >= confusionEndTime)
        {
            ChangeState(EnemyState.Patrol);
        }
    }

    private void AttackBehavior()
    {
        // 공격 모션 중에는 멈춰있습니다.
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        // 쿨타임이 지났다면 공격 실행
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("적이 플레이어를 공격했습니다!"); // 실제 플레이어 데미지 함수로 교체할 부분
            lastAttackTime = Time.time;

            // 공격 후 플레이어가 멀어졌으면 다시 추적
            ChangeState(EnemyState.Chase);
        }
    }

    // ==========================================
    // 2. 시야 및 감지 시스템 (Vision System)
    // ==========================================

    private void CheckPlayerInSight()
    {
        // 1. 콜라이더 겉면이 시야 반경에 닿았는지 1차 확인
        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, visionRange, playerLayer);

        if (hitPlayer != null)
        {
            Vector3 myEyePos = transform.position + eyeOffset;
            Vector3 targetEyePos = hitPlayer.transform.position + eyeOffset;

            // [추가된 핵심 로직!] 실제 눈과 눈 사이의 거리도 사거리 안쪽인지 2차로 확실하게 검사합니다.
            float distanceToPlayer = Vector2.Distance(myEyePos, targetEyePos);
            if (distanceToPlayer > visionRange) return; // 눈 거리가 멀면 아직 발견하지 못한 것으로 치고 감지 취소!

            Vector2 directionToPlayer = (targetEyePos - myEyePos).normalized;
            Vector2 facingDirection = movingRight ? Vector2.right : Vector2.left;

            float angle = Vector2.Angle(facingDirection, directionToPlayer);
            if (angle < visionAngle / 2f)
            {
                Debug.DrawRay(myEyePos, directionToPlayer * distanceToPlayer, Color.red);

                if (!Physics2D.Raycast(myEyePos, directionToPlayer, distanceToPlayer, obstacleLayer))
                {
                    playerTransform = hitPlayer.transform;
                    ChangeState(EnemyState.Chase);
                }
            }
        }
    }

    private void CheckAttackRange()
    {
        if (playerTransform == null) return;

        // 플레이어가 사거리 안에 들어오면 공격 상태로 전환
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if (distance <= attackRange)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                ChangeState(EnemyState.Attack);
            }
        }
    }

    // ==========================================
    // 3. 외부 호출 함수 (확장성)
    // ==========================================

    // 플레이어가 근처에 왔을 때, 지금 암살 가능한 상태인지 물어보는 함수입니다.
    public bool CanBeAssassinated(Transform playerTransform)
    {
        bool isAttackerOnLeft = playerTransform.position.x < transform.position.x;
        bool isBackAttack = (movingRight && isAttackerOnLeft) || (!movingRight && !isAttackerOnLeft);

        // 등 뒤에 있고, 적이 순찰 중이거나 혼란 상태일 때만 true(가능)를 반환합니다.
        return isBackAttack && (currentState == EnemyState.Confused || currentState == EnemyState.Patrol);
    }

    // 플레이어의 연막탄, 스킬 등이 이 함수를 호출하여 적을 혼란에 빠뜨립니다.
    public void TriggerConfusion(float duration)
    {
        confusionEndTime = Time.time + duration;
        ChangeState(EnemyState.Confused);
    }

    // 플레이어가 적을 공격했을 때 호출되는 함수
    public void TakeDamage(Transform attackerTransform)
    {
        // 1. 플레이어가 적의 왼쪽/오른쪽 중 어디에 있는지 판별
        bool isAttackerOnLeft = attackerTransform.position.x < transform.position.x;

        // 2. 적이 보는 방향과 플레이어의 위치를 비교해 '등 뒤'인지 계산
        bool isBackAttack = (movingRight && isAttackerOnLeft) || (!movingRight && !isAttackerOnLeft);

        // 3. 백어택이면서, 적이 무방비 상태(혼란 또는 순찰)일 때 즉사 판정!
        // (기존엔 Confused만 있었지만, 순찰 중인 적 뒤로 몰래 다가가도 암살할 수 있게 Patrol 조건을 추가하는 게 좋습니다)
        if (isBackAttack && (currentState == EnemyState.Confused || currentState == EnemyState.Patrol))
        {
            Debug.Log("백어택 성공! 푹악! 적 즉사!");
            Die();
        }
        else
        {
            Debug.Log("정면 공격! (아직 일반 체력 시스템이 없어 죽지 않습니다)");
        }
    }

    private void Die()
    {
        // 처치 시 처리 (애니메이션, 오브젝트 삭제 등)
        Destroy(gameObject);
    }

    // ==========================================
    // 4. 유틸리티 (상태 변경, 방향 전환 등)
    // ==========================================

    private void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        // 상태가 바뀔 때 UI 업데이트
        if (currentState == EnemyState.Chase) ToggleIcon(exclamationMark);
        else if (currentState == EnemyState.Confused) ToggleIcon(questionMark);
        else ToggleIcon(null);
    }

    private void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void ToggleIcon(GameObject iconToShow)
    {
        if (exclamationMark != null) exclamationMark.SetActive(exclamationMark == iconToShow);
        if (questionMark != null) questionMark.SetActive(questionMark == iconToShow);
    }

    // 유니티 씬(Scene) 뷰에서 적의 시야를 시각적으로 확인하기 위한 기즈모
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange); // 최대 시야 거리 원형

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // 공격 거리 원형

        // 시야각 선 그리기
        Vector3 facingDir = movingRight ? Vector3.right : Vector3.left;
        Vector3 upRay = Quaternion.Euler(0, 0, visionAngle / 2) * facingDir;
        Vector3 downRay = Quaternion.Euler(0, 0, -visionAngle / 2) * facingDir;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, upRay * visionRange);
        Gizmos.DrawRay(transform.position, downRay * visionRange);

        // 벽 감지 레이저 그리기 (초록색 선)
        Vector3 faceDir = movingRight ? Vector3.right : Vector3.left;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, faceDir * 0.6f);
    }
}