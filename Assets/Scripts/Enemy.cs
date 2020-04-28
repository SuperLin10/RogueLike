using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSpace;

public class Enemy : Role
{
    public int lossFood = 10;
    private GameObject m_player;

    public AudioClip attackAudio;

    // Start is called before the first frame update
    void Start()
    {
        Game.Instance.AddEnemy(this);
        targetPos.x = transform.position.x;
        targetPos.y = transform.position.y;
    }

    public void SetPlayer (GameObject player)
    {
        m_player = player;
    }

    public void Move()
    {
        // TODO: 这里简单通过朝向进行判断下一步需要进行移动的方向， 后续可以考虑使用A-Star算法进行计算
        Vector2 offset = m_player.transform.position - transform.position;
        if (offset.magnitude < 1.1f)
        { // 敌人与玩家间的距离小到一定程度进行攻击
            m_animator.SetTrigger("Attack");
            AudioManager.Instance.RandomPlay(attackAudio);
            m_player.SendMessage("TakeDamage", lossFood);
        }
        else
        {
            float x = 0, y = 0;
            if (Mathf.Abs(offset.y) > Mathf.Abs(offset.x))
            {
                y = offset.y < 0 ? -1 : 1; // 按照y轴移动
            }
            else
            {
                x = offset.x < 0 ? -1 : 1; // 按照x轴移动
            }
            Vector2 dir = new Vector2(x, y);
            RaycastHit2D hit = CheckDirection(dir);
            if (hit.transform == null)
            {
                targetPos += dir;
            }
            else
            {
                if (hit.collider.tag == "Food" || hit.collider.tag == "Soda")
                {
                    targetPos += dir;
                }
            }
        }
    }
}
