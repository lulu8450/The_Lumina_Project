using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq; // Added for Dictionary conversion, if needed

public class GameManager : MonoBehaviour
{
    // Singleton Instance
    public static GameManager Instance;

    [Header("Game State")]
    public bool isGamePaused = false;
    
    // Find the Pause Panel by tag/reference, not name, for safety.
    public GameObject pausePanel; 
    
    [Header("Level Management")]
    public int currentLevel = 0;
    
    // Using List<string> for correct C# syntax and simplicity.
    public List<string> levelNames = new List<string> { "MainMenu", "JungleLevel", "VolcanoLevel" };
    
    void Awake()
    {
        // 1. Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void OnEnable()
    {
        // Subscribe to scene loading event to find the panel in the new scene
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Search for the pause panel whenever a new scene loads.
        // We use FindGameObjectWithTag for the pause menu UI.
        GameObject panel = GameObject.FindGameObjectWithTag("PauseMenu");
        
        // This will be null in the MainMenu, which is exactly what we want.
        pausePanel = panel; 

        if (pausePanel != null)
        {
            pausePanel.SetActive(false); 
        }

        // Resume game state just in case timeScale was left at 0 from a prior scene load
        ResumeGame(); 
    }

    public void PauseGame()
    {
        // Only pause if the panel exists in the current scene.
        if (pausePanel != null) 
        {
            isGamePaused = true;
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
        // If pausePanel is null (like in the Main Menu), the game will not pause
    }

    public void ResumeGame()
    {
        // Guard clause for safety
        if (isGamePaused == false && Time.timeScale == 1f) return; 

        isGamePaused = false;
        Time.timeScale = 1f;
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelNames.Count)
        {
            // Ensure time scale is 1.0 before loading the next scene
            Time.timeScale = 1f; 
            currentLevel = levelIndex;
            SceneManager.LoadScene(levelNames[currentLevel]);
        }
        else
        {
            Debug.LogError($"Level index {levelIndex} is out of range. Check levelNames array.");
        }
    }
    
    public void LoadNextLevel()
    {
        int nextLevelIndex = (currentLevel + 1) % levelNames.Count; 
        LoadLevel(nextLevelIndex);
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
