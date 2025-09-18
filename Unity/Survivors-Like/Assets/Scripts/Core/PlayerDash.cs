using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashSpeed = 10.0f;
    public float dashDurationSeconds = 0.2f;
    public float dashCooldownSeconds = 1.0f;
    public PlayerMoveSimple playerMoveSimple;
    public SpriteRenderer spriteRenderer;
    public TrailRenderer trail;
    //public Rigidbody2D rigidbody2D;

    public bool isDashing = false;
    private float dashEndTime = 0.0f;
    private float nexAvailalbeDashTime = 0.0f;

    private Vector2 lastMoveDir = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
        trail.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        if (move.sqrMagnitude > 0.0001f)
        {
            lastMoveDir = move.normalized;
        }
        else
        {
            if (spriteRenderer.flipX == true)
            {
                lastMoveDir = Vector2.left;
            }
            else
            {
                lastMoveDir = Vector2.right;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            if (Time.time >= nexAvailalbeDashTime)
            {
                StartDash();
            }
        }

        Dashing();
    }

    void StartDash()
    {
        isDashing = true;
        dashEndTime = Time.time + dashDurationSeconds;
        nexAvailalbeDashTime = Time.time + dashCooldownSeconds;

        playerMoveSimple.enabled = false;
        trail.emitting = true;
    }

    void Dashing()
    {
        if (isDashing == true)
        {
            float delta = Time.deltaTime;
            ///---------------- ������ٵ� �̿��ϴ� ���-----------------------------
            //Vector2 curPos = rigidbody2D.position + lastMoveDir * dashSpeed * delta;
            //Vector2 newPos = curPos + lastMoveDir * dashSpeed * delta;


            //rigidbody2D.MovePosition(newPos);

            ///----------------------------------------------------------------------

            Vector2 curPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 newPos = curPos + lastMoveDir * dashSpeed * delta;

            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

            if (Time.time >= dashEndTime)
            {
                EndDash();
            }
        }
    }

    void EndDash()
    {
        isDashing = false;
        playerMoveSimple.enabled = true;
        trail.emitting = false;
    }

    public bool GetIsDasing()
    {
        return isDashing;
    }
}
