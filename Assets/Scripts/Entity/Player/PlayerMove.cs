using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    
    // MAKE THIS PUBLIC SO CLIMBING SYSTEM CAN READ IT
    public Vector2 moveInput { get; private set; } 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Input System callback for movement
    public void OnMove(InputAction.CallbackContext context)
    {
        // Store the input value. The 'context.ReadValue<Vector2>()' reads
        // the current state of the 2D vector (left/right, up/down).
        moveInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        // ONLY Execute if this component is enabled (managed by JumpSystem)
        if (!enabled) return;

        // Get the PlayerJump component to check for wall contact
        PlayerJump playerJump = GetComponent<PlayerJump>();

        // Check if the player is touching a wall
        bool isTouchingWall = playerJump.IsTouchingLeftWall() || playerJump.IsTouchingRightWall();

        // Apply horizontal velocity only if not touching a wall or not trying to move into it
        // We only care about horizontal movement here (moveInput.x)
        if (moveInput.x != 0)
        {
            // Check if attempting to move into a wall
            bool movingIntoWall = (moveInput.x > 0 && isTouchingWall && playerJump.IsTouchingRightWall()) ||
                                  (moveInput.x < 0 && isTouchingWall && playerJump.IsTouchingLeftWall());

            if (!movingIntoWall)
        {
            rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        }
        else
        {
                // If moving into a wall, allow sliding down, but stop horizontal movement
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        else if (playerJump.IsTouchingFloor())
        {
            // If the player releases the key while grounded, stop horizontal movement completely.
            // This is the critical fix for the "stuck" issue when on the ground.
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
}