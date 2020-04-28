using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSpace;

[HelpURL("http://www.baidu.com")]
public class Player : Role
{
    public float restTime = 1;

    public AudioClip chop1Audio;
    public AudioClip chop2Audio;
    public AudioClip step1Audio;
    public AudioClip step2Audio;

    public AudioClip soda1Audio;
    public AudioClip soda2Audio;
    public AudioClip fruit1Audio;
    public AudioClip fruit2Audio;

    private float m_restTimer = 0;

    private float colMid;
    private float rowMid;

    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<BoxCollider2D>();
        colMid = Game.Instance.m_colMid;
        rowMid = Game.Instance.m_rowMid;
        targetPos.x = 1 - colMid;
        targetPos.y = 1 - rowMid;
        Game.Instance.SetPlayer(this);
        Camera.Instance.MoveTo(targetPos);
    }

    private void FixedUpdate()
    {
        m_restTimer += Time.deltaTime;
        if (m_restTimer < restTime) return;
        if (Game.Instance.m_food <= 0 || Game.Instance.isEnd) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0) v = 0;

        if (h != 0 || v != 0)
        {
            // 检测移动的前方是否有碰撞
            Vector2 dir = new Vector2(h, v);
            RaycastHit2D hit = CheckDirection(dir);
            if (hit.transform == null)
            {
                targetPos += dir;
                AudioManager.Instance.RandomPlay(step1Audio, step2Audio);
                Camera.Instance.MoveTo(targetPos);
            }
            else
            {
                switch (hit.collider.tag)
                {
                    case "OutWall":
                        break;
                    case "Wall":
                        m_animator.SetTrigger("Attack");
                        AudioManager.Instance.RandomPlay(chop1Audio, chop2Audio);
                        hit.collider.SendMessage("TakeDamage");
                        break;
                    case "Food":
                        Game.Instance.AddFood(10);
                        targetPos += dir;
                        AudioManager.Instance.RandomPlay(step1Audio, step2Audio);
                        Destroy(hit.transform.gameObject);
                        AudioManager.Instance.RandomPlay(fruit1Audio, fruit2Audio);
                        Camera.Instance.MoveTo(targetPos);
                        break;
                    case "Soda":
                        Game.Instance.AddFood(20);
                        targetPos += dir;
                        AudioManager.Instance.RandomPlay(step1Audio, step2Audio);
                        Destroy(hit.transform.gameObject);
                        AudioManager.Instance.RandomPlay(soda1Audio, soda2Audio);
                        Camera.Instance.MoveTo(targetPos);
                        break;
                    case "Enemy":
                        break;
                    default:
                        Debug.LogError("未知的标签");
                        break;
                }
            }
            
            m_restTimer = 0;
            Game.Instance.ReduceFood(1);
            Game.Instance.OnPlayerMove();
            Invoke("CheckOver", 2);
        }
    }

    public void TakeDamage (int lossFood)
    {
        m_animator.SetTrigger("damage");
        Game.Instance.ReduceFood(lossFood);
    }

    void CheckOver()
    {
        Game.Instance.CheckOver();
    }
}
