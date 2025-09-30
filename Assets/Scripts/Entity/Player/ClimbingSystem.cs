using UnityEngine;

public class ClimbingSystem : LocomotionSystem
{
    public LayerMask vineLayer;
    public float climbSpeed = 3f;

    private Rigidbody2D rb;
    private PlayerMove playerMove;
    private Collider2D playerCollider; // New reference for Overlap check
    private bool isClimbing = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMove = GetComponent<PlayerMove>();
        playerCollider = GetComponent<Collider2D>(); // Initialize collider reference
    }

    // Use FixedUpdate for physics movement
    void FixedUpdate()
    {
        if (isClimbing)
        {
            // Get the vertical and horizontal input from the PlayerMove script's exposed property
            float verticalInput = playerMove.moveInput.y;
            float horizontalInput = playerMove.moveInput.x; // New horizontal input variable
            
            // Apply climbing movement. Now includes horizontal input.
            // Note: We use climbSpeed for both axes to keep movement consistent.
            rb.velocity = new Vector2(horizontalInput * climbSpeed, verticalInput * climbSpeed);
        }
    }

    // New check to see if the player is currently overlapping a vine object
    private bool IsTouchingVine()
    {
        if (playerCollider == null) return false;
        
        // Use the player's collider bounds for an accurate check
        Vector2 checkSize = playerCollider.bounds.size * 0.9f;
        
        // Check for overlap at the player's center position
        return Physics2D.OverlapBox(transform.position, checkSize, 0f, vineLayer);
    }
    
    // --- TRIGGER DETECTION FOR CLIMBING STATE ---

    // Automatically set isClimbing when entering a vine trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is on the vine layer AND this system is active
        if (other.gameObject.layer == Mathf.Log(vineLayer.value, 2) && enabled)
        {
            isClimbing = true;
            rb.gravityScale = 0f; // Disable gravity while climbing
            rb.velocity = Vector2.zero; // Stop all momentum
        }
    }
    
    // Keep attached while inside the trigger
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == Mathf.Log(vineLayer.value, 2) && enabled)
        {
            // If the player presses up/down or left/right while touching the vine, ensure climbing state is active
            if (playerMove.moveInput.y != 0 || playerMove.moveInput.x != 0)
            {
                isClimbing = true;
                rb.gravityScale = 0f;
            }
            // If the player lets go of vertical and horizontal input, they should stay attached but stop moving
            else if (isClimbing && playerMove.moveInput.y == 0 && playerMove.moveInput.x == 0)
            {
                rb.velocity = new Vector2(0f, 0f);
            }
        }
    }

    // Automatically exit climbing state when leaving a vine trigger
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == Mathf.Log(vineLayer.value, 2) && enabled)
        {
            isClimbing = false;
            rb.gravityScale = 1f; // Re-enable gravity
        }
    }

    // --- LOCOMOTION ACTIVATION/DEACTIVATION ---

    public override void Activate()
    {
        this.enabled = true;
        Debug.Log("Climbing System activated! Try pressing UP/DOWN/LEFT/RIGHT near a vine.");
        
        // PlayerMove component remains disabled to prevent it from overriding horizontal velocity
        // in its FixedUpdate, but we still read its moveInput property.
        // if (playerMove != null) playerMove.enabled = false;
        
        // FIX: If the player is overlapping a vine upon activation, start climbing immediately
        if (IsTouchingVine())
        {
             isClimbing = true;
             rb.gravityScale = 0f;
             rb.velocity = Vector2.zero;
        }
    }

    public override void Deactivate()
    {
        this.enabled = false;
        Debug.Log("Climbing System deactivated!");
        
        // Re-enable PlayerMove component so standard movement works when switching back
        if (playerMove != null) playerMove.enabled = true;

        // Reset gravity and velocity upon deactivation
        if (rb != null)
        {
            rb.gravityScale = 1f;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
        isClimbing = false;
    }
}
