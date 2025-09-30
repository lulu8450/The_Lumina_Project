using UnityEngine;

// This script should be attached to the projectile prefab.
public class Projectile : MonoBehaviour
{
    // The amount of time (in seconds) before the projectile destroys itself.
    public float lifeTime = 3f; 
    public int damageToDeal = 10; // Damage the projectile will deal
    
    void Start()
    {
        // Start the self-destruct countdown immediately.
        Destroy(gameObject, lifeTime);
    }
    
    // Handles what happens when the projectile hits something (Friendly fire authaurized)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile hit the player
        if (other.CompareTag("Player"))
        {
            // Try to find the PlayerController component
            if (other.TryGetComponent<PlayerController>(out PlayerController player)) 
            {
                player.TakeDamage(damageToDeal);
            }
        }
        // Check if the projectile hit an enemy
        else if (other.CompareTag("Enemy"))
        {
            // Try to find the Enemy component
            if (other.TryGetComponent<Enemy>(out Enemy enemy)) 
            {
                enemy.TakeDamage(damageToDeal);
            }
        }
        
        // Destroy the projectile on contact with anything (Player, Enemy, or environment)
        Destroy(gameObject);
    }
}
