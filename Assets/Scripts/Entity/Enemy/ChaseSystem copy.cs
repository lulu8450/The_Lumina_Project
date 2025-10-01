using UnityEngine;

public class ChaseSystems : MonoBehaviour
{
    public float chaseSpeed = 3f;
    private Enemy enemy;
    private Rigidbody2D rb;

    public Swordsman swordsman;

    void Awake()
    {
        // Get the parent Enemy script and the Rigidbody2D
        enemy = GetComponentInParent<Enemy>();
        rb = GetComponentInParent<Rigidbody2D>();
        swordsman = transform.GetComponent<Swordsman>();

        if (enemy == null || rb == null)
        {
            Debug.LogError("ChaseSystem requires an Enemy script and Rigidbody2D on the parent!", this);
        }
    }

    // Movement must be done in FixedUpdate for Rigidbody
    // void FixedUpdate()
    // {
    //     if (swordsman.patrolOrChase)
    //     {
    //         // ChaseTarget is now called from the parent Swordsman's FixedUpdate, 
    //         // but we'll include the logic here for modularity.
    //         if (enemy.target != null && enabled) // Only move if this component is enabled
    //         {
    //             ChaseTarget();
    //         }
    //         else if (rb != null && enabled)
    //         {
    //             // If enabled but no target, stop moving horizontally
    //             rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    //         }


    //      }

    // }
    
    // ChaseTarget function now applies velocity
    public void ChaseTarget()
    {
        if (enemy.target != null)
        {
            // Calculate direction only on the X-axis for platformer movement
            float directionX = (enemy.target.position.x > transform.position.x) ? 1 : -1;
            
            // Set the horizontal velocity while keeping the vertical velocity (gravity/jumps)
            rb.linearVelocity = new Vector2(directionX * chaseSpeed, rb.linearVelocity.y);
        }
    }
}
