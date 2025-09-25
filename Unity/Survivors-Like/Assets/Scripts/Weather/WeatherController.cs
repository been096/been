using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public Camera cameraToFollow;
    public ParticleSystem rainSystem;
    public ParticleSystem snowSystem;
    public float intensity = 0.0f;   // 0 ~ 1
    public float windX = 0.0f;       // -5 ~ +5 ����
    public bool enableRain = true;
    public bool enableSnow = false;

    private void Awake()
    {
        if (cameraToFollow == null)
        {
            cameraToFollow = Camera.main;
        }
        FollowCameraImmediate();
    }

    private void LateUpdate()
    {
        FollowCameraImmediate();

        ApplySettings(rainSystem, enableRain == true);
        ApplySettings(snowSystem, enableSnow == true);
    }

    void FollowCameraImmediate()
    {
        if (cameraToFollow == null)
        {
            return;
        }
        Vector3 camPos = cameraToFollow.transform.position;
        transform.position = new Vector3(camPos.x, camPos.y, transform.position.z);
    }

    void ApplySettings(ParticleSystem ps, bool enabled)
    {
        if (ps == null)
        {
            return;
        }

        var emission = ps.emission;
        var main = ps.main;
        var vel = ps.velocityOverLifetime;

        if (enabled == true)
        {
            if (ps.isPlaying == false)
            {
                ps.Play();
            }

            // 1) Emission rate : intensity ���
            float baseRate = 80f;   // ������Ʈ�� �°� ����.
            float rate = baseRate * Mathf.Clamp01(intensity);

            ParticleSystem.MinMaxCurve rateCurve = emission.rateOverTime;
            rateCurve.constant = rate;
            emission.rateOverTime = rateCurve;

            // 2) �ٶ� : x�� �ӵ� ����(���� ����)
            vel.enabled = true;
            vel.space = ParticleSystemSimulationSpace.World;
            ParticleSystem.MinMaxCurve vx = vel.x;
            vx.constant = windX;
            vel.x = vx;

            // 3) �ӵ�/ũ�� ����(������ �������� �� ������ �ణ Ŀ��)
            ParticleSystem.MinMaxCurve spd = main.startSpeed;
            spd.constant = Mathf.Lerp(1.5f, 8.0f, Mathf.Clamp01(intensity));
            main.startSpeed = spd;

            ParticleSystem.MinMaxCurve sz = main.startSize;
            sz.constant = Mathf.Lerp(0.05f, 0.15f, Mathf.Clamp01(intensity));
            main.startSize = sz;
        }
        else
        {
            if (ps.isPlaying == true)
            {
                ps.Stop();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
