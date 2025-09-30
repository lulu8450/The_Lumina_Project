using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int damageAmount;
    public Transform target; // The player's transform

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void FullHeal()
    {
        health = maxHealth;
    }

}