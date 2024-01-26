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
    [SerializeField, Required] private Slider enemyVolumeSlider;
    [SerializeField, Required] private Slider systemVolumeSlider;

    // Start is called before the first frame update
    void Awake()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        playerVolumeSlider.value = PlayerPrefs.GetFloat("playerVolume");
        enemyVolumeSlider.value = PlayerPrefs.GetFloat("enemyVolume");
        systemVolumeSlider.value = PlayerPrefs.GetFloat("systemVolume");
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
    public void EnemyVolumeChanged()
    {
        PlayerPrefs.SetFloat("enemyVolume", enemyVolumeSlider.value);
        if (Mathf.Log(enemyVolumeSlider.value) * 20f == Mathf.NegativeInfinity)
            audioMixer.SetFloat("Enemy", -80f);
        else
            audioMixer.SetFloat("Enemy", Mathf.Log(enemyVolumeSlider.value) * 20f);
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
