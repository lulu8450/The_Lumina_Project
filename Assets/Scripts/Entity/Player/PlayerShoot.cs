using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f; // NEW: Speed for the Rigidbody
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    public void Shoot()
    {
        // if (Time.time >= nextFireTime)
        // {
        //     Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        //     nextFireTime = Time.time + fireRate;
        // }
        if (Time.time >= nextFireTime)
        {
            Vector2 direction = firePoint.right;
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
            nextFireTime = Time.time + fireRate;
        }
    }
}