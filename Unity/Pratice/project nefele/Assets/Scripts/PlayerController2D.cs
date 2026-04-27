using UnityEngine;
using UnityEngine.InputSystem;

// Rigidbody2D 컴포넌트가 없으면 자동으로 추가해주는 안전장치입니다.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    // [Header]는 유니티 인스펙터 창에서 변수들을 그룹화하여 보기 좋게 만들어줍니다.
    [Header("Move Settings (이동 설정)")]
    // [SerializeField]는 변수를 private(보안)으로 유지하면서 유니티 창에서만 수정할 수 있게 합니다.
    [SerializeField] private float walkSpeed = 4.5f;      // 평상시 걷기 속도
    [SerializeField] private float sprintSpeed = 7.5f;    // Shift를 눌렀을 때 달리기 속도
    [SerializeField] private float acceleration = 15.0f;  // 속도에 도달하는 가속도 (클수록 빠름)
    [SerializeField] private float deceleration = 20.0f;  // 멈출 때 적용되는 감속도 (클수록 즉시 멈춤)

    [Header("Jump & Gravity (점프 및 중력)")]
    [SerializeField] private float jumpHeight = 1.2f;     // 점프할 높이 (미터 단위)
    [SerializeField] private float gravityScale = 2.0f;   // 기본 중력 세기
    [SerializeField] private float fallGravityScale = 3.0f; // 떨어질 때 더 묵직하게 떨어지도록 하는 중력
    [SerializeField] private float maxFallSpeed = -25.0f;   // 낙하 속도 제한 (지형 뚫기 방지)
    [SerializeField] private float coyoteTime = 0.12f;    // 바닥을 벗어나도 아주 잠깐 동안 점프를 허용하는 시간
    [SerializeField] private float jumpBufferTime = 0.12f; // 바닥에 닿기 직전에 점프를 눌러도 예약되는 시간

    [Header("Combat Settings (전투 설정)")]
    [SerializeField] private float attackRange = 0.8f;   // 플레이어의 공격 사거리
    [SerializeField] private LayerMask enemyLayer;       // 적을 타격하기 위한 레이어

    [Header("Skill Settings (스킬 설정)")]
    [SerializeField] private GameObject smokePrefab;     // 생성할 연막탄 프리팹
    [SerializeField] private float smokeCooldown = 5.0f; // 연막탄 쿨타임 (5초)
    private float lastSmokeTime = -999f;                 // 마지막으로 사용한 시간

    [Header("Ground & Slope (바닥 및 경사로)")]
    [SerializeField] private LayerMask groundMask;        // 무엇을 '바닥'으로 인식할지 결정하는 레이어
    [SerializeField] private Transform groundCheck;       // 바닥 체크를 시작할 위치 (캐릭터 발밑)
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.6f, 0.2f); // 바닥을 감지할 상자의 크기

    [Header("UI & Effect")]
    [SerializeField] private GameObject assassinationPrompt; // 머리 위 F키 아이콘

    [Header("References (참조)")]
    [SerializeField] private Animator animator;           // 애니메이션 제어를 위한 컴포넌트

    private Rigidbody2D rb;                               // 물리 계산을 위한 컴포넌트 변수

    // 내부 계산용 변수들 (인스펙터에 노출할 필요 없음)
    private Vector2 moveInput;               // 플레이어의 입력 방향 (좌/우)
    private bool isSprinting;                 // 현재 달리기 중인지 여부
    private float currentSpeedX;             // 현재 캐릭터의 X축 속도

    private bool isGrounded;                 // 현재 바닥에 닿아 있는지 여부
    private float lastGroundedTime;          // 마지막으로 바닥에 있었던 시간 (코요테 타임용)
    private float lastJumpPressTime;         // 마지막으로 점프 버튼을 누른 시간 (점프 버퍼용)

    private Vector2 groundNormal;            // 닿아 있는 바닥의 수직 벡터 (경사로 계산용)
    private bool isOnSlope;                  // 현재 경사로 위에 있는지 여부

    // 애니메이션 파라미터 이름을 미리 숫자로 변환해 성능을 높입니다.
    private readonly int animMoveHash = Animator.StringToHash("Move");

    private void Awake()
    {
        // 시작할 때 컴포넌트를 가져오고 초기 설정을 합니다.
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = gravityScale;      // 설정한 중력 적용
        rb.freezeRotation = true;            // 물리 충돌로 인해 캐릭터가 회전하지 않도록 고정
    }

    private void Update()
    {
        // Update는 매 프레임 실행되며, 주로 입력과 타이머를 체크합니다.
        float dt = Time.deltaTime;

        UpdateGrounded();                    // 1. 바닥 상태 확인
        UpdateHorizontalSpeed(dt);           // 2. 입력에 따른 속도 계산
        ApplyJumpLogic();                    // 3. 점프 가능 여부 판단
        UpdateAnimator();                    // 4. 애니메이션 업데이트

        CheckAssassinationOpportunity();     // 5. 암살 기회 체크
    }

    private void FixedUpdate()
    {
        // FixedUpdate는 물리 계산(Rigidbody 조작)에 최적화된 주기적인 실행 함수입니다.
        ApplyMovement();                     // 실제 이동 물리 적용
        ApplyCustomGravity();                // 상황에 따른 중력 변화 적용
        ClampFallSpeed();                    // 추락 속도 제한
    }

    // ===== Input (New Input System에서 호출하는 함수들) =====

    public void OnMove(InputAction.CallbackContext ctx)
    {
        // 이동 입력을 받았을 때 (키보드 좌우 화살표 등)
        if (ctx.performed || ctx.canceled)
        {
            moveInput = ctx.ReadValue<Vector2>();
        }
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        // 쉬프트 키 입력을 받았을 때
        if (ctx.performed) isSprinting = true;
        if (ctx.canceled) isSprinting = false;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        // 점프 키(스페이스바) 입력을 받았을 때 현재 시간을 기록합니다.
        if (ctx.performed)
        {
            lastJumpPressTime = Time.time;
        }
    }

    // ===== Grounded & Slope (지형 감지 로직) =====

    private void UpdateGrounded()
    {
        // 발밑에 보이지 않는 상자(BoxCast)를 그려서 groundMask 레이어와 충돌하는지 확인합니다.
        // 레이캐스트보다 상자 방식이 구석진 곳에서 더 안정적입니다.
        RaycastHit2D hit = Physics2D.BoxCast(groundCheck.position, groundCheckSize, 0f, Vector2.down, 0.05f, groundMask);

        if (hit.collider != null)
        {
            isGrounded = true;               // 바닥에 닿음
            lastGroundedTime = Time.time;    // 닿아 있는 동안 계속 현재 시간을 갱신
            groundNormal = hit.normal;       // 바닥의 각도(법선) 정보 저장

            // 법선의 y값이 1보다 작으면 기울어져 있다는 뜻이므로 경사로로 판단합니다.
            isOnSlope = groundNormal.y < 1.0f && groundNormal.y > 0.0f;
        }
        else
        {
            isGrounded = false;              // 공중에 떠 있음
            groundNormal = Vector2.up;       // 기본값은 하늘 방향
            isOnSlope = false;
        }
    }

    // 유니티 에디터 화면에 바닥 감지 영역을 그려주는 함수 (플레이 중에는 안 보임)
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            // 인스펙터에서 설정한 크기만큼 초록색 박스를 보여줍니다.
            Gizmos.DrawWireCube(groundCheck.position + Vector3.down * 0.025f, new Vector3(groundCheckSize.x, groundCheckSize.y, 0));
        }

        // Scene 뷰에서 플레이어의 공격 범위를 빨간색 원으로 확인하기 위한 기즈모
        if (Application.isPlaying)
        {
            Vector2 facingDir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            Vector2 attackPoint = (Vector2)transform.position + facingDir * 0.5f;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint, attackRange);
        }
    }

    // ===== Horizontal Speed (수평 속도 계산) =====

    private void UpdateHorizontalSpeed(float dt)
    {
        // 현재 달리기를 누르고 있는지에 따라 목표 속도를 결정합니다.
        float targetSpeedX = moveInput.x * (isSprinting ? sprintSpeed : walkSpeed);

        // 입력이 있으면 가속도를, 없으면(멈출 때는) 감속도를 사용합니다.
        float accelRate = (Mathf.Abs(targetSpeedX) > 0.01f) ? acceleration : deceleration;

        // currentSpeedX를 목표 속도까지 서서히 변화시켜 부드러운 가감속을 만듭니다.
        currentSpeedX = Mathf.MoveTowards(currentSpeedX, targetSpeedX, accelRate * dt);
    }

    // ===== Jump Logic (코요테 타임 & 점프 버퍼) =====

    private void ApplyJumpLogic()
    {
        // 코요테 타임: 바닥에서 떨어진 지 얼마 안 됐는가?
        bool canCoyoteJump = (Time.time - lastGroundedTime) <= coyoteTime;
        // 점프 버퍼: 점프 키를 누른 지 얼마 안 됐는가?
        bool hasBufferedJump = (Time.time - lastJumpPressTime) <= jumpBufferTime;

        // 두 조건이 모두 만족되면 실제로 점프시킵니다.
        if (hasBufferedJump && canCoyoteJump)
        {
            // 물리 공식: 속도 = 루트(2 * 높이 * 중력)
            float jumpForce = Mathf.Sqrt(jumpHeight * -2.0f * (Physics2D.gravity.y * gravityScale));

            // 기존의 y축 속도를 무시하고 위 방향으로 힘을 줍니다.
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            // 점프를 한 번 썼으니 타이머를 아주 옛날 시간으로 돌려 중복 점프를 막습니다.
            lastJumpPressTime = -999.0f;
            lastGroundedTime = -999.0f;
        }
    }

    // ===== Move Execution (실제 이동 실행) =====

    private void ApplyMovement()
    {
        // 1. 경사로 이동 처리
        if (isOnSlope && isGrounded && !IsJumping())
        {
            // 경사로의 각도에 맞춰 이동 방향 벡터를 회전시킵니다.
            Vector2 tangent = Vector2.Perpendicular(groundNormal).normalized;
            float dir = Mathf.Sign(currentSpeedX);

            // 움직임 입력이 거의 없을 때는 경사로에서 미끄러지지 않게 고정합니다.
            if (Mathf.Abs(currentSpeedX) < 0.01f)
            {
                rb.linearVelocity = new Vector2(0f, 0f);
            }
            else
            {
                // 경사로를 타고 오르내리는 속도를 계산합니다.
                Vector2 slopeMove = new Vector2(-tangent.x * dir, -tangent.y * dir) * Mathf.Abs(currentSpeedX);
                rb.linearVelocity = new Vector2(slopeMove.x, slopeMove.y);
            }
        }
        // 2. 평지 또는 공중 이동 처리
        else
        {
            rb.linearVelocity = new Vector2(currentSpeedX, rb.linearVelocity.y);
        }

        // 캐릭터의 보는 방향 전환 (좌우 반전)
        if (currentSpeedX > 0.01f) transform.localScale = new Vector3(1, 1, 1);       // 오른쪽
        else if (currentSpeedX < -0.01f) transform.localScale = new Vector3(-1, 1, 1); // 왼쪽
    }

    // ===== Custom Gravity (가변 중력) =====

    private void ApplyCustomGravity()
    {
        // 캐릭터가 상승 중일 때보다 낙하 중일 때 더 강한 중력을 주어 
        // 점프가 붕 뜨는 느낌 없이 묵직하고 조작감이 좋게 만듭니다.
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = fallGravityScale;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    private void ClampFallSpeed()
    {
        // 추락 속도가 너무 빨라지면 물리 엔진 오작동이 일어날 수 있으므로 캡(Cap)을 씌웁니다.
        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
        }
    }

    private bool IsJumping()
    {
        // y축 속도가 위를 향하고 있고 바닥에 닿아있지 않다면 점프 상태로 봅니다.
        return rb.linearVelocity.y > 0.1f && !isGrounded;
    }

    // ===== Animation (애니메이션 연동) =====

    private void UpdateAnimator()
    {
        if (animator != null)
        {
            // 속도의 절대값이 0.1보다 크면 Move 파라미터를 true로 만듭니다. (걷기 애니메이션 재생)
            animator.SetBool(animMoveHash, Mathf.Abs(currentSpeedX) > 0.1f);
        }
    }

    // ===== Combat (전투) =====

    // Input System에서 'Attack' 키를 누르면 실행될 함수
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        // 키를 꾹 누르고 있을 때 여러 번 실행되는 것을 막기 위해 performed(눌린 순간)만 체크
        if (ctx.performed)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        // 플레이어가 바라보는 방향(localScale.x)을 확인하여 앞쪽으로 공격 범위를 생성합니다.
        Vector2 facingDir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // 플레이어 중심에서 바라보는 방향으로 살짝 앞(0.5f)을 공격 중심으로 잡습니다.
        Vector2 attackPoint = (Vector2)transform.position + facingDir * 0.5f;

        // 원형 범위 안에 있는 Enemy 레이어를 가진 모든 콜라이더를 찾습니다.
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                // 적에게 데미지 함수를 호출하며, '내가 때렸다'고 위치 정보(transform)를 넘겨줍니다.
                enemyAI.TakeDamage(transform);
            }
        }
    }

    private void CheckAssassinationOpportunity()
    {
        // 아이콘이 연결되어 있지 않으면 에러가 나지 않게 빠져나갑니다.
        if (assassinationPrompt == null) return;

        // 플레이어가 바라보는 방향과 공격 위치 계산
        Vector2 facingDir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Vector2 attackPoint = (Vector2)transform.position + facingDir * 0.5f;

        // 공격 범위 안에 적이 있는지 딱 1명만 확인합니다.
        Collider2D hitEnemy = Physics2D.OverlapCircle(attackPoint, attackRange, enemyLayer);

        bool showPrompt = false;

        if (hitEnemy != null)
        {
            EnemyAI enemyAI = hitEnemy.GetComponent<EnemyAI>();
            // 적이 존재하고, 그 적이 '암살 가능한 상태'라고 대답하면 프롬프트를 켤 준비를 합니다.
            if (enemyAI != null && enemyAI.CanBeAssassinated(transform))
            {
                showPrompt = true;
            }
        }

        // 상황에 맞게 아이콘을 켜거나 끕니다.
        assassinationPrompt.SetActive(showPrompt);
    }

    // Input System에서 'Smoke' 키를 누르면 실행될 함수
    public void OnSmoke(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            UseSmoke();
        }
    }

    private void UseSmoke()
    {
        // 쿨타임이 지났는지 확인합니다.
        if (Time.time >= lastSmokeTime + smokeCooldown)
        {
            if (smokePrefab != null)
            {
                // 플레이어의 현재 위치에 연막탄 오브젝트를 생성합니다.
                Instantiate(smokePrefab, transform.position, Quaternion.identity);
                lastSmokeTime = Time.time;
                Debug.Log("연막탄 투척!");
            }
        }
        else
        {
            // 쿨타임이 덜 찼을 때 남은 시간을 알려줍니다.
            float timeLeft = (lastSmokeTime + smokeCooldown) - Time.time;
            Debug.Log($"연막탄 쿨타임 중... {timeLeft:F1}초 남음");
        }
    }
}