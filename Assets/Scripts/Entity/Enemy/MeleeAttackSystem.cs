using UnityEngine;

public class MeleeAttackSystem : MonoBehaviour
{
    public float attackRange = 1f;
    private Enemy enemy;
    public float attackRate = 0.5f;
    private float nextAttackTime = 0f;

    void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void Attack()
    {
        if (enemy.target != null )
        {
            if (Vector2.Distance(transform.position, enemy.target.position) < attackRange)
            {
                // Damage the player.
                PlayerController player = enemy.target.GetComponent<PlayerController>();
                if (Time.time >= nextAttackTime)
                {
                    player.TakeDamage(enemy.damageAmount);
                    // Add attack cooldown if needed.
                }
                nextAttackTime = Time.time + attackRate;
            }
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(other.CompareTag("Player"))Attack();
    // }
}