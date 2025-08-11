using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    //------------------애니메이션 변수-----------------------------
    public Sprite[] moveSprites;
    public Sprite[] dieSprites;

    private AnimState state = AnimState.Move;


    private int frame = 0;
    private float animtimer = 0.0f;
    public float frameRate = 0.15f;
    //-------------------------------------------------------------

    public PlayerHealth heartManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
