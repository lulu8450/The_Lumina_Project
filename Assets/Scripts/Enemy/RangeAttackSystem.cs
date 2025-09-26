using UnityEngine;

public class RangeAttackSystem : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float nextFireTime;

    public void Attack()
    {
        if (Time.time >= nextFireTime)
        {
            // Instantiates the projectile from the fire point.
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
}