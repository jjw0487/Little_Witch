using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameAudioClip
{
    public string key;
    public AudioClip value;
}

/// <summary>
/// 보관된 AudioClip을 재생하거나 외부에서 받아 볼륨에 맞게 재생한다.
/// </summary>
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
        // AudioSource 생성
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;

        bgmSource = source;

        for (var i = 0; i < 10; i++)
        {
            sourceList.Add(AddAudioSource());
        }
    }
    public void InitializeSoundManager()
    {
        int _volume = PLoad.Load("AudioVolume", 100);

        mainVolume = _volume * 0.01f;
    }

    public void OnVolumeValueChanged(int _volume)
    {
        mainVolume = _volume * 0.01f;

        bgmSource.volume = mainVolume;

        PSave.Save("AudioVolume", _volume);
    }

    public void PlayBGM(AudioClip clip) // 씬 전환시 씬 매니저에서 클립을 받아 BGM을 재생 
    {
        if (mainVolume <= 0) return;

        bgmSource.Stop();

        bgmSource.clip = clip;
        bgmSource.volume = mainVolume;
        bgmSource.Play();

        bgmSource.loop = true;
    }


    public void Play(string address) // 보관된 음향을 찾아 재생
    {
        if (mainVolume <= 0) return;

        if (string.IsNullOrEmpty(address)) return;

        if (audioClipMap.TryGetValue(address, out AudioClip value))
        {
            // 한 번 string 비교로 발견된 AudioClip은 딕셔너리에 저장하여 다음 검색을 빠르게 한다
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
    public void Play(string address, float volume) // 볼륨 설정이 따로 필요한 클립
    {
        if (mainVolume <= 0) return;

        if (string.IsNullOrEmpty(address)) return;

        if (audioClipMap.TryGetValue(address, out AudioClip value))
        {
            // 한 번 string 비교로 발견된 AudioClip은 딕셔너리에 저장하여 다음 검색을 빠르게 한다
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

    public void Play(AudioClip clip) // 외부에서 클립을 제공
    {
        if (mainVolume <= 0) return;

        sourceList[GetFreeAudioSource()].PlayOneShot(clip, mainVolume);
    }

    public void Play(AudioClip clip, float volume) // 외부에서 제공된 클립이 볼륨 설정이 따로 필요한 경우
    {
        if (mainVolume <= 0) return;

        sourceList[GetFreeAudioSource()].PlayOneShot(clip, mainVolume * volume);
    }

    private int GetFreeAudioSource() // 재생중이지 않은 source를 찾아 반환
    {
        for (int i = 0; i < sourceList.Count; i++)
        {
            if (!sourceList[i].isPlaying) return i;
        }

        sourceList.Add(AddAudioSource());
        return (sourceList.Count - 1);
    }
    private AudioSource AddAudioSource() // soruce가 부족할 시 추가로 생성
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
