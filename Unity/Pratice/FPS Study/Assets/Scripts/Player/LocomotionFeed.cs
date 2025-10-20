using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(CharacterController))]
public class LocomotionFeed : MonoBehaviour
{
    [Header("Ground Check (���� ����)")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundMask;

    CharacterController controller;

    public float HorizontalSpeed { get; private set; }        // ���� �ӵ� ũ��(m/s)
    public Vector3 HorizontalVelocity { get; private set; }   // ���� �ӵ� ����.
    public float VerticalVelocity { get; private set; }      //  ���� �ӵ�
    public bool IsGrounded { get; private set; }             //  ���� ����

    public Vector3 PlayerRight { get { return transform.right; } }
    public Vector3 PlyaerForward { get { return transform.forward; } }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("LocomotionFeed: CharacterController�� �ʿ��մϴ�.");
        }

        if (groundCheck == null)
        {
            GameObject gc = new GameObject("GroundCheck"); // �ڵ� ���� groundCheck.
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
