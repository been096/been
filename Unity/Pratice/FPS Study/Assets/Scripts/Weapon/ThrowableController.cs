using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �÷��̾� ��ô ��Ʈ�ѷ�.
/// - ��ư �� : �⺻ ����� ��� ������.
/// - ��ư Ȧ�� : �Ŀ� ����(�ɼ�)�� ���� ������
/// </summary>
public class ThrowableController : MonoBehaviour
{
    [Header("Refs")]
    public Camera playerCamera;                 // ���� ���� ����.
    public Transform hand;                      // ��/������ ����.
    public Grenade grenadePrefab;               // ����ź ������.

    [Header("Throw")]
    public float baseThorwSpeed = 14.0f;        // �⺻ ������ �ӷ�.
    public float maxThorwSpeed = 22.0f;         // Ȧ�� �� �ִ� �ӷ�.
    public float chargeTime = 1.0f;             // �ִ� ������� �ɸ��� �ð�(��)
    public float angularSpin = 20.0f;           // ȸ��(�ð�)

    private bool charging;                      // �Ŀ� ���� ��.
    private float charge;                       // 0 ~ 1 ����.
    private bool fireRequested;                 // �Է� ������ ����.

    public void OnThrow(InputAction.CallbackContext ctx)
    {
        if (ctx.started == true)
        {
            charging = true;
            charge = 0.0f;
        }
        else if (ctx.canceled == true || ctx.performed == true)
        {
            charging = false;
            fireRequested = true;
        }
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        if (charging == true)
        {
            // �Ŀ� ����.
            float inc = 0.0f;
            if ( chargeTime > 0.0001f)

            {
                inc = dt / chargeTime;
            }
            charge = charge + inc;
            if ( charge > 1.0f)
            {
                charge = 1.0f;
            }
        }

        if (fireRequested == true)
        {
            fireRequested = false;
            ThrowOne();
        }
    }

    private void ThrowOne()
    {
        if (grenadePrefab == null)
        {
            return;
        }
        if (playerCamera == null)
        {
            return;
        }

        // 1) ���� ����/����.
        Vector3 origin = hand != null ? hand.position : playerCamera.transform.position;
        Vector3 dir = playerCamera.transform.forward;

        // 2) �ӷ� ���� (������ �ݿ�)
        float spd = Mathf.Lerp(baseThorwSpeed, maxThorwSpeed, charge);
        Vector3 vel = dir * spd;

        // 3) ���ӵ�(ȸ�� ����)
        Vector3 ang = Random.onUnitSphere * angularSpin;

        // 4) �ν��Ͻ� ���� �� Throw ȣ��.
        Grenade g = Instantiate(grenadePrefab, origin, Quaternion.identity);
        g.Throw(vel, ang);

        // 5) ���� �� �ʱ�ȭ.
        charge = 0.0f;
    }
}
