using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    private float volume = 0.3f;
    private AudioSource musicPlayer;
    public static MusicManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 0.3f);
    }

    private void Start()
    {
        musicPlayer = gameObject.GetComponent<AudioSource>();
        musicPlayer.volume = volume;
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        if (volume > 1f)
        {
            volume = 0;
        }
        
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME,volume);
        PlayerPrefs.Save();
        musicPlayer.volume = volume;
    }

    public float GetVolume()
    {
        return volume;
    }
    
}
