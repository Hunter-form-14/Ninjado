using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [SerializeField] TileBase woodTile;
    [SerializeField] TileBase stoneTile;
    [SerializeField] TileBase ghostSpawnTile;
    [SerializeField] TileBase itemTile;
    [SerializeField] TileBase woodBurnTile;
    [SerializeField] Camera EditModeCamera;
    [SerializeField] GameObject ghostObj;
    [SerializeField] GameObject burningObj;
    [SerializeField] GameObject playerObj;
    [SerializeField] AudioClip whooshSe;
    [SerializeField] AudioClip wadaikoDoubleSe;
    [SerializeField] AudioClip fuchiuchiDoubleSe;

    const int horizontalBlockNumber = 80;
    const int verticalBlockNumber = 30;
    const int maxStockOfMapData = 20;

    int[,,] map = new int[maxStockOfMapData, horizontalBlockNumber, verticalBlockNumber];
    int mapDataCount = 0;
    int currentMapData = 0;
    bool[,] isBurning = new bool[horizontalBlockNumber, verticalBlockNumber];
    List<GameObject> enemyList = new List<GameObject>();
    List<GameObject> burningList = new List<GameObject>();
    List<IEnumerator> delaySpreadFireCoroutines = new List<IEnumerator>();
    TileBase selectedTile;
    AudioSource audioSource;

    Tilemap tilemap;

    PlayerManager playerManager;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        audioSource = GetComponent<AudioSource>();

        selectedTile = woodTile;

        for (int j = 0; j < verticalBlockNumber; j++)
        {
            for (int i = 0; i < horizontalBlockNumber; i++)
            {
                map[0, i, j] = DontDestroyManager.maps[DontDestroyManager.stage, i, j];

                var position = new Vector3Int(i, j, 0);

                tilemap.SetTile(position, numberToTile(map[0, i, j]));
            }
        }
    }
    TileBase numberToTile(int _number)
    {
        if (_number == 0) return woodTile;
        else if (_number == 1) return stoneTile;
        else if (_number == 2) return ghostSpawnTile;
        else if (_number == 3) return itemTile;

        return null;
    }

    void Start()
    {
        playerManager = playerObj.GetComponent<PlayerManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fire"))
        {
            Transform otherTf = other.GetComponent<Transform>();

            BurnBlock(otherTf, 0.5f);
            BurnBlock(otherTf, -0.5f);

            Destroy(other.gameObject);
        }
    }

    void BurnBlock(Transform _otherTf, float _distance)
    {
        int x = Mathf.FloorToInt(_otherTf.position.x + _otherTf.localScale.x * 0.5f);
        int y = Mathf.FloorToInt(_otherTf.position.y + _distance);

        var position = new Vector3Int(x, y, 0);

        if (tilemap.GetTile(position) == woodTile && !isBurning[position.x, position.y])
        {
            isBurning[position.x, position.y] = true;

            var coroutine = DelaySpreadFire(position);
            StartCoroutine(coroutine);
            delaySpreadFireCoroutines.Add(coroutine);

            tilemap.SetTile(position, woodBurnTile);
            GameObject burningInstance = Instantiate(burningObj, (Vector3)position + new Vector3(0.5f, 0.5f, 0f), Quaternion.identity);
            burningList.Add(burningInstance);

            Destroy(burningInstance, 1.0f);
        }
    }

    IEnumerator DelaySpreadFire(Vector3Int _position)
    {
        yield return new WaitForSeconds(1.0f); // 燃え広がる遅延時間
        SpreadFire(_position);
    }

    void SpreadFire(Vector3Int _position)
    {
        Vector3Int leftPosition = _position + Vector3Int.left;
        Vector3Int rightPosition = _position + Vector3Int.right;
        Vector3Int downPosition = _position + Vector3Int.down;
        Vector3Int upPosition = _position + Vector3Int.up;

        SpreadFireAtPosition(leftPosition);
        SpreadFireAtPosition(rightPosition);
        SpreadFireAtPosition(downPosition);
        SpreadFireAtPosition(upPosition);

        tilemap.SetTile(_position, null);
    }

    void SpreadFireAtPosition(Vector3Int _position)
    {
        if (tilemap.GetTile(_position) == woodTile && !isBurning[_position.x, _position.y])
        {
            isBurning[_position.x, _position.y] = true;

            var coroutine = DelaySpreadFire(_position);
            StartCoroutine(coroutine);
            delaySpreadFireCoroutines.Add(coroutine);

            tilemap.SetTile(_position, woodBurnTile);
            GameObject burningInstance = Instantiate(burningObj, (Vector3)_position + new Vector3(0.5f, 0.5f, 0f), Quaternion.identity);

            burningList.Add(burningInstance);

            Destroy(burningInstance, 1.0f);
        }
    }

    public void StopAllDelaySpreadFireCoroutines()
    {
        foreach (var coroutine in delaySpreadFireCoroutines)
        {
            StopCoroutine(coroutine);
        }
        delaySpreadFireCoroutines.Clear();
    }

    public void ChooseTile(TileBase _tile)
    {
        selectedTile = _tile;
    }

    public void UpdateTile()
    {
        //マウス座標の取得
        Vector3 mousePos = Input.mousePosition;
        //スクリーン座標をワールド座標に変換
        Vector3 worldPos = EditModeCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        int x = Mathf.FloorToInt(worldPos.x);
        int y = Mathf.FloorToInt(worldPos.y);

        var position = new Vector3Int(x, y, 0);

        if (isInStageArea(x)
        && isInDisplayArea(mousePos.x, mousePos.y)
        && playerManager.OriginalPositionInt != position
        && !playerManager.IsDrugged)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mapDataCount++;
                currentMapData = mapDataCount;

                for (int i = 0; i < horizontalBlockNumber; i++)
                {
                    for (int j = 0; j < verticalBlockNumber; j++)
                    {
                        map[mapDataCount % maxStockOfMapData, i, j] = map[(mapDataCount - 1) % maxStockOfMapData, i, j];
                    }
                }
            }
            if (Input.GetMouseButton(0))
            {
                tilemap.SetTile(position, selectedTile);

                if (map[mapDataCount % maxStockOfMapData, x, y] != tileToNumber(selectedTile))
                {
                    map[mapDataCount % maxStockOfMapData, x, y] = tileToNumber(selectedTile);

                    audioSource.PlayOneShot(whooshSe);
                }
            }
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftShift))
        {
            RedoTile();
        }
        else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z))
        {
            UndoTile();
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
        {
            SaveMap();
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            DeleteTile();
        }
    }

    public void UndoTile()
    {
        if (mapDataCount > 0 && mapDataCount > currentMapData - maxStockOfMapData)
        {
            mapDataCount--;

            for (int j = 0; j < verticalBlockNumber; j++)
            {
                for (int i = 0; i < horizontalBlockNumber; i++)
                {
                    var position = new Vector3Int(i, j, 0);

                    tilemap.SetTile(position, numberToTile(map[mapDataCount % maxStockOfMapData, i, j]));
                }
            }

            audioSource.PlayOneShot(fuchiuchiDoubleSe);
        }
    }

    public void RedoTile()
    {
        if (mapDataCount < currentMapData)
        {
            mapDataCount++;

            for (int j = 0; j < verticalBlockNumber; j++)
            {
                for (int i = 0; i < horizontalBlockNumber; i++)
                {
                    var position = new Vector3Int(i, j, 0);

                    tilemap.SetTile(position, numberToTile(map[mapDataCount % maxStockOfMapData, i, j]));
                }
            }

            audioSource.PlayOneShot(wadaikoDoubleSe);
        }
    }

    public void DeleteTile()
    {
        mapDataCount++;
        currentMapData = mapDataCount;

        for (int i = 0; i < horizontalBlockNumber; i++)
        {
            for (int j = 0; j < verticalBlockNumber; j++)
            {
                map[mapDataCount % maxStockOfMapData, i, j] = -1;

                var position = new Vector3Int(i, j, 0);
                tilemap.SetTile(position, null);
            }
        }
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < 5; i++)
            {
                map[mapDataCount % maxStockOfMapData, i, j] = 1;

                var position = new Vector3Int(i, j, 0);
                tilemap.SetTile(position, stoneTile);
            }
            for (int i = horizontalBlockNumber - 5; i < horizontalBlockNumber; i++)
            {
                map[mapDataCount % maxStockOfMapData, i, j] = 1;

                var position = new Vector3Int(i, j, 0);
                tilemap.SetTile(position, stoneTile);
            }
        }
    }

    public void SaveMap()
    {
        string[] lines = new string[verticalBlockNumber];

        for (int j = 0; j < verticalBlockNumber; j++)
        {
            for (int i = 0; i < horizontalBlockNumber; i++)
            {
                if (i == 0)
                {
                    lines[j] = map[mapDataCount % maxStockOfMapData, i, j].ToString();
                }
                else
                {
                    lines[j] += "," + map[mapDataCount % maxStockOfMapData, i, j].ToString();
                }
            }
        }

        string filePath = "Assets/Resources/text" + DontDestroyManager.stage.ToString() + ".txt";
        File.WriteAllLines(filePath, lines);

        DontDestroyManager.KeepMap(map, mapDataCount % maxStockOfMapData);
    }

    int tileToNumber(TileBase _tile)
    {
        if (_tile == woodTile) return 0;
        else if (_tile == stoneTile) return 1;
        else if (_tile == ghostSpawnTile) return 2;
        else if (_tile == itemTile) return 3;

        return -1;
    }

    bool isInStageArea(int _x)
    {
        return _x >= 5 && _x < horizontalBlockNumber - 5;
    }

    bool isInDisplayArea(float _x, float _y)
    {
        return _y < 950.0f && _x > 150.0f;
    }

    public void SpawnEnemy()
    {
        for (int i = 0; i < horizontalBlockNumber; i++)
        {
            for (int j = 0; j < verticalBlockNumber; j++)
            {
                if (map[mapDataCount % maxStockOfMapData, i, j] == 2)
                {
                    var position = new Vector3Int(i, j, 0);
                    tilemap.SetTile(position, null);
                    var spawnPosition = new Vector3(i + 0.5f, j + 0.5f, 0f);
                    GameObject ghostInstance = Instantiate(ghostObj, spawnPosition, Quaternion.identity);
                    enemyList.Add(ghostInstance);
                }
            }
        }
    }

    public void ResetEnemy()
    {
        foreach (GameObject obj in enemyList)
        {
            Destroy(obj);
        }
        enemyList.Clear();
    }

    public void ResetTile()
    {
        for (int i = 0; i < horizontalBlockNumber; i++)
        {
            for (int j = 0; j < verticalBlockNumber; j++)
            {
                isBurning[i, j] = false;

                var position = new Vector3Int(i, j, 0);

                tilemap.SetTile(position, numberToTile(map[mapDataCount % maxStockOfMapData, i, j]));
            }
        }
        foreach (GameObject obj in burningList)
        {
            Destroy(obj);
        }
        burningList.Clear();
    }
}
