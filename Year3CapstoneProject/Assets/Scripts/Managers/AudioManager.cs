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

    //The list of environment Audio Sources
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("The SFX Audio Sources for environment objects")]
    private List<AudioSource> envAudioSourceList;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("The SFX Audio Source for Mr.20")]
    private AudioSource mrTwentyAudioSource;

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

    //The list of environment SFX
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Sound FX list for the Environment sounds")]
    private List<AudioClip> envAudioList;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Sound FX list for the Chaos Factors")]
    private List<AudioClip> cfAudioList;

    // Mr.20 Voice lines list
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Sound FX list for Mr.20 intro Chaos Factors")]
    private List<AudioClip> mrTwentyChaosFactorList;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Sound FX list for Mr.20 inactive players")]
    private List<AudioClip> mrTwentyInactiveList;

    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Sound FX list for Mr.20 intro Stage")]
    private List<AudioClip> mrTwentyStageIntroList;

    // Music list
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Music track list")]
    private List<AudioClip> musicList;

    // Stats
    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("Display timer that plays a clip when less than 0")]
    private float timerForInactivity;

    [SerializeField]
    [Foldout("Stats"), Tooltip("Max amount of time before a clip plays")]
    private float maxTimeForInactivity;
    #endregion

    #region Getters&Setters
    // Audio Sources
    public List<AudioSource> PlayerAudioSourceList { get { return playerAudioSourceList; } }
    public List<AudioSource> EnvAudioSourceList { get { return envAudioSourceList; } }
    public AudioSource MRTwentyAudioSource { get { return mrTwentyAudioSource; } }
    public AudioSource UIAudioSource { get { return uiAudioSource; } }

    // Player/System/Env/CF Audio Clips
    public List<AudioClip> PlayerAudioList { get { return playerAudioList; } }
    public List<AudioClip> UIAudioList { get { return uiAudioList; } }
    public List<AudioClip> CFAudioList { get { return cfAudioList; } }
    public List<AudioClip> EnvAudioList { get { return envAudioList; } }

    // Mr.20 Audio Clips
    public List<AudioClip> MRTwentyChaosFactorList { get { return mrTwentyChaosFactorList; } }
    public List<AudioClip> MRTwentyInactiveList { get { return mrTwentyInactiveList; } }
    public List<AudioClip> MRTwentyStageIntroList { get { return mrTwentyStageIntroList; } }
    
    // Music Audio Clips
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
        ResetInactivityTimer();
    }

    private void Update()
    {
        if (!GameManager._Instance.InGame)
        {
            timerForInactivity = maxTimeForInactivity;
            return;
        }

        timerForInactivity -= Time.deltaTime;
        if (timerForInactivity <= 0)
        {
            // Play voice line
            AudioClip clipToPlay = mrTwentyInactiveList[Random.Range(0, mrTwentyInactiveList.Count)];
            PlaySoundFX(clipToPlay, mrTwentyAudioSource); 

            // Reset timer
            ResetInactivityTimer();
        }
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
        StartCoroutine(TransitionMusic(musicToPlay, musicAudioSource, 0f));
        if (!musicAudioSource.isPlaying)
            musicAudioSource.Play();
    }
	// Switch music Tracks
	public void PlayMusic(int musicIndex)
	{
        StartCoroutine(TransitionMusic(musicList[musicIndex], musicAudioSource, 0f));
		if (!musicAudioSource.isPlaying)
			musicAudioSource.Play();
	}
	//// Transition Coroutine to switch the music tracks
	//public IEnumerator TransitionMusic(AudioClip musicToPlay, float fadeOutDuration, float fadeInDuration)
	//{
	//    // Local variables
	//    float originalVolume = musicAudioSource.volume;     // Original volume to transition up to
	//    float start = musicAudioSource.volume;              // Volume to change
	//    float currentTime = 0;                              // Timer counter

	//    while (currentTime < fadeOutDuration)
	//    {
	//        currentTime += Time.deltaTime;
	//        musicAudioSource.volume = Mathf.Lerp(start, 0f, currentTime / fadeOutDuration);
	//        yield return null;
	//    }

	//    // Switch Clip
	//    musicAudioSource.clip = musicToPlay;
	//    currentTime = 0;

	//    // Fade in
	//    while (currentTime < fadeInDuration)
	//    {
	//        currentTime += Time.deltaTime;
	//        musicAudioSource.volume = Mathf.Lerp(start, originalVolume, currentTime / fadeInDuration);
	//        yield return null;
	//    }

	//    yield break; // Exit Coroutine
	//}

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

    //Checks all three environment audio sources, and returns the first one that isn't playing
    public AudioSource ChooseEnvAudioSource()
    {
        for (int i = 0; i < envAudioSourceList.Count; i++)
        {
            if (!envAudioSourceList[i].isPlaying)
            {
                return envAudioSourceList[i];
            }
        }
        return null;
    }

    public void ResetInactivityTimer()
    {
        timerForInactivity = maxTimeForInactivity;
    }
}
