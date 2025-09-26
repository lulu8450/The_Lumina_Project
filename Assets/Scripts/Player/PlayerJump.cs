using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 3f;
    private Rigidbody2D rb;
    private PlayerController controller;
    
    // New Ground Check variables
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    // New Wall Jump variables
    public Transform wallCheck;
    public float wallCheckRadius = 0.2f;
    public LayerMask wallLayer;
    public float wallJumpForce = 5f;
    private bool isTouchingWall;
    private int wallJumpDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }
        void Update()
    {
        // Check if the player is touching the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Check if the player is touching a wall
        isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);

        // Determine the direction for a wall jump
        if (isTouchingWall)
        {
            wallJumpDirection = -Mathf.RoundToInt(rb.velocity.x);
        }
    }

    public void Jump()
    {  
        // The player has the jump ability
        if (controller.canJump)
        {
            if (isGrounded) // The player is grounded  
            {
                // Standard Ground Jump
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }
            else if (isTouchingWall) // The player is on wall 
            {
                // Wall Jump
                rb.velocity = new Vector2(wallJumpDirection * wallJumpForce, jumpForce);
            }
        }
    }

    // Visualization for the Unity Editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
        }
    }

}