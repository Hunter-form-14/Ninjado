using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyManager : MonoBehaviour
{
    [SerializeField] AudioClip wadaikoSe;
    [SerializeField] AudioClip HyoushigiSe;

    const int allStageNum = 3;
    const int horizontalBlockNumber = 80;
    const int verticalBlockNumber = 30;

    public static int stage = 0;
    public static int[,,] maps = new int[allStageNum, horizontalBlockNumber, verticalBlockNumber];

    AudioSource audioSource;

    string mode;
    public string Mode
    {
        get { return mode; }
        set { mode = value; }
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        audioSource = GetComponent<AudioSource>();

        for(int k=0; k<allStageNum; k++)
        {
            GetStage(k);
        }
    }

    void GetStage(int _num)
    {
        string filePath = "text" + _num.ToString();
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);

        if (textAsset != null)
        {
            string[] lines = textAsset.text.Split('\n');

            for (int j = 0; j < verticalBlockNumber; j++)
            {
                string[] data = lines[j].Split(',');
                for (int i = 0; i < horizontalBlockNumber; i++)
                {
                    maps[_num, i, j] = int.Parse(data[i]);
                }
            }
        }

    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(wadaikoSe);
    }

    public void PlayReturnSound()
    {
        audioSource.PlayOneShot(HyoushigiSe);
    }

    public static void SelectStage(int _num)
    {
        stage = _num;
    }

    public static void KeepMap(int[,,] _map, int _mapDataCount)
    {
        for(int i=0; i<horizontalBlockNumber; i++)
        {
            for(int j=0; j<verticalBlockNumber; j++)
            {
                maps[stage, i, j] = _map[_mapDataCount, i, j];
            }
        }
    }
}
