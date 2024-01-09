using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoadManager : MonoBehaviour
{
    public static LevelLoadManager _Instance;

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("")]
    private bool isLoadingLevel = false;
    public bool IsLoadingLevel { get { return isLoadingLevel; } }

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("The name of the level currently loaded and in use")]
    private string currentLevelName;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private List<string> levelNameList;
    public List<string> LevelNames { get { return levelNameList; } }

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
        {
            Debug.LogWarning("Destroyed a repeated LevelLoadManager");
            Destroy(this.gameObject);
        }

        else if (_Instance == null)
            _Instance = this;
    }

    private void Start()
    {
        StartLoadLevel(levelNameList[0]);
    }

    public void StartLoadLevel(string levelName)
    {
        StartCoroutine(LoadLevel(levelName));
    }

    private IEnumerator LoadLevel(string sceneToLoad)
    {
        isLoadingLevel = true;

        // TODO NATHANF: ADD LOADING SCREEN HOOKUP
        //loadingScreen.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.25f);

        // Unload current scene
        if (currentLevelName.Length != 0)
            SceneManager.UnloadSceneAsync(currentLevelName);

        // Load new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            // TODO NATHANF: FIX HERE ie. uncomment
            //loadingScreen.UpdateSlider(asyncLoad.progress);
            yield return null;
        }
        // TODO NATHANF: FIX HERE ie. uncomment
        //loadingScreen.UpdateSlider(1);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));

        currentLevelName = sceneToLoad;

        // Initialize player etc.
        yield return new WaitForSeconds(1f);
        // TODO NATHANF: FIX HERE ie. uncomment
        //loadingScreen.gameObject.SetActive(false);
        isLoadingLevel = false;
    }

    public void ResetAll()
    {
        // RESET STAGE
        // RESET PLAYERS
        // RESET STATS
    }

    public void LoadFromSave(string level)
    {
        StartCoroutine(LoadLevel(level));
    }

    public void StartNewGame()
    {
        ResetAll();
    }
}
