using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "Game/BulletData")]
public class BulletDataSO : ScriptableObject
{
    public float fireIntervaleconds = 0.5f;
    public float detectRadius = 5.0f;
    public float damage = 20;
    public float projectileSpeed = 10.0f;
    public float projectileLifeSeconds = 3.0f;
    public int pierceCount = 0;
    public float muzzleOffset = 0.6f;       // 발사 위치. 캐릭터랑 어느정도 떨어진 곳에서 발사할거냐의 수치
    public int multishotCount = 1;
    public float totalSpreadAngleDegrees = 0.0f;
    public bool fireEvenIfNoTarget = false;



    
}
