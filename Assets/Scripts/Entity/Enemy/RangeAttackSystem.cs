using UnityEngine;

public class RangeAttackSystem : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float projectileSpeed = 10f; // NEW: Speed for the Rigidbody
    public float attackRange = 7f; // Set a default attack range
    public float rotationSpeed = 5f; // New: Speed at which the fire point rotates
    public bool canRotate;
    private float nextFireTime;
    private Enemy enemy; // Reference to the parent Enemy script

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>(); // Get the parent Enemy script
        if (enemy == null)
        {
            Debug.LogError("RangeAttackSystem must be a child of an Enemy!", this);
        }
    }

    void Update()
    {
        // 1. Check if the enemy has a target and is ready to attack
        if (enemy.target != null)
        {
            // 2. Calculate the direction to the target
            // Use the position of this object (RangeAttack) as the origin
            Vector3 direction = enemy.target.position - transform.position;
            
            // 3. Calculate the angle in degrees
            // Using Mathf.Atan2(y, x) * Mathf.Rad2Deg converts the vector direction to an angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // 4. Create the target rotation 
            // We use angle + 0, which assumes the sprite points right (0 degrees)
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            
            // 5. Smoothly apply the rotation to the RangeAttack GameObject (this transform)
            // If rotationSpeed is 0, this will not rotate!
             if (canRotate) transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
             else transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void Attack()
    {
        if (Time.time >= nextFireTime)
        {
            // Check if there is a target (Player)
            if (enemy.target != null)
            {
                // Calculate the direction towards the target (using the current rotation of the firePoint for accuracy)
                // Note: firePoint should still be aligned with the rotated RangeAttack object.
                Vector2 direction = firePoint.right; // FirePoint.right is the local X axis of the rotated FirePoint

                // Instantiates the projectile
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                
                // Add velocity to the projectile's Rigidbody2D
                if (projectile.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    rb.linearVelocity = direction * projectileSpeed;
                }
                else
                {
                    Debug.LogError("Projectile prefab is missing a Rigidbody2D component!", this);
                }
            }
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
}
