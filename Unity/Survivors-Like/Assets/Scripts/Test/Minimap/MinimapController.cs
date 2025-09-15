using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public MapBounds mapbounds;
    public Transform player;
    public Camera worldcamera;

    public RectTransform minimapRect;
    public RectTransform iconplayer;
    public RectTransform rectCameraView;
   

    // Update is called once per frame
    void Update()
    {
        float minX;
        float maxX;
        float minY;
        float maxY;

        mapbounds.GetWorldBounds(out minX, out maxX, out minY, out maxY);

        float mapW = maxX - minX;
        float mapH = maxY - minY;

        float panelW = minimapRect.rect.width;
        float panelH = minimapRect.rect.height;

        Vector3 p = player.position;
        float tX = (p.x - minX) / mapW;
        float tY = (p.y - minY) / mapH;

        float px = (tX - 0.5f) * panelW;
        float py = (tY - 0.5f) * panelH;

        iconplayer.anchoredPosition = new Vector2(px, py);
    }
}
