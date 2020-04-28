using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMgr : MonoBehaviour
{

    public GameObject about;
    // Start is called before the first frame update
    void Start()
    {
        about.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartBtnClick()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnAboutBtnClick()
    {
        about.SetActive(true);
    }
    public void OnAboutExitBtnClick()
    {
        about.SetActive(false);
    }
    public void OnExitBtnClick()
    {
        Application.Quit(0);
    }
}
