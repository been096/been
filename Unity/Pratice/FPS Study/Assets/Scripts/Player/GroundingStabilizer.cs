using UnityEngine;

/// <summary>
/// ��� ����(ProjectOnGround), ���� ����(ApplystepSmoothing), ���� �ٿ�(TrySnapDown)�� �����ϴ� ����ȭ ���.
/// - ���� : CharacterController �̵����� ��� / ��� / ���� ��Ȳ�� '���� / �� / �� ��'�� �ٿ� ü�� ǰ���� ����.
/// - �ٽ� ������ġ :
/// 1) �����̰ų�(���� X) ���� ����Ʈ�� ������ ���� ����(�߸��� �������� �̵� ��� ����)
/// 2) ������ ��(�ǹ� ���� ���� ����) ��� ������ ���� ����
/// 3) ���� �� ���� �ս��� �����ϸ�(������ ���� ���� �̸�) ���� �̵� ���͸� ����
/// 4) ���� ������ "���� ���� ��Ʈ + ���� ���� ����Ʈ"�� ���� ����
/// 5) ���� �ٿ��� �����(y > 0)���� �������� ����(���� ��� ����)
/// </summary>
public class GroundingStabilizer : MonoBehaviour
{
    [Header("Ground Probe")]
    public float probeDistance = 0.6f;              // �ٴ� Ž��� ���� ����(�÷��̾� �ߺ��� �ణ �� ��)
    public LayerMask groundMask;                    // �ٴ����� ������ ���̾� ����ũ.

    [Header("Slope Projection")]
    public float slopeStickAngleBias = 2.0f;        // CC.slopeLimit���� �ణ ���� ��� ������(�̲��� ������ ����)
    public float minMoveSpeedForProjection = 0.4f;  // �̵� �ӵ��� �� �� �̸��̸� �������� ����(���� ��ó ���� ����)
    public float maxProjectionLossRatio = 0.5f;     // ���� �� ���� ���̰� ������ 50% �̸��̸� ���� ����(���� ����)

    [Header("Step Smoothing")]
    public float stepUpSmoothTime = 0.06f;          // ��� ��� ���� �ð�(ª�� �ε巴��)
    public float maxStepHeight = 0.4f;              // �������� ��½�ų �� �ִ� �ִ� ����(����� ����)
    public float stepProbeFoward = 0.25f;           // ���� Ž�� �Ÿ�(�ʹ� ��� �պ� ����)

    [Header("Snap Down")]
    public float snapDownDistance = 0.35f;          // ��
    public float snapDownSpeed = 20.0f;

    // ���� ����(���� ������)
    private float stepUpVelocity;
    private float currentStepOffset;
    
    /// <summary>
    /// �̵� ���͸� ���� �ٴ� ���� �������� '��� ����'�Ͽ� ��縦 ���󰡰� �����.
    /// </summary>
    /// <param name="move"></param>
    /// <param name="cc"></param>
    /// <param name="pc"></param>
    /// <returns></returns>
    public Vector3 ProjectOnGround(Vector3 move, CharacterController cc, PlayerController pc)
    {
        // �ʼ� ���۷��� Ȯ��.
        if (cc == null)
        {
            return move; // CC�� ������ ���� �Ұ� -> ���� ����.
        }

        // ���� ���� �̵� �ӵ� ����(������ �ǹ� �ִ��� �Ǵ�.)
        float dt = Time.deltaTime; // ������ �ð�(��)
        float speedPlaner = 0f;    // ���� ���� �ӵ�(m/s)
        if (dt > 0f)
        {
            // move�� "�̹� ������ �̵���"�� �ƴ϶� "�ʴ� �ӵ�"�� �����ϰ� ���.
            speedPlaner = new Vector2(move.x, move.z).magnitude; // ���� ������ ũ��.
        }

        // ���� ���� �Ǵ�( ���� �ƴ� ���� ��� ���� ����)
        bool grounded = false;
        if (pc != null)
        {
            // PlayerController�� ���� ���� ���� ������ ���ٸ�,
            // �����ϰ� CC ��Ģ�� �����ϰų� ���⼭�� ���������� true/false ��å ����.
            // �ǽ����� CC.isGrounded�� �����ص� OK.
            grounded = cc.isGrounded;
        }
        else
        {
            grounded = cc.isGrounded;
        }

        if (grounded == false)
        {
            return move; // �����̸� ���� ����.
        }

        // �ʹ� ������ �������� ����.(���� ��ó�� �̼� ���� ����)
        if (speedPlaner < minMoveSpeedForProjection)
        {
            return move;
        }

        // �ٴ� ���� ���ϱ�(���� ����Ʈ�� �������� ����)
        Vector3 normal;
        bool got = TryGetGroundNormal(cc, out normal);
        if (got == false)
        {
            return move;
        }

        // ��� ���� �˻� : CC�� ���� �� ���� �ް��� ������ �ǳʶٱ�.
        float slope = Vector3.Angle(normal, Vector3.up); // ������ Up�� ����(0 = ����, 90 = ����)
        float limit = cc.slopeLimit - slopeStickAngleBias;
        if (slope > limit)
        {
            // �ް�翡���� ���� ��Ģ(CharacterController) ���ۿ� �ñ�.
            return move;
        }

        // ��� ���� : move�� �ٴ� ������ ���� ���� '���� ����' �̵�.
        Vector3 onPlane = Vector3.ProjectOnPlane(move, normal);

        // �������� ���̰� �����ϰ� �پ����� ���� ����(���� ����)
        float originalLen = new Vector2(move.x, move.z).magnitude;
        float projectedLen = new Vector2(onPlane.x, onPlane.z).magnitude;

        if (originalLen > 0.0001f)
        {
            float ratio = projectedLen / originalLen; // ���� �� ���� ����.
            if (ratio < maxProjectionLossRatio)
            {
                return move; // ���� ���Ϸ� �ٸ� ���� ����(���� ���� '����' ����)
            }
        }

        // ���� �ӵ�(y)�� ���� ����(������ ���� ����)
        onPlane.y = move.y;
        return onPlane;
    }

    /// <summary>
    /// ���� '��'�� �ε巴�� �ѵ��� ��� ������ ����.
    /// '���� ���� ��Ʈ + ���� ���� ����Ʈ' ���տ����� �������� ����.
    /// </summary>
    /// <param name="move"></param>
    /// <param name="cc"></param>
    /// <param name="playerController"></param>
    /// <returns></returns>
    public Vector3 ApplyStepSmoothing(Vector3 move, CharacterController cc, PlayerController playerController)
    {
        if (cc == null)
        {
            return move; // CC ���� -> ���� �Ұ�.
        }

        // ���� �̵��� ���� ������ ���� ���� ��� '������ ����'�� ����.
        Vector2 planer = new Vector2(move.x, move.z);
        if (planer.sqrMagnitude <= 0.000001f)
        {
            return ReleaseStepGradually(move);
        }

        // ���� ���� ���(���� ���⸸)
        Vector3 dir = GetHorizontalDir(move); // ���� ���� ����

        // ���� ���� �� ��(���� / ����) - ���� ���� �ο� �ɸ��� ���� ���� �ɸ��� ������ '���'���� ����.
        Vector3 originLow = cc.transform.position + Vector3.up * (cc.stepOffset * 0.5f); // ���� ����.
        Vector3 originHigh = cc.transform.position + Vector3.up * (cc.stepOffset + 0.08f); // ���� ����(���� �� ��)

        // ���� ���� ����( �ʹ� ��� �պ��� ������ ����)
        float castDist = Mathf.Max(cc.radius + stepProbeFoward, 0.15f);

        // ����ĳ��Ʈ ����(Ʈ���� ����)
        RaycastHit hitLow;
        bool lowHit = Physics.Raycast(originLow, dir, out hitLow, castDist, groundMask, QueryTriggerInteraction.Ignore);

        RaycastHit hitHigh;
        bool highHit = Physics.Raycast(originHigh, dir, out hitHigh, castDist, groundMask, QueryTriggerInteraction.Ignore);

        // ���� ���̴� �°� ���� ���̴� ��� '��'���� �����ϰ� ��� ����
        if (lowHit == true && highHit == false)
        {
            // ��ǥ ��·� : ���� ���̰� ���� ������ ���� ��.
            float desiredUp = hitLow.point.y - cc.transform.position.y;
            if (desiredUp < 0f)
            {
                desiredUp = 0f; // �ϰ� ������ ���� ������� ó������ ����.
            }
            if (desiredUp > maxStepHeight)
            {
                desiredUp = maxStepHeight; // ����� ����.
            }

            // SmoothDamp�� currentStepOffset�� desiredUp���� �Ų��ϰ� ����.
            float dt = Time.deltaTime;
            float smoothed = Mathf.SmoothDamp(
                currentStepOffset,      // ���� ��.
                desiredUp,              // ��ǥ ��.
                ref stepUpVelocity,     // ���� �ӵ� ĳ��(����)
                stepUpSmoothTime,       // ���� �ð�.
                Mathf.Infinity,         // �ִ� �ӵ� ������.
                dt                      // ������ �ð�.
             );

            // �̹� �����ӿ��� ������ ��·�(��Ÿ) ���.
            float delta = smoothed - currentStepOffset;
            currentStepOffset = smoothed;

            // ���� y = ���� y + ��Ÿ(����� ����)
            float newY = move.y + delta;
            if (newY > maxStepHeight)
            {
                newY = maxStepHeight;
            }
            move.y = newY;
        }
        else
        {
            //�� ��Ȳ�� �ƴϸ� �������� ���������� 0���� ȸ��.
            move = ReleaseStepGradually(move);
        }

        return move;
    }

    /// <summary>
    /// �߹� ������ �۰� ������ �� 'ª�� �Ʒ���' �̵����� �ٴڿ� �� ���̴� ���� �ٿ�.
    /// ��� �ߤ��� ���� ���� �������� �ʽ��ϴ�.(����/��� ��� ����)
    /// </summary>
    /// <param name="cc"></param>
    /// <param name="playerController"></param>
    public void TrySnapDown(CharacterController cc, PlayerController playerController)
    {
        if (cc == null)
        {
            return;
        }

        // ��� ���̸� ���� �ٿ� ����(���� / ������ ���� ���� ����)
        if (cc.velocity.y > 0f)
        {
            return;
        }

        // ���� ������ : �÷��̾� �� �Ʒ� ��¦ ��.
        Vector3 start = cc.transform.position + Vector3.up * 0.1f;

        // �Ʒ��� ���� ���� ����� �ٴ� ���� ����.
        RaycastHit hit;
        bool got = Physics.Raycast(
            start,
            Vector3.down,
            out hit,
            snapDownDistance + 0.1f,
            groundMask,
            QueryTriggerInteraction.Ignore
        );

        if (got == true)
        {
            // ���� ��ġ�� �ٴ� ������ ���� ����.
            float gap = (start.y - hit.point.y) - 0.1f;

            // �ʹ� ���� �����̸� ����(�̼� ���� ����)
            if (gap < 0.015f)
            {
                return;
            }

            // �� �����ӿ� �̵��� �ϰ���(�ʹ� ũ�� �������� �ʵ��� ����)
            float step = Mathf.Min(gap, snapDownSpeed * Time.deltaTime);

            // �Ʒ��� Move - CharacterController ��Ģ(�浹 / ������)�� ����
            Vector3 down = Vector3.down * step;
            cc.Move(down);
        }
    }

    // ===== ���� ��ƿ =====

    /// <summary>
    /// �÷��̾� �� �Ʒ��� ����ĳ��Ʈ�Ͽ� �ٴ� ������ ����.
    /// </summary>
    /// <param name="cc"></param>
    /// <param name="normal"></param>
    /// <returns></returns>
    private bool TryGetGroundNormal(CharacterController cc, out Vector3 normal)
    {
        normal = Vector3.up; // ���� �� �⺻��(���� ����)

        // ���� �������� �ߺ��� �ణ ���� ��� ���� ����.
        Vector3 origin = cc.transform.position + Vector3.up * 0.2f;

        RaycastHit hit;
        bool got = Physics.Raycast(
            origin,
            Vector3.down,
            out hit,
            probeDistance + 0.25f,
            groundMask,
            QueryTriggerInteraction.Ignore
        );

        if (got == true)
        {
            normal = hit.normal; // ���� �ٴ� ����.
            return true;
        }

        return false;
    }

    /// <summary>
    /// ���� ���� ���� ���͸� ���Ѵ�( y = 0, ����ȭ).
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    private Vector3 GetHorizontalDir(Vector3 move)
    {
        Vector3 d = move; // ���� ���� : ���� ����.
        d.y = 0f;         // ���� ���и� ���.
        if (d.sqrMagnitude > 0f)
        {
            d.Normalize(); // 0�� �ƴ� ���� ����ȭ
        }
        return d;
    }

    /// <summary>
    /// ���� ��� ����ġ�� ������ 0���� �ǵ�����,
    /// �� ��ȭ��(����)�� �̹� ������ �̵��� �ݿ�.
    /// </summary>
    /// <param name="move"></param>
    /// <returns></returns>
    private Vector3 ReleaseStepGradually(Vector3 move)
    {
        float dt = Time.deltaTime;  // ������ �ð�.
        //  currentStepOffset -> 0���� �ε巴�� ����.
        float smoothed = Mathf.SmoothDamp(
            currentStepOffset,  // ���� ��.
            0f,                 // ��ǥ ��.
            ref stepUpVelocity, // ���� �ӵ� ĳ��(����)
            stepUpSmoothTime,   // ���� �ð�
            Mathf.Infinity,     // �ִ� �ӵ� ������
            dt                  // ������ �ð�
        );

        // �̹� �����ӿ��� ��ȭ�� ��ŭ y�� ������
        float delta = smoothed - currentStepOffset;
        currentStepOffset = smoothed;
        move.y += delta;
        return move;
    }
}
