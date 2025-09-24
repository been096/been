using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightController : MonoBehaviour
{
    [Header("Cycle")]
    public float cycleSeconds = 90f;        //�� -> �� -> �� �� ���� �ð�(��).
    [Range(0f, 1f)]
    public float time01 = 0f;              // �Ϸ� ���൵(0 = ����, 0.25 = ��ħ, 0.5 = ����, 0.75 = ����, 1 = ����)
    public bool runAutomatically = true;   // true�� �ڵ����� �ð��� �帧.

    [Header("Night Curve")]
    public float nightCurveSharpness = 1.0f; // 1 = �⺻, >�̸� ���� �� �� ���/���ϰ� ������.

    public bool isNight = false;
    public EnemyCore enemyCore;
    public float nightBerserk = 30.0f;

    /// <summary>
    /// ���� �ð����� '���� ����(0 ~ 1)'�� ���Ѵ�.
    /// 0.5(����) �α��� 0, 0 �Ǵ� 1(����) ��ó�� 1.
    /// </summary>
    /// <returns></returns>
    public float GetNightFactor()
    {
        float raw = 0f;

        if (time01 >= 0.5f)
        {
            raw = (time01 - 0.5f) * 2f;
        }
        else
        {
            raw = 1f - (time01 * 2f);
        }

        float smooth = Mathf.SmoothStep(0f, 1f, raw);
        if (nightCurveSharpness > 1.0f)
        {
            smooth = Mathf.Pow(smooth, nightCurveSharpness);
        }
        return smooth;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (runAutomatically == true)
        {
            float dt = Time.deltaTime;
            if (cycleSeconds > 0f)
            {
                time01 = time01 + (dt / cycleSeconds);
                if (time01 >= 1f)
                {
                    time01 = time01 - 1f;
                }
                if (time01 < 0f)
                {
                    time01 = 0f;
                }
            }
        }

        float nightFactor = GetNightFactor();
        bool nowNight = (nightFactor > 0.5f);
        if (nowNight != isNight)
        {
            isNight = nowNight;
        }

        if (isNight == true)
        {
            enemyCore.Buff(); 
        }
    }
}
