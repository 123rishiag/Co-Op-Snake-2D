using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton Setup
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } } // Singleton instance accessor.
    #endregion

    #region Audio Components
    public AudioSource soundEffect; // Handles playing sound effects.
    public AudioSource soundMusic; // Handles playing background music.
    #endregion

    #region Sound Configuration
    public Sound[] sounds; // Array of sound configurations.
    private float currentVolume; // Current volume level of the sound manager.
    public bool isMute = false; // Indicates whether the sound is muted.
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        // Ensure only one instance of SoundManager exists.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevent destruction on load.
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate.
        }
    }

    private void Start()
    {
        // Start background music and store current volume at start.
        PlayMusic(SoundType.BackgroundMusic);
        currentVolume = soundMusic.volume;
    }
    #endregion

    #region Volume Control
    public void MuteGame()
    {
        // Toggle mute state and set volume accordingly.
        isMute = !isMute;
        SetVolume(isMute ? 0.0f : GetVolume()); // Set volume to 0 if muted, else restore.
    }

    private float GetVolume()
    {
        // Retrieve the current volume setting.
        return currentVolume;
    }

    public void SetVolume(float newVolume)
    {
        // Set new volume for both music and effects.
        if (newVolume == 0.0f)
        {
            currentVolume = soundMusic.volume; // Store current volume before muting.
        }
        else
        {
            currentVolume = newVolume; // Update current volume to new value.
        }
        soundEffect.volume = newVolume; // Apply to sound effects.
        soundMusic.volume = newVolume; // Apply to background music.
    }
    #endregion

    #region Playback Methods
    public void PlayMusic(SoundType soundType)
    {
        // Play background music if not muted.
        if (isMute) return; // Return if muted.
        AudioClip soundClip = GetSoundClip(soundType);
        if (soundClip != null)
        {
            soundMusic.clip = soundClip;
            soundMusic.Play(); // Play the selected clip as background music.
        }
        else
        {
            Debug.Log("Did not find any Sound Clip for selected Sound Type");
        }
    }

    public void PlayEffect(SoundType soundType)
    {
        // Play a sound effect if not muted.
        if (isMute) return; // Return if muted.
        AudioClip soundClip = GetSoundClip(soundType);
        if (soundClip != null)
        {
            soundEffect.PlayOneShot(soundClip); // Play the sound effect.
        }
        else
        {
            Debug.Log("Did not find any Sound Clip for selected Sound Type");
        }
    }
    #endregion

    #region Utility Methods
    private AudioClip GetSoundClip(SoundType soundType)
    {
        // Find and return the AudioClip corresponding to a SoundType.
        Sound sound = Array.Find(sounds, item => item.soundType == soundType);
        if (sound != null)
        {
            return sound.soundClip; // Return clip if found.
        }
        else
        {
            return null; // Return null if no clip found.
        }
    }
    #endregion
}

[Serializable]
#region Sound Data Model
// Represents a sound item, including its type and associated audio clip.
public class Sound
{
    public SoundType soundType; // Defines the type of sound.
    public AudioClip soundClip; // Holds the audio clip associated with the sound type.
}
#endregion


#region Enums
// Represents different types of sounds used in the game.
public enum SoundType
{
    ButtonClick,            // Sound for button click actions.
    ButtonQuit,             // Sound for quitting the game.
    BackgroundMusic,        // Background music playing during the game.
    SpecialAbilityPickup,   // Sound when an item is picked up.
    SnakeHeal,             // Sound when snake gets healed.
    SnakeHurt,             // Sound when snake gets hurt.
    LevelPause,             // Sound for level pause.
    LevelOver,              // Sound for level over.
    LevelStart              // Sound for the start of a level.
}
#endregion