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
        float x = Input.GetAxisRaw("Horizontal"); // GetAxis�� Ű�� ���� �ٷ� ������ �ʰ� ��¦ ������ �پ� �̲�������, GetAxisRaw�� �ٷ� ����. ���ӵ��� �ֳ� ���� ����. Raw�� ����.
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
