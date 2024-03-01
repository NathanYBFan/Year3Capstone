using UnityEngine;
using TMPro;
public class VideoSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        QualitySettings.vSyncCount = 0; // VSync turned off
        WindowStateChanged(PlayerPrefs.GetInt("windowState", 1));
        ResolutionChanged(PlayerPrefs.GetInt("resolution", 2));
    }
    public void WindowStateChanged(TMP_Dropdown dropDown)
    {
        WindowStateChanged(dropDown.value);
    }
    public void WindowStateChanged(int value)
    {
        PlayerPrefs.SetInt("windowState", value); // Save the value to the player prefs
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
    }
    
    public void ResolutionChanged(TMP_Dropdown dropdown)
    {
        ResolutionChanged(dropdown.value);
    }
    public void ResolutionChanged(int value)
    {
        // We need to check if the window state is fullscreen, as we need to pass that to the Screen.SetResolution method
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
    }
}
