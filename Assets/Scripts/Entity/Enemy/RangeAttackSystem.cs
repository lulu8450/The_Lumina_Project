using UnityEngine;

public class RangeAttackSystem : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float projectileSpeed = 10f;
    private float nextFireTime;
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>(); // Get the parent Enemy script
        if (enemy == null)
        {
            Debug.LogError("RangeAttackSystem must be a child of an Enemy!", this);
        }
    }

    public void Attack()
    {
        if (Time.time >= nextFireTime)
        {
            // Check if there is a target
            if (enemy.target != null)
            {
                // Calculate the direction towards the target
                Vector2 direction = (enemy.target.position - firePoint.position).normalized;

                // Instantiates the projectile from the fire point.
                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

                // Add velocity to the projectile
                if (projectile.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    rb.velocity = direction * projectileSpeed;
                }
            }
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
}