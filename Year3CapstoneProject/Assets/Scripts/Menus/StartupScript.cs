using UnityEngine;
using UnityEngine.Audio;

namespace UI.SettingsMenu { 
    // TODO: Optimize this script

    public class StartupScript : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        
        private void Start()
        {
            int value = PlayerPrefs.GetInt("windowState", 0); // Save the value to the player prefs
            Screen.fullScreenMode = value switch
            {
                0 => // Borderless
                    FullScreenMode.ExclusiveFullScreen,
                1 => // Windowed
                    FullScreenMode.FullScreenWindow,
                2 => // Fullscreen
                    FullScreenMode.Windowed,
                _ => Screen.fullScreenMode
            };
            bool fullscreen = PlayerPrefs.GetInt("windowState", 1) == 0 || PlayerPrefs.GetInt("windowState", 1) == 1;
            PlayerPrefs.SetInt("resolution", value);
            switch (value)
            {
                case 0:
                    Screen.SetResolution(2560, 1440, fullscreen);
                    break;
                case 1:
                    Screen.SetResolution(1920, 1080, fullscreen);
                    break;
                case 2:
                    Screen.SetResolution(1280, 720, fullscreen);
                    break;
            }
            
            if (PlayerPrefs.GetInt("initialFloat", 0) == 0) {
                audioMixer.SetFloat("Master", 20);
                audioMixer.SetFloat("Music", 20);
                audioMixer.SetFloat("Player", 20);
                audioMixer.SetFloat("System", 20);
                PlayerPrefs.SetInt("initialFloat", 20);
            }

            if (float.IsNegativeInfinity(Mathf.Log(PlayerPrefs.GetFloat("masterVolume")) * 20f))
                audioMixer.SetFloat("Master", -80f);
            else
                audioMixer.SetFloat("Master", Mathf.Log(PlayerPrefs.GetFloat("masterVolume")) * 20f);

            if (float.IsNegativeInfinity(Mathf.Log(PlayerPrefs.GetFloat("musicVolume")) * 20f))
                audioMixer.SetFloat("Music", -80f);
            else
                audioMixer.SetFloat("Music", Mathf.Log(PlayerPrefs.GetFloat("musicVolume")) * 20f);
                
            if (float.IsNegativeInfinity(Mathf.Log(PlayerPrefs.GetFloat("playerVolume")) * 20f))
                audioMixer.SetFloat("Player", -80f);
            else
                audioMixer.SetFloat("Player", Mathf.Log(PlayerPrefs.GetFloat("playerVolume")) * 20f);

            if (float.IsNegativeInfinity(Mathf.Log(PlayerPrefs.GetFloat("systemVolume")) * 20f))
                audioMixer.SetFloat("System", -80f);
            else
                audioMixer.SetFloat("System", Mathf.Log(PlayerPrefs.GetFloat("systemVolume")) * 20f);
            
            Destroy(this.gameObject.GetComponent<StartupScript>());
        }
    }
}