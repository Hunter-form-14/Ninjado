using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeScrollbarManager : MonoBehaviour
{
    [SerializeField] Scrollbar volumeScrollbar;
    
    GameObject dontDestroyObj;
    AudioListener dontDestroyAudioListener;

    void Start()
    {
        dontDestroyObj = GameObject.Find("DontDestroy");

        dontDestroyAudioListener = dontDestroyObj.GetComponent<AudioListener>();

        volumeScrollbar.value = AudioListener.volume;

        volumeScrollbar.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float value)
    {
        AudioListener.volume = value;
    }
}
