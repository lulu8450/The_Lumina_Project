using UnityEngine;

// public class Turret : Enemy
// {
//     // TODO : The Turret will use the AI scripts attach to it.
//     // This script going to be used for any unique Turret behavior.
// }

public class Turret : Enemy
{
    // These references are assigned in the Inspector, as shown in your image.
    public RangeDetection rangeDetection;
    public RangeAttackSystem rangeAttackSystem;
    

    private void Awake()
    {
        // Ensure these components exist before we try to use them (optional, since you linked them)
        if (rangeDetection == null || rangeAttackSystem == null)
        {
            Debug.LogError("Turret is missing one or more required AI components! Check the Inspector links.", this);
        }
    }

    private void Update()
    {
        // The core state machine logic
        if (rangeDetection.isTargetDetected)
        {
            // Player is detected: Stop patrolling and attack.
            Attack();
        }
        else {
            // Reset Rotation to Default and heal
            ResetRotation();
            base.FullHeal();

        }
    }

    // --- State Functions ---

    private void Attack()
    {
        // Execute the attack logic defined in the component.
        if (rangeAttackSystem != null)
        {
            rangeAttackSystem.Attack();
            rangeAttackSystem.canRotate = true;
        }
    }
    
    private void ResetRotation()
    {
        rangeAttackSystem.canRotate = false;

    }

    // Override the Die function from Enemy.cs if needed for specific death logic.
    public override void Die()
    {
        // Add Gunner-specific death animation/effects here
        base.Die();
    }
}
