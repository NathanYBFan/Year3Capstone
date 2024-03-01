using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using NaughtyAttributes;
public class AudioSettings : MonoBehaviour
{
    [Header("Audio Initializations")]
    [SerializeField, Required] private AudioMixer audioMixer;
    [SerializeField, Required] private Slider masterVolumeSlider;
    [SerializeField, Required] private Slider musicVolumeSlider;
    [SerializeField, Required] private Slider playerVolumeSlider;
    [SerializeField, Required] private Slider systemVolumeSlider;

    // Start is called before the first frame update
    void Awake()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume", 20);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 20);
        playerVolumeSlider.value = PlayerPrefs.GetFloat("playerVolume", 20);
        systemVolumeSlider.value = PlayerPrefs.GetFloat("systemVolume", 20);
    }

    public void MasterVolumeChanged()
    {
        PlayerPrefs.SetFloat("masterVolume", masterVolumeSlider.value);
        if (Mathf.Log(masterVolumeSlider.value) * 20f == Mathf.NegativeInfinity)
            audioMixer.SetFloat("Master", -80f);
        else
            audioMixer.SetFloat("Master", Mathf.Log(masterVolumeSlider.value) * 20f);
    }
    public void MusicVolumeChanged()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
        if (Mathf.Log(musicVolumeSlider.value) * 20f == Mathf.NegativeInfinity)
            audioMixer.SetFloat("Music", -80f);
        else
            audioMixer.SetFloat("Music", Mathf.Log(musicVolumeSlider.value) * 20f);
    }
    public void PlayerVolumeChanged()
    {
        PlayerPrefs.SetFloat("playerVolume", playerVolumeSlider.value);
        if (Mathf.Log(playerVolumeSlider.value) * 20f == Mathf.NegativeInfinity)
            audioMixer.SetFloat("Player", -80f);
        else
            audioMixer.SetFloat("Player", Mathf.Log(playerVolumeSlider.value) * 20f);
    }
    public void SystemVolumeChanged()
    {
        PlayerPrefs.SetFloat("systemVolume", systemVolumeSlider.value);
        if (Mathf.Log(systemVolumeSlider.value) * 20f == Mathf.NegativeInfinity)
            audioMixer.SetFloat("System", -80f);
        else
            audioMixer.SetFloat("System", Mathf.Log(systemVolumeSlider.value) * 20f);
    }
}
