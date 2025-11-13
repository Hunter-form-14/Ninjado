using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDeleteManager : MonoBehaviour
{
    [SerializeField] GameObject tilemapObj;
    [SerializeField] AudioClip slashSe;
    
    AudioSource audioSource;

    TilemapManager tilemapManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        tilemapManager = tilemapObj.GetComponent<TilemapManager>();
    }

    public void OnClick()
    {
        audioSource.PlayOneShot(slashSe);

        tilemapManager.DeleteTile();
    }
}
