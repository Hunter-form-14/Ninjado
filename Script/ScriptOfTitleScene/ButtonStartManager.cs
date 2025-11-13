using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonStartManager : MonoBehaviour
{
    [SerializeField] GameObject dontDestoyObj;

    DontDestroyManager dontDestroyManager;

    void Start()
    {
        dontDestroyManager = dontDestoyObj.GetComponent<DontDestroyManager>();
    }

    public void OnClick()
    {
        dontDestroyManager.PlayClickSound();
        
        SceneManager.LoadScene("SelectScene");
    }
}
