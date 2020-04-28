using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSpace;


public class UIMgr : MonoBehaviour
{
    public Image dayImage;
    public Text dayText;
    // Start is called before the first frame update
    void Start()
    {
        dayText.text = "Day" + Game.Instance.Level;
        Invoke("HideBlack", 1);
    }

    void HideBlack()
    {
        dayImage.gameObject.SetActive(false);
        Game.Instance.isEnd = false;
    }
}
