using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    AudioSource audioSource;
    public bool musicIsPlaying = false;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void StartMusic()
    {
       
        if (musicIsPlaying == false)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
            audioSource.Play();
        }
        musicIsPlaying = true;

    }

    public void StopMusic()
    {
        audioSource.Stop();
        musicIsPlaying = false;

    }

    public float GetSongTime()
    {
        return audioSource.time;
    }
}