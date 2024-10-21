using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Threading;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource bgmSource, sfxSource;
    [Header("Add sound clips here!")]
    public Sound[] bgmSounds, sfxSounds;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        PlayMusic("base", true);
    }

    public void PlayMusic(string name, bool isbgm)
    {
        Sound[] sounds = isbgm ? bgmSounds : sfxSounds;

        Sound s = Array.Find(sounds, sound => sound.name == name);

        if(s == null)
        {
            Debug.Log("Sound: " + name + " Not Found");
            return;
        }

        AudioSource source = isbgm ? bgmSource : sfxSource;

        if (source == null)
        {
            Debug.LogError("AudioSource not assigned.");
            return;
        }

        source.clip = s.clip;
        source.Play();
    }

    public void StartBGM()
    {
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void ToggleBGM()
    {
        bgmSource.mute = !bgmSource.mute;
    }

    public void BGMVolume(float volume)
    {
        Debug.Log(volume);
        bgmSource.volume = volume;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
    
}
