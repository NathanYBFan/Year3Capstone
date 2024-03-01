using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoadManager : MonoBehaviour
{
    // Singleton Initialization
    public static LevelLoadManager _Instance;

    #region SerializeFields
    [SerializeField]
    [Foldout("Dependencies"), Tooltip("Loading screen script to call methods on")]
    private LoadingScreen loadingScreen;

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("Bool to tell if a level is loading")]
    private bool isLoadingLevel = false;

    [SerializeField, ReadOnly]
    [Foldout("Stats"), Tooltip("The name of the level currently loaded and in use")]
    private List<string> currentLevelList;

    [SerializeField]
    [Foldout("Stats"), Tooltip("List of level names that are currently in play")]
    private List<string> levelNamesList;
    #endregion

    #region Getters&Setters
    public List<string> LevelNamesList { get { return levelNamesList; } }
    public bool IsLoadingLevel { get { return isLoadingLevel; } }
    #endregion

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
        StartLoadNewLevel(levelNamesList[0], true);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("PersistentScene"));
    }

    // Load a new level method
    public void StartLoadNewLevel(string levelName, bool showLoadingScreen)
    {
        StartCoroutine(LoadLevel(levelName, showLoadingScreen));
    }

    // Load a new level but as a menu additive
    public void LoadMenuOverlay(string menuName)
    {
        SceneManager.LoadScene(menuName, LoadSceneMode.Additive);
        currentLevelList.Insert(0, menuName); // Insert at front
    }

    // Unload a menu
    public void UnloadMenuOverlay(string menuName)
    {
        SceneManager.UnloadSceneAsync(menuName);
        currentLevelList.RemoveAt(0); // Remove first
        // SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentLevelList[0])); // Set as active scene
    }

    // Coroutine to load level properly
    private IEnumerator LoadLevel(string sceneToLoad, bool showLoadingScreen)
    {
        isLoadingLevel = true;
        if (showLoadingScreen)
            loadingScreen.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.25f);

        // Unload all opened scenes (Not the persistent scene
        if (currentLevelList.Count != 0)
        {
            for (int i = 0; i < currentLevelList.Count; i++)
                SceneManager.UnloadSceneAsync(currentLevelList[i]);
            currentLevelList.Clear();
        }

        // Load new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            loadingScreen.UpdateSlider(asyncLoad.progress);
            yield return null;
        }
        loadingScreen.UpdateSlider(1);
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));

        currentLevelList.Add(sceneToLoad);

        // Initialize player etc.
        yield return new WaitForSeconds(1f);
        
        loadingScreen.gameObject.SetActive(false);
        isLoadingLevel = false;

        yield break;
    }

    // Level Reset
    public void ResetLevelAll()
    {
        GameManager._Instance.InGame = true;

        // RESET STAGE   TODO NATHANF: MAKE A RESET STAGE METHOD


        // RESET PLAYERS
        GameManager._Instance.StartNewGame();

        // RESET STATS
        PlayerStatsManager._Instance.ResetStats();
    }

    // New game started
    public void StartNewGame()
    {
        ResetLevelAll();
    }
}
