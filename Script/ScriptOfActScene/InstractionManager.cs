using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstractionManager : MonoBehaviour
{
    TMP_Text inatractionText;

    string mode;

    void Awake()
    {
        inatractionText = GetComponent<TMP_Text>();
    }

    void Start()
    {
        GameObject dontDestoyObj = GameObject.Find("DontDestroy");
        mode = dontDestoyObj.GetComponent<DontDestroyManager>().Mode;

        if (mode == "Edit")
        {
            inatractionText.text += "\nE：編集";
        }
    }
}
