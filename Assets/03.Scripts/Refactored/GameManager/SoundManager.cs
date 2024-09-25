using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameAudioClip
{
    public string key;
    public AudioClip value;
}
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private List<GameAudioClip> audioList;

    private List<AudioSource> sourceList = new List<AudioSource>();

    private AudioSource bgmSource = new AudioSource();

    private Dictionary<string, AudioClip> audioClipMap
        = new Dictionary<string, AudioClip>();

    private float mainVolume;

    private void Awake()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;

        bgmSource = source;

        for (var i = 0; i < 10; i++)
        {
            sourceList.Add(AddAudioSource());
        }
    }

    public void OnVolumeValueChanged(int _volume)
    {
        mainVolume = _volume * 0.01f;

        bgmSource.volume = mainVolume;

        PSave.Save("AudioVolume", _volume);
    }

    public void InitializeSoundManager()
    {
        int _volume = PLoad.Load("AudioVolume", 100);

        mainVolume = _volume * 0.01f;
    }

    public void PlayBGM(AudioClip clip)
    {
        if (mainVolume <= 0) return;

        bgmSource.Stop();

        bgmSource.clip = clip;
        bgmSource.volume = mainVolume;
        bgmSource.Play();

        bgmSource.loop = true;
    }


    public void Play(string address)
    {
        if (mainVolume <= 0) return;

        if (string.IsNullOrEmpty(address)) return;

        if (audioClipMap.TryGetValue(address, out AudioClip value))
        {
            sourceList[GetFreeAudioSource()].PlayOneShot(value, mainVolume);
        }
        else
        {
            if (GetAudioClip(address, out AudioClip clip))
            {
                sourceList[GetFreeAudioSource()].PlayOneShot(clip, mainVolume);

                audioClipMap.Add(address, clip);

            }
        }


    }
    public void Play(string address, float volume) // 볼륨을 따로 설정하는 상황
    {
        if (mainVolume <= 0) return;

        if (string.IsNullOrEmpty(address)) return;

        if (audioClipMap.TryGetValue(address, out AudioClip value))
        {
            sourceList[GetFreeAudioSource()].PlayOneShot(value, mainVolume * volume);
        }
        else
        {
            if (GetAudioClip(address, out AudioClip clip))
            {
                sourceList[GetFreeAudioSource()].PlayOneShot(clip, mainVolume * volume);

                audioClipMap.Add(address, clip);

                Debug.Log("New Clip Added");
            }
        }
    }

    public void Play(AudioClip clip)
    {
        if (mainVolume <= 0) return;

        sourceList[GetFreeAudioSource()].PlayOneShot(clip, mainVolume);
    }

    public void Play(AudioClip clip, float volume) // 볼륨을 따로 설정하는 상황
    {
        if (mainVolume <= 0) return;

        sourceList[GetFreeAudioSource()].PlayOneShot(clip, mainVolume * volume);
    }

    private int GetFreeAudioSource()
    {
        for (int i = 0; i < sourceList.Count; i++)
        {
            if (!sourceList[i].isPlaying) return i;
        }

        sourceList.Add(AddAudioSource());
        return (sourceList.Count - 1);
    }
    private AudioSource AddAudioSource()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        return source;
    }
    public bool GetAudioClip(string address, out AudioClip clip)
    {
        for (int i = 0; i < audioList.Count; i++)
        {
            if (address == audioList[i].key)
            {
                clip = audioList[i].value; return true;
            }
        }

        clip = null; return false;
    }
}
