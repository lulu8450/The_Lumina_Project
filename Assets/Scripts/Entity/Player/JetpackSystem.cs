using UnityEngine;
using UnityEngine.InputSystem;

public class JetpackSystem : LocomotionSystem
{
    [Header("Jetpack Stats")]
    public float maxFuel = 5f;
    public float thrustForce = 8f;
    public float fuelConsumptionRate = 1f; // Fuel consumed per second
    public float fuelRechargeRate = 2f;    // Fuel recharged per second
    public float jetpackHorizontalSpeed = 6f; // New: Speed for horizontal movement

    [SerializeField] private float currentFuel;

    private Rigidbody2D rb;
    private PlayerMove playerMove;
    private PlayerControls inputActions; // NEW: Local reference to the Input Action Asset
    
    // New: Input tracking for Jump and Move actions
    private float horizontalInput;
    private bool isThrusting = false; // Tracks if Jump key is held

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMove = GetComponent<PlayerMove>();
        currentFuel = maxFuel;
        
        // FIX: Instantiate the Input Action Asset to access its actions
        inputActions = new PlayerControls();

        // IMPORTANT: Bind the Jump action directly to this component when active
        // This is necessary because PlayerController only handles the Jump input if it's the JumpSystem.
        inputActions.Player.Jump.performed += OnJumpInput;
        inputActions.Player.Jump.canceled += OnJumpInput;

        // Ensure the input actions are enabled so the callbacks work
        inputActions.Enable();
    }

    // New: Handle Jump input for thrusting
    private void OnJumpInput(InputAction.CallbackContext context)
    {
        // Only set isThrusting if the JetpackSystem component is currently enabled
        if (enabled)
        {
            isThrusting = context.performed;
        }
    }

    void OnDestroy()
    {
        // Clean up input binding and disable the actions when the component is destroyed
        if (inputActions != null)
        {
            inputActions.Player.Jump.performed -= OnJumpInput;
            inputActions.Player.Jump.canceled -= OnJumpInput;
            inputActions.Disable();
            inputActions.Dispose(); // Recommended to release resources
        }
    }

    void Update()
    {
        // Get continuous horizontal input from the PlayerMove component
        horizontalInput = playerMove.moveInput.x;

        // Recharge logic
        // Recharge if not actively thrusting AND fuel is not full
        if (currentFuel < maxFuel && !isThrusting)
        {
            currentFuel += fuelRechargeRate * Time.deltaTime;
            currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
        }

        // Optional: Provide UI feedback based on fuel level here
    }

    void FixedUpdate()
    {
        // 1. Horizontal Movement (always active in Jetpack mode)
        // We set velocity directly for responsive movement.
        rb.linearVelocity = new Vector2(horizontalInput * jetpackHorizontalSpeed, rb.linearVelocity.y);


        // 2. Vertical Thrusting (using the Jump key)
        // Check if the player is holding Jump and has fuel
        if (isThrusting && currentFuel > 0)
        {
            // Apply thrust force
            rb.AddForce(Vector2.up * thrustForce, ForceMode2D.Force);

            // Consume fuel
            currentFuel -= fuelConsumptionRate * Time.deltaTime;
            currentFuel = Mathf.Max(0f, currentFuel);
            
            // Set gravity to zero while thrusting for consistent ascent
            rb.gravityScale = 0f; 
        }
        else
        {
            // If out of fuel or not pressing Jump, re-enable gravity 
            rb.gravityScale = 1f;
        }
    }

    public override void Activate()
    {
        this.enabled = true;
        Debug.Log("Jetpack System activated!");

        // Disable PlayerMove and PlayerJump
        // if (playerMove != null) playerMove.enabled = false;
        if (GetComponent<PlayerJump>() != null) GetComponent<PlayerJump>().enabled = false;

        // Re-enable local input actions when this system activates
        if (inputActions != null) inputActions.Enable();

        // Reset state on activation
        isThrusting = false;
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0f, 0f); // Start with no momentum
            rb.gravityScale = 1f; // Start with gravity on, let FixedUpdate manage it
        }
    }

    public override void Deactivate()
    {
        this.enabled = false;
        Debug.Log("Jetpack System deactivated!");
        
        // Disable local input actions when this system deactivates
        if (inputActions != null) inputActions.Disable();

        // Reset thrusting state immediately on deactivate
        isThrusting = false; 

        // Re-enable PlayerMove/Jump 
        if (playerMove != null) playerMove.enabled = true;
        if (GetComponent<PlayerJump>() != null) GetComponent<PlayerJump>().enabled = true;

        // Ensure gravity is restored for normal physics
        if (rb != null)
        {
            rb.gravityScale = 1f;
        }
    }
}
