using UnityEngine;

// This system represents the default ground movement (running and jumping).
public class JumpSystem : LocomotionSystem
{
    private PlayerJump playerJump;
    private PlayerMove playerMove;
    private Rigidbody2D rb;

    void Awake()
    {
        playerJump = GetComponent<PlayerJump>();
        playerMove = GetComponent<PlayerMove>();
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Activate()
    {
        Debug.Log("Jump System activated! Standard movement enabled.");
        
        // Ensure the components that handle the mechanics are enabled
        playerMove.enabled = true;
        playerJump.enabled = true;
    }

    public override void Deactivate()
    {
        Debug.Log("Jump System deactivated! Standard movement disabled.");
        
        // When deactivated, stop movement and disable the components
        playerMove.enabled = false;
        playerJump.enabled = false;
        
        // Immediately zero out horizontal velocity to prevent sliding
        if (rb != null)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }
}
