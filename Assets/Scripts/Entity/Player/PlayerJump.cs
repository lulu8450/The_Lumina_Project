using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 3f;
    public float wallJumpForce = 5f;
    private Rigidbody2D rb;
    private PlayerController controller;

    // Ground Check variables
    public Transform groundCheck;   
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    // Wall Jump variables
    public Transform leftWallCheck;
    public Transform rightWallCheck;
    public float wallCheckRadius = 0.2f;
    public LayerMask wallLayer;
    private bool isTouchingLeftWall;
    private bool isTouchingRightWall;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        // Check if the player is touching the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Check for walls on both the left and right sides
        isTouchingLeftWall = Physics2D.OverlapCircle(leftWallCheck.position, wallCheckRadius, wallLayer);
        isTouchingRightWall = Physics2D.OverlapCircle(rightWallCheck.position, wallCheckRadius, wallLayer);
    }

    public void Jump()
    {
        if (controller.canJump)
        {
            if (isGrounded)
            {
                // Standard Ground Jump
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }
            else if (isTouchingLeftWall)
            {
                // Wall Jump from the left side (jump to the right)
                // We clear the current velocity and apply the new one for a snappy jump
                rb.linearVelocity = new Vector2(wallJumpForce, jumpForce*2);
            }
            else if (isTouchingRightWall)
            {
                // Wall Jump from the right side (jump to the left)
                // We clear the current velocity and apply the new one for a snappy jump
                rb.linearVelocity = new Vector2(-wallJumpForce, jumpForce*2);
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
        if (leftWallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(leftWallCheck.position, wallCheckRadius);
        }
        if (rightWallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(rightWallCheck.position, wallCheckRadius);
        }
    }
    
    // Public methods to be called from other scripts
    public bool IsTouchingLeftWall()
    {
        return isTouchingLeftWall;
    }
    public bool IsTouchingRightWall()
    {
        return isTouchingRightWall;
    }
    public bool IsTouchingFloor()
    {
        return isGrounded;
    }
}
