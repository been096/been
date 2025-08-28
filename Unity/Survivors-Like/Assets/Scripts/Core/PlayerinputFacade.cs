using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerinputFacade : MonoBehaviour
{
    public Animator animator;
    private PlayerCore player;

    private void Awake()
    {
        player = GetComponent<PlayerCore>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal"); // GetAxis는 키를 떼면 바로 멈추지 않고 살짝 관성이 붙어 미끄러지고, GetAxisRaw는 바로 멈춤. 가속도가 있냐 없냐 차이. Raw가 없음.
        float y = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(x, y).normalized;
        player.SetMoveInput(input);

        if(x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        if(x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        bool move = false;

        if( x != 0.0f || y != 0.0f)
        {
            move = true;
        }

        animator.SetBool("Move", move);
    }
}
