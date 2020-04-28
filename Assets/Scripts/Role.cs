using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Role : MonoBehaviour
{
    public float smoothing = 1;
    [HideInInspector]
    public Vector2 targetPos = new Vector2(1, 1);

    protected Animator m_animator;
    protected BoxCollider2D m_collider;

    void Awake ()
    {
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (Mathf.Approximately(transform.position.x, targetPos.x) && Mathf.Approximately(transform.position.y, targetPos.y))
        {
            return;
        }

        Vector2 pos = Vector2.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
        transform.position = new Vector3(pos.x, pos.y, 0);
    }

    /// <summary>
    /// 检测目标方向是否有障碍物，如果有则返回障碍物的tag值
    /// </summary>
    /// <param name="direction">目标方向向量</param>
    /// <returns></returns>
    public RaycastHit2D CheckDirection (Vector2 direction)
    {
        m_collider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(targetPos, targetPos + direction);
        m_collider.enabled = true;
        return hit;
    }
}
