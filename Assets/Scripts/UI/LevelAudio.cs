using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAudio : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    void Update()
    {

    }
    public void AudioA()
    {
        audioManager.SetMusicVolume();
    }
}
