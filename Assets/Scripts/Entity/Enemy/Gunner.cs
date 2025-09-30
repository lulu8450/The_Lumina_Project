// using UnityEngine;

// public class Gunner : Enemy
// {
//     // References to the modular AI systems
//     public RangeDetection rangeDetection;
//     public PatrolSystem patrolSystem;
//     public RangeAttackSystem rangeAttackSystem;

//     // A flag to check if the Gunner is currently moving or standing still
//     public bool isPatrolling = true;

//     private void Awake()
//     {
//         // Get the attached AI components
//         rangeDetection = GetComponentInChildren<RangeDetection>();
//         patrolSystem = GetComponentInChildren<PatrolSystem>();
//         rangeAttackSystem = GetComponentInChildren<RangeAttackSystem>();

//         // Ensure these components exist before we try to use them
//         if (rangeDetection == null) Debug.LogError("Gunner is missing the RangeDetection component!", this);
//         if (patrolSystem == null) Debug.LogError("Gunner is missing the PatrolSystem component!", this);
//         if (rangeAttackSystem == null) Debug.LogError("Gunner is missing the RangeAttackSystem component!", this);
//     }

//     private void Update()
//     {
//         // If the player is detected, switch to attack mode.
//         if (rangeDetection.isTargetDetected)
//         {
//             // Stop patrolling
//             isPatrolling = false;

//             // Attack the target
//             Attack();
//         }
//         else
//         {
//             // If the player is not detected, patrol.
//             isPatrolling = true;
//             Patrol();
//         }
//     }

//     private void Patrol()
//     {
//         // This method simply tells the PatrolSystem component to do its job.
//         if (patrolSystem != null)
//         {
//             // Enable the patrol system's update loop.
//             patrolSystem.enabled = true;
//         }
//     }

//     private void Attack()
//     {
//         // This method tells the RangeAttackSystem to fire.
//         if (rangeAttackSystem != null)
//         {
//             // Disable the patrol system so the Gunner stands still to attack.
//             if (patrolSystem != null)
//             {
//                 patrolSystem.enabled = false;
//             }

//             // Tell the RangeAttackSystem to perform an attack
//             rangeAttackSystem.Attack();
//         }
//     }
// }

using UnityEngine;

// The Gunner acts as the controller, orchestrating the actions of its modular systems.
public class Gunner : Enemy
{
    // These references are assigned in the Inspector, as shown in your image.
    public RangeDetection rangeDetection;
    public PatrolSystem patrolSystem;
    public RangeAttackSystem rangeAttackSystem;
    
    // We get the Rigidbody2D to ensure we can stop movement when attacking.
    private Rigidbody2D rb; 

    private void Awake()
    {
        // Simple check to ensure we have the Rigidbody component for physics control
        rb = GetComponent<Rigidbody2D>();

        // Ensure these components exist before we try to use them (optional, since you linked them)
        if (rangeDetection == null || patrolSystem == null || rangeAttackSystem == null)
        {
            Debug.LogError("Gunner is missing one or more required AI components! Check the Inspector links.", this);
        }
    }

    private void Update()
    {
        // The core state machine logic
        if (rangeDetection.isTargetDetected)
        {
            // Player is detected: Stop patrolling and attack.
            StopMovement();
            Attack();
        }
        else
        {
            // Player is not detected: Patrol.
            Patrol();
        }
    }

    // --- State Functions ---

    private void Patrol()
    {
        // Only run the PatrolSystem logic when not attacking.
        if (patrolSystem != null)
        {
            patrolSystem.enabled = true;
        }
    }

    private void Attack()
    {
        // Stop movement immediately when attacking
        StopMovement(); 

        // Execute the attack logic defined in the component.
        if (rangeAttackSystem != null)
        {
            rangeAttackSystem.Attack();
        }
    }
    
    private void StopMovement()
    {
        // Disable Patrol to prevent it from moving the transform.
        if (patrolSystem != null)
        {
            patrolSystem.enabled = false;
        }
        
        // Ensure Rigidbody velocity is zeroed out to prevent sliding (if using physics).
        if (rb != null)
        {
             rb.velocity = Vector2.zero;
        }
    }

    // Override the Die function from Enemy.cs if needed for specific death logic.
    public override void Die()
    {
        // Add Gunner-specific death animation/effects here
        base.Die();
    }
}
