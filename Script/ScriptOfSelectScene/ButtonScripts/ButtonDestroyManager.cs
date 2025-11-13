using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDestroyManager : MonoBehaviour
{
    [SerializeField] MiniTilemapManager miniTilemapManager;
    [SerializeField] AudioClip slashSe;

    const int horizontalBlockNumber = 80;
    const int verticalBlockNumber = 30;

    Button button;
    AudioSource audioSource;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        audioSource = GetComponent<AudioSource>();
    }

    void OnClick()
    {
        audioSource.PlayOneShot(slashSe);

        for (int i = 0; i < horizontalBlockNumber; i++)
        {
            for (int j = 0; j < verticalBlockNumber; j++)
            {
                DontDestroyManager.maps[DontDestroyManager.stage, i, j] = -1;
            }
        }
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < 5; i++)
            {
                DontDestroyManager.maps[DontDestroyManager.stage, i, j] = 1;
            }
            for (int i = horizontalBlockNumber - 5; i < horizontalBlockNumber; i++)
            {
                DontDestroyManager.maps[DontDestroyManager.stage, i, j] = 1;
            }
        }
        miniTilemapManager.LoadTile();
    }
}
