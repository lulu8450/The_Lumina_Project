using UnityEngine;

// The Swordsman is a melee enemy that chases the player and attacks when close.
public class Swordsman : Enemy
{
    // References to the modular AI systems
    public RangeDetection rangeDetection;
    public ChaseSystem chaseSystem;
    public PatrolSystem patrolSystem;
    public MeleeAttackSystem meleeAttackSystem;
    // public GameObject meleeZone;

    [Header("Swordsman Settings")]
    public float stoppingDistance = 1.5f; // How close the Swordsman gets before stopping to attack
    
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // meleeZone = GameObject.Find("MeleeAttack")

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

    private void Update()
    {
        if (target == null) return;

        if (rangeDetection.isTargetDetected)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            // 1. Attack Condition
            if (distanceToTarget <= meleeAttackSystem.attackRange)
            {
                Attack();
                StopMovement();
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
            base.FullHeal();

        }
    }

    private void StopMovement()
    {
        // Stop all components that handle movement
        if (patrolSystem != null) patrolSystem.enabled = false;
        if (chaseSystem != null) chaseSystem.enabled = false;
        if (rb != null) rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    private void Patrol()
    {
        // Swordsman continues patrolling until the player is spotted.
        // StopMovement(); // Ensure other movement systems are off
        if (patrolSystem != null) patrolSystem.enabled = true;
    }

    private void Chase()
    {
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
            // meleeZone.SetActive(true);
        }
    }

    public override void Die()
    {
        // Specific death logic for the Swordsman (e.g., dropping a sword)
        base.Die();
    }
}
