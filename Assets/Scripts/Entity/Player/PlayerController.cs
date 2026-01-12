using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public PlayerMove playerMove;
    public PlayerJump playerJump;
    public PlayerShoot playerShoot;
    public GameObject leftFirePoint;
    public GameObject rightFirePoint;
    
    [Header("Locomotion Systems")]
    public LocomotionSystem jumpSystem; 
    public LocomotionSystem climbingSystem;
    public LocomotionSystem jetpackSystem;
    public LocomotionSystem grapplingSystem;
    public LocomotionSystem currentLocomotionSystem;

    [Header("Player Stats")]
    public int currentHealth;
    public int maxHealth = 100;
    public bool canJump = true;

    private PlayerControls inputActions;
    private LocomotionSystem[] locomotionSystems;
    private int currentSystemIndex = 0;
    
    // NEW: Reference to the GameManager singleton
    private GameManager gameManager;

    void Awake()
    {
        // Initialize health
        currentHealth = maxHealth;

        // Initialize Fire Point
        leftFirePoint = GameObject.Find("LeftFirePoint");
        rightFirePoint = GameObject.Find("RightFirePoint");

        // Initialize the input system
        inputActions = new PlayerControls();
        inputActions.Enable();

        // Get references to all the modular player scripts
        playerMove = GetComponent<PlayerMove>();
        playerJump = GetComponent<PlayerJump>();
        playerShoot = GetComponent<PlayerShoot>();

        playerShoot.firePoint = rightFirePoint.transform;
        
        // Get the GameManager singleton instance
        gameManager = GameManager.Instance;

        // --- LOCOMOTION SYSTEM SETUP ---
        // Get the Locomotion System references (must be on the same GameObject)
        jumpSystem = GetComponent<JumpSystem>();
        climbingSystem = GetComponent<ClimbingSystem>();
        jetpackSystem = GetComponent<JetpackSystem>();
        // grapplingSystem = GetComponent<GrapplingSystem>();

        // Store all available systems in an array for sequential switching
        // Order is important: Jump is default (index 0), Climb is index 1
        locomotionSystems = new LocomotionSystem[] { jumpSystem, climbingSystem, jetpackSystem }; 
        
        // Ensure JumpSystem is the default active one at the start
        currentLocomotionSystem = jumpSystem;
        ActivateLocomotion(jumpSystem); // Activates the first system

        // Deactivate all other systems on start to ensure clean state
        foreach (var system in locomotionSystems)
        {
            if (system != jumpSystem)
            {
                system.Deactivate();
            }
        }

        // --- INPUT BINDINGS ---
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Shoot.performed += OnShoot;
        inputActions.Player.Pause.performed += OnPause;
        inputActions.Player.SwitchLeft.performed += OnSwitchLeft;
        inputActions.Player.SwitchRight.performed += OnSwitchRight;
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    // --- INPUT HANDLERS for Locomotion Switching ---
    public void OnSwitchLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Decrement index, wrapping around to the end if necessary (circular array)
            currentSystemIndex = (currentSystemIndex - 1 + locomotionSystems.Length) % locomotionSystems.Length;
            ActivateLocomotion(locomotionSystems[currentSystemIndex]);
        }
    }

    public void OnSwitchRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Increment index, wrapping around to the start if necessary (circular array)
            currentSystemIndex = (currentSystemIndex + 1) % locomotionSystems.Length;
            ActivateLocomotion(locomotionSystems[currentSystemIndex]);
        }
    }


    // --- ACTION HANDLERS ---

    public void OnPause(InputAction.CallbackContext context)
    {
        // FIX: Now references the GameManager singleton and toggles the pause state.
        if (context.performed)
        {
            if (gameManager != null)
            {
                if (gameManager.isGamePaused)
                {
                    gameManager.ResumeGame();
                }
                else
                {
                    gameManager.PauseGame();
                }
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Only the currently active LocomotionSystem handles the Jump action
        if (context.performed)
        {
            // If the current system is JumpSystem, call the physical jump method
            if (currentLocomotionSystem == jumpSystem && canJump)
            {
                playerJump.Jump();
            }
        }
    }

    // This method is automatically called when the Shoot button is pressed.
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerShoot.Shoot();
        }
    }

    // --- CORE GAMEPLAY METHODS ---

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            // Die();
            // gameObject.SetActive(false);
            return;
        }
    }

    public void Die()
    {
        Debug.Log("Player has died!");
        Destroy(gameObject);
    }

    // --- LOCOMOTION ACTIVATION ---

    public void ActivateLocomotion(LocomotionSystem newSystem)
    {
        if (currentLocomotionSystem != null && currentLocomotionSystem != newSystem)
        {
            currentLocomotionSystem.Deactivate();
        }
        
        currentLocomotionSystem = newSystem;
        currentLocomotionSystem.Activate();
    }
}