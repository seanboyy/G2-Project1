using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To type the next 4 lines, start by typing /// and then Tab.
/// <summary>
/// Keeps a GameObject on screen. 
/// Note that this ONLY works for an orthographic Main Camera at [ 0, 0, 0 ].
/// </summary>
public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 1f;
    public bool keepOnScreen = false;
    public bool wrap = false;
    public bool deleteIfOffScren = false;

    [Header("Set Dynamically")]
    public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;

    [HideInInspector]
    public bool offRight, offLeft, offUp, offDown;

    void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        isOnScreen = true;
        offRight = offLeft = offUp = offDown = false;

        if (pos.x > camWidth - radius)
        {
            if (wrap)
            {
                transform.position = new Vector3(-camWidth, pos.y, pos.z);
            }
            else
            {
                pos.x = camWidth - radius;
                isOnScreen = false;
                offRight = true;
            }
        }

        if (pos.x < -camWidth + radius)
        {
            if (wrap)
            {
                transform.position = new Vector3(camWidth, pos.y, pos.z);
            }
            else
            {
                pos.x = -camWidth + radius;
                isOnScreen = false;
                offLeft = true;
            }
        }

        if (pos.y > camHeight - radius)
        {
            if (wrap)
            {
                transform.position = new Vector3(pos.x, -camHeight, pos.z);
            }
            else
            {
                pos.y = camHeight - radius;
                isOnScreen = false;
                offUp = true;
            }
        }

        if (pos.y < -camHeight + radius)
        {
            if (wrap)
            {
                transform.position = new Vector3(pos.x, camHeight, pos.z);
            }
            else
            {
                pos.y = -camHeight + radius;
                isOnScreen = false;
                offDown = true;
            }
        }

        isOnScreen = !(offRight || offLeft || offUp || offDown);
        if (!isOnScreen && deleteIfOffScren)
        {
            Destroy(gameObject);
        }
        if (keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            isOnScreen = true;
            offRight = offLeft = offUp = offDown = false;
        }
    }

    // Draw the bounds in the Scene pane using OnDrawGizmos()
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
