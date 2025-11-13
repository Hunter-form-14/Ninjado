using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonReturnManager : MonoBehaviour
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
        dontDestroyManager.PlayReturnSound();

        SceneManager.LoadScene("SelectScene");
    }
}
