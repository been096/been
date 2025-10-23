using UnityEngine;

/// <summary>
/// �߻� �� ī�޶� Pivot�� ª�� �ݵ��� �ο�.(���� ��¦ ƨ�� + �¿� �ణ ����).
/// </summary>
public class RecoilApplier : MonoBehaviour
{
    public Transform cameraPivot;       // ȸ�� ���� ����(���� CameraPivot)
    public float pitchKick = 1.2f;      // ���� Ƣ�� ����.
    public float yawJitter = 0.4f;      // �¿� ����.
    public float returnTime = 0.12f;    // ����ġ ���� �ð�(��)

    private float timeLeft;             // ���� �ݵ� �ð�
    private Quaternion baseLocalRot;    // ���� ȸ��
    private Quaternion targetRot;       // ��ǥ ȸ��.

    private void Awake()
    {
        if (cameraPivot == null)
        {
            cameraPivot = transform;
        }
        baseLocalRot = cameraPivot.localRotation;
        targetRot = baseLocalRot;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft > 0.0f)
        {
            float t = 1.0f - (timeLeft / returnTime); // 0 -> 1
            float smooth = Mathf.SmoothStep(0.0f, 1.0f, t);
            cameraPivot.localRotation = Quaternion.Slerp(targetRot, baseLocalRot, smooth);

            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0.0f)
            {
                cameraPivot.localRotation = baseLocalRot;
            }
        }
    }

    public void Kick()
    {
        // ���� pitchKick, �¿�� ����.
        float yaw = Random.Range(-yawJitter, yawJitter);
        Quaternion kick = Quaternion.Euler(-pitchKick, yaw, 0.0f);

        baseLocalRot = cameraPivot.localRotation;
        targetRot = baseLocalRot * kick;
        timeLeft = returnTime;
    }
}
