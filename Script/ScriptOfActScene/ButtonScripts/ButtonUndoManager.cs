using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUndoManager : MonoBehaviour
{
    [SerializeField] GameObject tilemapObj;

    Button button;

    TilemapManager tilemapManager;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Start()
    {
        button.onClick.AddListener(() => tilemapManager.UndoTile());

        tilemapManager = tilemapObj.GetComponent<TilemapManager>();
    }
}
