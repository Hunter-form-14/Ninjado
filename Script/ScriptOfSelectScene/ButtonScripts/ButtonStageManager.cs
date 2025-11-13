using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonStageManager : MonoBehaviour
{
    [SerializeField] GameObject miniTilemap;
    [SerializeField] TMP_Text text;
    [SerializeField] int numberOfStage;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] AudioClip kozutsumiSe;

    AudioSource ausioSource;
    
    MiniTilemapManager minitilemapManager;

    string[] stageName = {"舞台壱", "舞台弐", "舞台参"};

    void Awake()
    {
        ausioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        minitilemapManager = miniTilemap.GetComponent<MiniTilemapManager>();
        text.text = stageName[numberOfStage];
    }

    public void OnClick()
    {
        ausioSource.PlayOneShot(kozutsumiSe);

        DontDestroyManager.SelectStage(numberOfStage);

        minitilemapManager.LoadTile();

        inputField.text = text.text;
    }
}
