using UnityEngine;

// The Swordsman is a melee enemy that chases the player and attacks when close.
public class Swordsman : Enemy
{
    // References to the modular AI systems
    public RangeDetection rangeDetection;
    public ChaseSystem chaseSystem;
    public PatrolSystem patrolSystem;
    public MeleeAttackSystem meleeAttackSystem;

    [Header("Swordsman Settings")]
    public float stoppingDistance = 1.5f; // How close the Swordsman gets before stopping to attack
    public float stoppingChaseDist = 4f; // How Far the Swordsman gets before stop chasing
    
    private Rigidbody2D rb;
    private Vector2 chaseAnchorPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Get the attached AI components
        rangeDetection = GetComponentInChildren<RangeDetection>();
        chaseSystem = GetComponentInChildren<ChaseSystem>();
        patrolSystem = GetComponentInChildren<PatrolSystem>();
        meleeAttackSystem = GetComponentInChildren<MeleeAttackSystem>();

        // Basic error checking
        if (rangeDetection == null || chaseSystem == null || meleeAttackSystem == null)
        {
            Debug.LogError("Swordsman is missing one or more required AI components!", this);
        }
    }

    private void Start()
    {
        // Start patrolling immediately to ensure movement begins on spawn.
        Patrol();
    }

    private void Update()
    {
        if (target == null) return;
        
        // If the enemy is too far from its anchor, it stops chasing and returns to Patrol.
        float distanceToAnchor = Vector2.Distance(transform.position, chaseAnchorPosition);
        if (distanceToAnchor > stoppingChaseDist)
        {
            // Set the detection to false, so the logic falls back to the Patrol state
            rangeDetection.isTargetDetected = false;
        }

        if (rangeDetection.isTargetDetected)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            // 1. Attack Condition
            if (distanceToTarget <= meleeAttackSystem.attackRange)
            {
                StopMovement();
                Attack();
            }
            // 2. Chase Condition (Move within a set range)
            else if (distanceToTarget > stoppingDistance)
            {
                Chase();
            }
            // 3. Stop (Close enough but outside attack range, e.g., waiting for cooldown)
            else
            {
                StopMovement();
            }
        }
        else
        {
            // Player is not detected: Patrol and heal
            Patrol();
        }
    }

    private void StopMovement()
    {
        // Stop all components that handle movement
        if (patrolSystem != null) patrolSystem.enabled = false;
        if (chaseSystem != null) chaseSystem.enabled = false;
        if (rb != null) rb.linearVelocity = Vector2.zero; // Use velocity to stop movement instantly.
    }

    private void Patrol()
    {
        // Ensure other movement systems are off
        if (chaseSystem != null) chaseSystem.enabled = false;
        if (patrolSystem != null) patrolSystem.enabled = true;
    }

    private void Chase()
    {
        // This is where the leash anchor is set when the chase begins.
        chaseAnchorPosition = transform.position;

        // Enable Chase and disable Patrol while pursuing the player.
        if (patrolSystem != null) patrolSystem.enabled = false;
        if (chaseSystem != null) chaseSystem.enabled = true;
    }

    private void Attack()
    {
        // Execute the attack logic defined in the component.
        if (meleeAttackSystem != null)
        {
            meleeAttackSystem.Attack();
        }
    }

    public override void Die()
    {
        // Specific death logic for the Swordsman (e.g., dropping a sword)
        base.Die();
    }
}