using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    [SerializeField] AudioClip playingBgm;
    [SerializeField] AudioClip editingBgm;

    AudioSource bgmAudio;

    void Awake()
    {
        bgmAudio = GetComponent<AudioSource>();

        bgmAudio.clip = editingBgm;
        bgmAudio.Play();
    }

    public void PlayEditingBgm()
    {
        bgmAudio.Stop();
        bgmAudio.clip = editingBgm;
        bgmAudio.Play();
    }

    public void PlayPlayingBgm()
    {
        bgmAudio.Stop();
        bgmAudio.clip = playingBgm;
        bgmAudio.Play();
    }

}
