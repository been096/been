using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveSimple : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float maxDeltaTime = 0.05f;
    public MapBounds mapBounds;


    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);
        dir = dir.normalized;

        float deltaTime = Time.deltaTime;
        if (deltaTime > maxDeltaTime)
        {
            deltaTime = maxDeltaTime;
        }

        Vector3 pos = transform.position;
        Vector3 delta = new Vector3(dir.x, dir.y, 0.0f) * moveSpeed * deltaTime;
        Vector3 nextpos = pos + delta;

        float minX;
        float minY;
        float maxX;
        float maxY;
        mapBounds.GetWorldBounds(out minX, out maxX, out minY, out maxY);

        nextpos.x = Mathf.Clamp(nextpos.x, minX, maxX);
        nextpos.y = Mathf.Clamp(nextpos.y, minY, maxY);

        transform.position = nextpos;
    }
}
