using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Player and Music will be centralized here, other audio pieces will be localized to their game objects.

    // Singleton Initialization
    public static AudioManager _Instance;

    #region SerializeFields
    [SerializeField, ReadOnly]
    [Foldout("Dependencies"), Tooltip("Audio Source List of each character, this is propgated within this script")]
    private List<AudioSource> playerAudioSourceList;

    [SerializeField, Required]
    [Foldout("Dependencies"), Tooltip("The music Audio Source which should be attached to the main camera(s)")]
    private AudioSource musicAudioSource;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("The SFX Audio Source for UI and menu Buttons")]
    private AudioSource uiAudioSource;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Sound FX list for the players")]
    private List<AudioClip> playerAudioList;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Sound FX list for the UI")]
    private List<AudioClip> uiAudioList;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Music track list")]
    private List<AudioClip> musicList;
    #endregion

    #region Getters&Setters
    public List<AudioSource> PlayerAudioSourceList { get { return playerAudioSourceList; } }
    public List<AudioClip> PlayerAudioList { get { return playerAudioList; } }
    public List<AudioClip> UIAudioList { get { return uiAudioList; } }

    public AudioSource UIAudioSource { get { return uiAudioSource; } }
    public List<AudioClip> MusicList { get { return musicList; } }
    #endregion
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

    private void Start()
    {
        ResetAudioSources();
        PlayMusic(musicList[0]);
    }

    // Reset and grab all the Player Audio Sources
    public void ResetAudioSources()
    {
        playerAudioSourceList.Clear();
        foreach(GameObject player in GameManager._Instance.Players)
            playerAudioSourceList.Add(player.GetComponentInChildren<AudioSource>());
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

        yield break; // Exit Coroutine
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
