using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Player and Music will be centralized here, other audio pieces will be localized to their game objects.

    // Singleton Initialization
    public static AudioManager _Instance;

    // Serialize Fields
    [SerializeField, ReadOnly]
    [Foldout("Dependencies"), Tooltip("Audio Source List of each character, this is propgated within this script")]
    private List<AudioSource> playerAudioSourceList; // TODO NATHANF: Propogate using a call to GameManager

    [SerializeField, Required]
    [Foldout("Dependencies"), Tooltip("The music Audio Source which should be attached to the main camera(s)")]
    private AudioSource musicAudioSource;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Sound FX list for the players")]
    private List<AudioClip> playerAudioList;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Music track list")]
    private List<AudioClip> musicList;

    // Getters
    public List<AudioSource> PlayerAudioSourceList { get { return playerAudioSourceList; } }
    public List<AudioClip> PlayerAudioList { get { return playerAudioList; } }
    public List<AudioClip> MusicList { get { return musicList; } }

    private void Awake()
    {
        if (_Instance != null && _Instance != this) // If another AudioManager exists
        {
            Debug.LogWarning("Destroyed a repeated AudioManager");
            Destroy(this.gameObject);
        }

        else if (_Instance == null) // If no other AudioManager exists
            _Instance = this;
    }

    // Play oneshot soundFX
    public void PlaySoundFX(AudioClip soundFxToPlay, AudioSource audioSourceToPlayFrom)
    {
        audioSourceToPlayFrom.PlayOneShot(soundFxToPlay);
    }

    // Switch music Tracks
    public void PlayMusic(AudioClip musicToPlay)
    {
        StartCoroutine(TransitionMusic(musicToPlay, musicAudioSource, 5f));
        if (!musicAudioSource.isPlaying)
            musicAudioSource.Play();
    }

    // Transition Coroutine to switch the music tracks
    public IEnumerator TransitionMusic(AudioClip musicToPlay, float fadeOutDuration, float fadeInDuration)
    {
        // Local variables
        float originalVolume = musicAudioSource.volume;     // Original volume to transition up to
        float start = musicAudioSource.volume;              // Volume to change
        float currentTime = 0;                              // Timer counter

        while (currentTime < fadeOutDuration)
        {
            currentTime += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(start, 0f, currentTime / fadeOutDuration);
            yield return null;
        }

        // Switch Clip
        musicAudioSource.clip = musicToPlay;
        currentTime = 0;

        // Fade in
        while (currentTime < fadeInDuration)
        {
            currentTime += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(start, originalVolume, currentTime / fadeInDuration);
            yield return null;
        }

        yield break;
    }

    // Transition Coroutine to switch the music tracks
    public IEnumerator TransitionMusic(AudioClip musicToPlay, AudioSource musicSource, float duration)
    {
        // Local variables
        float originalVolume = musicAudioSource.volume;     // Original volume to transition up to
        float start = musicAudioSource.volume;              // Volume to change
        float currentTime = 0;                              // Timer counter

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(start, 0f, currentTime / duration);
            yield return null;
        }

        // Switch Clip
        musicAudioSource.clip = musicToPlay;
        currentTime = 0;

        // Fade in
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(start, originalVolume, currentTime / duration);
            yield return null;
        }

        yield break;
    }
}
