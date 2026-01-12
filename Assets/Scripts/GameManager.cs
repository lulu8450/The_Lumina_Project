using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Singleton Instance
    public static GameManager Instance;

    [Header("Game State")]
    public bool isGamePaused = false;
    
    // UI References
    public GameObject pauseCanvas; 
    public Button resume; 
    public Button option;
    public Button quit;

    [Header("Level Management")]
    public int currentLevel = 0;
    public List<string> levelNames = new List<string> { "MainMenu", "JungleLevel", "VolcanoLevel" };
    
    // --- MAIN MENU BUTTON REFERENCES ---
    private Button mainMenuStart;
    private Button mainMenuQuit;


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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Start the coroutine to find the UI after the scene is fully loaded.
        StartCoroutine(SetupSceneUI());
    }

    private IEnumerator SetupSceneUI()
    {
        // Wait one frame to ensure all scene GameObjects (especially the UI) have finished initializing.
        yield return new WaitForEndOfFrame();
        
        // --- 1. CLEAR PREVIOUS REFERENCES ---
        pauseCanvas = null;
        resume = null;
        option = null;
        quit = null;
        mainMenuStart = null;
        mainMenuQuit = null;

        // --- 2. SETUP UI BASED ON SCENE ---
        string sceneName = SceneManager.GetActiveScene().name;
        
        if (sceneName == "MainMenu")
        {
            SetupMainMenuButtons();
        }
        else // Assume any other scene is a gameplay level with a Pause Menu
        {
            SetupPauseMenu();
        }
        
        // 3. Ensure game state is correct for any scene load
        if (sceneName != "MainMenu")
        {
            // Only resume if we are in a game level
            ResumeGame();
        }
        else
        {
            // Reset timescale for main menu
            Time.timeScale = 1f;
            isGamePaused = false;
        }
    }

    // --- PAUSE MENU SETUP (For JungleLevel, VolcanoLevel, etc.) ---
    private void SetupPauseMenu()
    {
        // Search for the Canvas by name ("Canvas"), or the Panel ("PausePanel") and get its parent.
        GameObject canvasObject = GameObject.Find("Canvas");
        if (canvasObject == null)
        {
            GameObject panelObject = GameObject.Find("PausePanel");
            if (panelObject != null)
            {
                canvasObject = panelObject.transform.parent.gameObject;
            }
        }
        
        pauseCanvas = canvasObject; 
        
        if (pauseCanvas != null)
        {
            Transform pausePanelTransform = pauseCanvas.transform.Find("PausePanel");
            
            if (pausePanelTransform != null)
            {
                resume = pausePanelTransform.Find("Resume")?.GetComponent<Button>();
                option = pausePanelTransform.Find("Option")?.GetComponent<Button>();
                quit = pausePanelTransform.Find("Quit")?.GetComponent<Button>();
                
                // Assign Pause Menu click events
                if (resume != null)
                {
                    resume.onClick.RemoveAllListeners(); 
                    resume.onClick.AddListener(ResumeGame);
                }
                if (quit != null)
                {
                    quit.onClick.RemoveAllListeners();
                    // PAUSE MENU QUIT: Loads the Main Menu (index 0)
                    quit.onClick.AddListener(() => LoadLevel(0)); 
                }
            }
            
            // Deactivate the Canvas to hide the UI initially
            pauseCanvas.SetActive(false); 
        }
    }

    // --- MAIN MENU SETUP (For MainMenu scene) ---
    private void SetupMainMenuButtons()
    {
        // We look for the Main Menu Panel inside the MainMenuCanvas.
        GameObject canvasObject = GameObject.Find("MainMenuCanvas");

        if (canvasObject != null)
        {
            // Find the panel inside the canvas
            Transform panelTransform = canvasObject.transform.Find("Panel");
            
            if (panelTransform != null)
            {
                // Find and assign the buttons using the actual names: PlayButton, QuitButton
                mainMenuStart = panelTransform.Find("PlayButton")?.GetComponent<Button>();
                mainMenuQuit = panelTransform.Find("QuitButton")?.GetComponent<Button>();

                // Assign Main Menu click events
                if (mainMenuStart != null)
                {
                    mainMenuStart.onClick.RemoveAllListeners();
                    // MAIN MENU START: Loads the first game level (index 1: JungleLevel)
                    mainMenuStart.onClick.AddListener(() => LoadLevel(1));
                }
                
                if (mainMenuQuit != null)
                {
                    mainMenuQuit.onClick.RemoveAllListeners();
                    // MAIN MENU QUIT: Quits the application
                    mainMenuQuit.onClick.AddListener(QuitGame);
                }
            }
            else
            {
                Debug.LogWarning("Panel not found inside MainMenuCanvas. Main Menu buttons may not be functional.");
            }
        }
        else
        {
            Debug.LogWarning("MainMenuCanvas not found. Main Menu button setup failed.");
        }
    }


    // --- CORE GAMEPLAY FUNCTIONS ---

    public void PauseGame()
    {
        if (pauseCanvas != null) 
        {
            isGamePaused = true;
            Time.timeScale = 0f;
            pauseCanvas.SetActive(true); 
        }
    }

    public void ResumeGame()
    {
        if (isGamePaused == false && Time.timeScale == 1f) return; 

        isGamePaused = false;
        Time.timeScale = 1f;
        
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelNames.Count)
        {
            Time.timeScale = 1f; 
            currentLevel = levelIndex;
            SceneManager.LoadScene(levelNames[currentLevel]);
        }
        else
        {
            Debug.LogError($"Level index {levelIndex} is out of range. Check levelNames array.");
        }
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