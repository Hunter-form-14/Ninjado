using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonModeManager : MonoBehaviour
{
    GameObject dontDestoyObj;
    DontDestroyManager dontDestroyManager;

    void Start()
    {
        dontDestoyObj = GameObject.Find("DontDestroy");
        dontDestroyManager = dontDestoyObj.GetComponent<DontDestroyManager>();
    }

    public void OnClick()
    {
        dontDestroyManager.PlayClickSound();

        if (this.gameObject.name == "ButtonOfEdit")
        {
            dontDestroyManager.Mode = "Edit";
        }
        else if (this.gameObject.name == "ButtonOfPlay")
        {
            dontDestroyManager.Mode = "Play";
        }

        SceneManager.LoadScene("ActScene");
    }
}
