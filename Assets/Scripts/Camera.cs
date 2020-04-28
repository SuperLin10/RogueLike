using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSpace;

public class Camera : MonoBehaviour
{
    private static Camera _instance;
    public static Camera Instance
    {
        get
        {
            return _instance;
        }
    }

    public float smoothing = 8;

    public Vector2 targetPos = new Vector2(0, 0);

    private void Awake()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Approximately(transform.position.x, targetPos.x) && Mathf.Approximately(transform.position.y, targetPos.y))
        {
            return;
        }

        Vector2 pos = Vector2.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
        transform.position = new Vector3(pos.x, pos.y, -10);
    }

    public void MoveTo (Vector3 pos)
    {
        float offsetX = Game.Instance.m_colMid - 10;
        float offsetY = Game.Instance.m_rowMid - 5;

        Vector3 p = pos - new Vector3(0, 0, 0);


        if (Game.Instance.m_colMid > 10)
        {
            if (Mathf.Sign(p.x) == 1)
            {
                targetPos.x = (p.x >= offsetX ? offsetX : p.x) + 0.5f;
            }
            else
            {
                targetPos.x = (p.x <= -offsetX ? -offsetX : p.x) - 0.5f;
            }
        }
        else
        {
            targetPos.x = 0;
        }

        if (Mathf.Sign(p.y) == 1)
        {
            targetPos.y = (p.y >= offsetY ? offsetY : p.y) + 0.5f;
        }
        else
        {
            targetPos.y = (p.y <= -offsetY ? -offsetY : p.y) - 0.5f;
        }
        //transform.position = new Vector3(targetPos.x, targetPos.y, -10);

        //if (Mathf.Abs(p.x) > 3)
        //{
        //    if (Mathf.Sign(p.x) == 1)
        //    {
        //        targetPos.x = (p.x > offsetX ? offsetX : p.x - 3) + oX;
        //    }
        //    else
        //    {
        //        targetPos.x = (p.x < offsetX ? -offsetX : p.x + 3) - oX;
        //    }
        //}
        //if (Mathf.Abs(p.y) > 3)
        //{
        //    if (Mathf.Sign(p.y) == 1)
        //    {
        //        targetPos.y = (p.y > offsetY ? offsetY : p.y - 3) + oY;
        //    }
        //    else
        //    {
        //        targetPos.y = (p.y < offsetY ? -offsetY : p.y + 3) - oY;
        //    }
        //}
        //transform.position = new Vector3(targetPos.x, targetPos.y, -10);
    }
}
