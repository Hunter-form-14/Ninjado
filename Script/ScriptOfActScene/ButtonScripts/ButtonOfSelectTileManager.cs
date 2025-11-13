using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ButtonOfSelectTileManager : MonoBehaviour
{
    [SerializeField] TileBase tile;
    [SerializeField] GameObject tilemapObj;
    [SerializeField] AudioClip kozutsumiSe;
    
    AudioSource audioSource;

    TilemapManager tilemapManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        tilemapManager = tilemapObj.GetComponent<TilemapManager>();
    }

    public void OnClick()
    {
        audioSource.PlayOneShot(kozutsumiSe);

        tilemapManager.ChooseTile(tile);
    }
}
