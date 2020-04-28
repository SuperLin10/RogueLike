using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLoading : MonoBehaviour
{
    public float smoothing = 1;
    public RectTransform progress;
    public RectTransform progressBar;

    private float m_curProcess = 0;

    public static AsyncOperation async;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: 这里的只预加载了场景， 后面改为真正的预加载

        async = SceneManager.LoadSceneAsync("Menu");
        //禁止加载完成后自动切换场景
        async.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_curProcess >= 1)
        {
            progressBar.sizeDelta = new Vector2(progress.rect.width, progress.rect.height);
            async.allowSceneActivation = true;
            //SceneManager.LoadScene("Menu");
        }
        else
        {
            m_curProcess += Time.deltaTime * smoothing;
            progressBar.sizeDelta = new Vector2(progress.rect.width * m_curProcess, progress.rect.height);
        }
    }
}
