using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public PlayerMove playerMove;
    public PlayerJump playerJump;
    public PlayerShoot playerShoot;
    public LocomotionSystem currentLocomotionSystem;

    [Header("Player Stats")]
    public int currentHealth;
    public int maxHealth = 100;
    public bool canJump = true;

    private PlayerControls inputActions;

    void Awake()
    {
        // Initialize health
        currentHealth = maxHealth;

        // Initialize the input system
        inputActions = new PlayerControls();
        inputActions.Enable();

        // Get references to all the modular player scripts
        playerMove = GetComponent<PlayerMove>();
        playerJump = GetComponent<PlayerJump>();
        playerShoot = GetComponent<PlayerShoot>();

        // Bind actions to their respective methods
        // inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Shoot.performed += OnShoot;
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    // // This method is automatically called when the Shoot button is pressed.
    // public void OnMove(InputAction.CallbackContext context)
    // {
    //     if (context.performed)
    //     {
    //         playerShoot.Shoot();
    //     }
    // }

    // This method is called by the Input System when the Jump button is pressed.
    public void OnJump(InputAction.CallbackContext context)
    {
        // Only jump if the action was a button press and the player can jump
        if (context.performed && canJump)
        {
            playerJump.Jump();
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Player has died!");
        Destroy(gameObject);
    }

    public void ActivateLocomotion(LocomotionSystem newSystem)
    {
        if (currentLocomotionSystem != null)
        {
            currentLocomotionSystem.Deactivate();
        }
        currentLocomotionSystem = newSystem;
        currentLocomotionSystem.Activate();
    }
}