using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] _musicSounds, _sfxSounds;
    public AudioSource _musicSource, _sfxSource, _alternateSource;

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
                PlayMusic("menu");
                break;
            case GameStatus.Playing:
                PlayMusic("playing");
                break;
            case GameStatus.Paused:
                PlayPauseMusic();
                break;
            case GameStatus.GameOver:
                if (_musicSource.isPlaying)
                {
                    _musicSource.Stop();
                    PlaySFX("dead");
                }
                break;
        }
    }

    private void OnGameStatusChanged(GameStatus status)
    {
        MusicSelector (status);
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(_musicSounds, x => x.name == name);

        if (s == null)  
        {
            Debug.Log("Sound not Found");
        }
        else
        {
            SetUpAudioSource(_musicSource,s);
            _musicSource.Play();
            if(_alternateSource.isPlaying)
            {
                StopPauseMusic();
            }
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(_sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not Found");
        }
        else
        {
            SetUpAudioSource (_sfxSource, s);
            _sfxSource.Play();
        }
    }

    public void PlayPauseMusic()
    {
        Sound s = Array.Find(_musicSounds, x => x.name == "pause");

        if (s == null)
        {
            Debug.Log("Sound not Found");
        }
        else
        {
            if (_musicSource.isPlaying)
            {
                PauseMusic();
            }

            if(!_sfxSource.isPlaying)
            {
                SetUpAudioSource(_alternateSource, s);
                _alternateSource.Play();
            }

        }
    }

    public void StopPauseMusic()
    {
        _alternateSource.Stop();


    }

    public void PauseMusic() { _musicSource.Pause(); }
    public void UnPauseMusic() { _musicSource.UnPause(); }

    private void SetUpAudioSource(AudioSource audioSource, Sound sound)
    {
        audioSource.clip = sound.clip;
        audioSource.pitch = sound.pitch;
        audioSource.volume = sound.volume;
    }

    IEnumerator PlaySFXWithDelay(string soundName)
    {
        yield return new WaitForSecondsRealtime(0.3f);
        PlaySFX(soundName);
    }
}
