using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MiniTilemapManager : MonoBehaviour
{
    [SerializeField] TileBase woodTile;
    [SerializeField] TileBase stoneTile;
    [SerializeField] TileBase ghostSpawnTile;
    [SerializeField] TileBase itemTile;

    Tilemap tilemap;
    const int horizontalBlockNumber = 80;
    const int verticalBlockNumber = 30;
    int[,] map = new int[horizontalBlockNumber, verticalBlockNumber];

    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        LoadTile();
    }

    public void LoadTile()
    {
        for (int j = 0; j < verticalBlockNumber; j++)
        {
            for (int i = 0; i < horizontalBlockNumber; i++)
            {
                map[i, j] = DontDestroyManager.maps[DontDestroyManager.stage, i, j];

                var position = new Vector3Int(i, j, 0);

                tilemap.SetTile(position, numberToTile(map[i, j]));
            }
        }
    }
    TileBase numberToTile(int _number)
    {
        if (_number == 0)
        {
            return woodTile;
        }
        else if (_number == 1)
        {
            return stoneTile;
        }
        else if (_number == 2)
        {
            return ghostSpawnTile;
        }
        else if (_number == 3)
        {
            return itemTile;
        }
        return null;
    }

    public void ResetTile()
    {
        for (int i = 0; i < horizontalBlockNumber; i++)
        {
            for (int j = 0; j < verticalBlockNumber; j++)
            {
                map[i, j] = -1;

                var position = new Vector3Int(i, j, 0);
                tilemap.SetTile(position, null);
            }
        }
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < 5; i++)
            {
                map[i, j] = 1;

                var position = new Vector3Int(i, j, 0);
                tilemap.SetTile(position, stoneTile);
            }
            for (int i = horizontalBlockNumber - 5; i < horizontalBlockNumber; i++)
            {
                map[i, j] = 1;

                var position = new Vector3Int(i, j, 0);
                tilemap.SetTile(position, stoneTile);
            }
        }
    }
}
