using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource,alternateSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MusicSelector(GameManager.gameManager.CurrentGameStatus);
        GameManager.gameManager.OnGameStatusChanged.AddListener(OnGameStatusChanged);
    }

    private void MusicSelector (GameStatus status)
    {
        switch (status)
        {
            case GameStatus.OnMenu:
                PlayMusic("menuMusic");
                break;
            case GameStatus.Playing:
                PlayMusic("playingMusic");
                break;
            case GameStatus.Paused:
                PlayPauseMusic();
                break;
        }
    }

    private void OnGameStatusChanged(GameStatus status)
    {
        MusicSelector (status);
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)  
        {
            Debug.Log("Sound not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
            if(alternateSource.isPlaying)
            {
                StopPauseMusic();
            }
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not Found");
        }
        else
        {
            sfxSource.clip = s.clip;
            sfxSource.Play();
        }
    }

    public void PlayPauseMusic()
    {
        Sound s = Array.Find(musicSounds, x => x.name == "pauseMusic");

        if (s == null)
        {
            Debug.Log("Sound not Found");
        }
        else
        {
            if (musicSource.isPlaying)
            {
                PauseMusic();
            }
            alternateSource.clip = s.clip;
            alternateSource.Play();
        }
    }

    public void StopPauseMusic()
    {
        alternateSource.Stop();


    }

    public void PauseMusic() { musicSource.Pause(); }
    public void UnPauseMusic() { musicSource.UnPause(); }

}
