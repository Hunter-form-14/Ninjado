using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputStageNameManager : MonoBehaviour
{
    [SerializeField] TMP_Text[] texts;
    TMP_InputField inputField;

    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.text = texts[DontDestroyManager.stage].text;
    }

    public void InputText()
    {
        texts[DontDestroyManager.stage].text = inputField.text;
    }
}