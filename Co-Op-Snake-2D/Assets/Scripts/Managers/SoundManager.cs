using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    public AudioSource soundEffect;
    public AudioSource soundMusic;
    public Sound[] sounds;

    private float currentVolume;
    public bool isMute = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(SoundType.BackgroundMusic);
        currentVolume = soundMusic.volume;
    }

    public void MuteGame()
    {
        isMute = !isMute;
        SetVolume(isMute ? 0.0f : currentVolume);
    }

    public void SetVolume(float newVolume)
    {
        currentVolume = newVolume == 0.0f ? soundMusic.volume : newVolume;
        soundEffect.volume = newVolume;
        soundMusic.volume = newVolume;
    }

    public void PlayMusic(SoundType soundType)
    {
        if (isMute) return;

        AudioClip soundClip = GetSoundClip(soundType);
        if (soundClip != null)
        {
            soundMusic.clip = soundClip;
            soundMusic.Play();
        }
        else
        {
            Debug.LogWarning("Did not find any Sound Clip for selected Sound Type");
        }
    }

    public void PlayEffect(SoundType soundType)
    {
        if (isMute) return;

        AudioClip soundClip = GetSoundClip(soundType);
        if (soundClip != null)
        {
            soundEffect.PlayOneShot(soundClip);
        }
        else
        {
            Debug.LogWarning("Did not find any Sound Clip for selected Sound Type");
        }
    }

    private AudioClip GetSoundClip(SoundType soundType)
    {
        Sound sound = Array.Find(sounds, item => item.soundType == soundType);
        return sound?.soundClip;
    }
}

[Serializable]
public class Sound
{
    public SoundType soundType;
    public AudioClip soundClip;
}

public enum SoundType
{
    ButtonClick,
    ButtonQuit,
    BackgroundMusic,
    SpecialAbilityPickup,
    SnakeHeal,
    SnakeHurt,
    LevelPause,
    LevelOver,
    LevelStart
}
