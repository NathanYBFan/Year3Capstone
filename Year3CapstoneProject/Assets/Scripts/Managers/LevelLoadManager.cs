using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoadManager : MonoBehaviour
{
    // Singleton Initialization
    public static LevelLoadManager _Instance;

    // Serialize Fields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("")]
    private LoadingScreen loadingScreen;

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("")]
    private bool isLoadingLevel = false;

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("The name of the level currently loaded and in use")]
    private string currentLevelName;

    [SerializeField]
    [Foldout("Stats"), Tooltip("")]
    private List<string> levelNamesList;
    
    // Getters
    public List<string> LevelNamesList { get { return levelNamesList; } }
    public bool IsLoadingLevel { get { return isLoadingLevel; } }

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

    // Initial Level Load
    private void Start()
    {
        StartLoadLevel(levelNamesList[0]);
    }

    // LoadLevelScript
    public void StartLoadLevel(string levelName)
    {
        StartCoroutine(LoadLevel(levelName));
    }

    // Coroutine to load level properly
    private IEnumerator LoadLevel(string sceneToLoad)
    {
        isLoadingLevel = true;

        loadingScreen.gameObject.SetActive(true);

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
        loadingScreen.UpdateSlider(1);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));

        currentLevelName = sceneToLoad;

        // Initialize player etc.
        yield return new WaitForSeconds(1f);
        // TODO NATHANF: FIX HERE ie. uncomment
        //loadingScreen.gameObject.SetActive(false);
        isLoadingLevel = false;
    }

    // Level Reset
    public void ResetAll()
    {
        // RESET STAGE
        // RESET PLAYERS
        // RESET STATS
    }

    // New game started
    public void StartNewGame()
    {
        ResetAll();
    }
}
