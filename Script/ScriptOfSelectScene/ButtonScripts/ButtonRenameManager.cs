using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonRenameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        inputField.ActivateInputField();
    }
}
