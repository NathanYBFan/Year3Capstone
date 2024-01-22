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
    private List<string> currentLevelList;

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
        StartLoadNewLevel(levelNamesList[0], true);
    }

    // Load a new level method
    public void StartLoadNewLevel(string levelName, bool showLoadingScreen)
    {
        StartCoroutine(LoadLevel(levelName, showLoadingScreen));
    }

    public void LoadMenuOverlay(string menuName)
    {
        SceneManager.LoadScene(menuName, LoadSceneMode.Additive);
        currentLevelList.Insert(0, menuName); // Insert at front
    }

    public void UnloadMenuOverlay(string menuName)
    {
        SceneManager.UnloadSceneAsync(currentLevelList[0]);
        currentLevelList.RemoveAt(0); // Remove first
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentLevelList[0])); // Set as active scene
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
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneToLoad));

        currentLevelList.Add(sceneToLoad);

        // Initialize player etc.
        yield return new WaitForSeconds(1f);
        
        loadingScreen.gameObject.SetActive(false);
        isLoadingLevel = false;

        yield break;
    }

    // Level Reset
    public void ResetAll()
    {
        GameManager._Instance.inGame = true;

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
