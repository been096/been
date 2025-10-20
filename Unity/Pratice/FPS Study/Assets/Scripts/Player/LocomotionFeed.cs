using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController))]
public class LocomotionFeed : MonoBehaviour
{
    [Header("Ground Check (접지 판정)")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundMask;

    CharacterController controller;

    public float HorizontalSpeed { get; private set; }        // 수평 속도 크기(m/s)
    public Vector3 HorizontalVelocity { get; private set; }   // 수평 속도 벡터.
    public float VerticalVelocity { get; private set; }      //  수직 속도
    public bool IsGrounded { get; private set; }             //  접지 여부

    public Vector3 PlayerRight { get { return transform.right; } }
    public Vector3 PlyaerForward { get { return transform.forward; } }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("LocomotionFeed: CharacterController가 필요합니다.");
        }

        if (groundCheck == null)
        {
            GameObject gc = new GameObject("GroundCheck"); // 자동 생성 groundCheck.
            gc.transform.SetParent(transform);
            gc.transform.localPosition = new Vector3(0f, 0.1f, 0f);
            groundCheck = gc.transform;
        }
    }

    private void Update()
    {
        Vector3 vel = controller.velocity;
        Vector3 horizontal = vel;
        horizontal.y = 0f;

        HorizontalVelocity = horizontal;
        HorizontalSpeed = horizontal.magnitude;
        VerticalVelocity = vel.y;

        bool grounded = false;
        
        if (groundCheck != null)
        {
            Collider[] hits = Physics.OverlapSphere(
                groundCheck.position,
                groundCheckRadius,
                groundMask,
                QueryTriggerInteraction.Ignore);

            if (hits != null)
            {
                if (hits.Length > 0)
                {
                    grounded = true;
                }
            }

        }

        IsGrounded = grounded;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
