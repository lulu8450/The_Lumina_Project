using UnityEngine;

public class MeleeAttackSystem : MonoBehaviour
{
    public float attackRange = 1f;
    private Enemy enemy;

    void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void Attack()
    {
        if (enemy.target != null)
        {
            if (Vector2.Distance(transform.position, enemy.target.position) < attackRange)
            {
                // Damage the player.
                PlayerController player = enemy.target.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(enemy.damageAmount);
                    // Add attack cooldown if needed.
                }
            }
        }
    }
}