using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 场景管理

namespace GameSpace
{
    public class Game
    {
        private static readonly Game instance = new Game();

        // 显式静态构造函数告诉C＃编译器
        // 不要将类型标记为BeforeFieldInit
        static Game()
        {

        }
        private Game()
        {
            cols = 30;
            rows = 30;
        }

        public static Game Instance
        {
            get
            {
                return instance;
            }
        }

        private int m_level = 2;
        private int m_rows = 0;
        private int m_cols = 0;
        private List<Enemy> m_enemys = new List<Enemy>();
        private bool m_sleepStep = false;

        private Text m_foodText;
        private Text m_FailText;
        private Player m_player;

        public bool isEnd = false;
        public float m_rowMid = 0;
        public float m_colMid = 0;
        public Vector3 m_exitVector = new Vector3();
        public int m_food = 200;

        public int Level
        {
            get
            {
                return m_level;
            }
            set
            {
                if (value <= 0 || value > 10)
                {
                    throw new System.ApplicationException("关卡等级超过限制");
                }
                m_level = value;
            }
        }

        public int rows
        {
            get
            {
                return m_rows;
            }
            set
            {
                if (value <= 5)
                {
                    throw new System.ApplicationException("行数最少五行");
                }
                m_rows = value;
                m_rowMid = m_rows / 2;
                if (m_rows % 2 == 0) m_rowMid -= 0.5f;
                m_exitVector.y = m_rows - m_rowMid - 2;
            }
        }

        public int cols
        {
            get
            {
                return m_cols;
            }
            set
            {
                if (value <= 5)
                {
                    throw new System.ApplicationException("列数最少五列");
                }
                m_cols = value;
                m_colMid = m_cols / 2;
                if (m_cols % 2 == 0) m_colMid -= 0.5f;
                m_exitVector.x = m_cols - m_colMid - 2;
            }
        }
        public void InitGame()
        {
            m_foodText = GameObject.Find("FoodText").GetComponent<Text>();
            UpdateFoodText(0);

            m_FailText = GameObject.Find("FailText").GetComponent<Text>();
            m_FailText.enabled = false;
        }
        void UpdateFoodText (int foodChange)
        {
            if (foodChange == 0)
            {
                m_foodText.text = "Food: " + m_food;
            }
            else
            {
                string str = foodChange < 0 ? foodChange.ToString() : "+" + foodChange;
                m_foodText.text = str + " Food: " + m_food;
            }
        }
        public void ReduceFood (int count)
        {
            m_food -= count;
            UpdateFoodText(-count);

            if (m_food <= 0)
            {
                m_FailText.enabled = true;
                AudioManager.Instance.PlayDie();
            }
        }
        public void AddFood (int count)
        {
            m_food += count;
            UpdateFoodText(count);
        }

        public void AddEnemy (Enemy enemy)
        {
            m_enemys.Add(enemy);
        }
        public void RemoveEnemy (Enemy enemy)
        {
            m_enemys.Remove(enemy);
        }
        public void RemoveAllEnemy ()
        {
            m_enemys.Clear();
        }
        public void SetPlayer (Player player)
        {
            m_player = player;
        }
        public void OnPlayerMove()
        {
            if (m_sleepStep)
            {
                m_sleepStep = false;
            }
            else
            {
                foreach(Enemy enemy in m_enemys)
                {
                    enemy.Move();
                }
                m_sleepStep = true;
            }
            // 检测是否到了终点
            if (Mathf.Approximately(m_exitVector.x, m_player.targetPos.x) && Mathf.Approximately(m_exitVector.y, m_player.targetPos.y))
            {
                isEnd = true;
                // 加载下一个关卡
                NextGame();
            }
        }
        public void CheckOver()
        {
            if (m_food <= 0)
            {
                RestartGame();
            }
        }
        public void RestartGame()
        {
            m_level = 1;
            m_food = 20;
            RemoveAllEnemy();
            SceneManager.LoadScene(0);
        }
        public void NextGame()
        {
            m_level++;
            rows++;
            cols++;
            RemoveAllEnemy();
            SceneManager.LoadScene(0);
        }
    }
}
