using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSaveManager : MonoBehaviour
{
    [SerializeField] GameObject tilemapObj;
    [SerializeField] AudioClip shishiodoshiSe;
    
    AudioSource audioSource;

    TilemapManager tilemapManager;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        tilemapManager = tilemapObj.GetComponent<TilemapManager>();
    }

    public void OnClick()
    {
        audioSource.PlayOneShot(shishiodoshiSe);

        tilemapManager.SaveMap();
    }
}
