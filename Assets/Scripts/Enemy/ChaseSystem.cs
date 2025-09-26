using UnityEngine;

public class ChaseSystem : MonoBehaviour
{
    public float chaseSpeed = 3f;
    private Enemy enemy;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public void ChaseTarget()
    {
        if (enemy.target != null)
        {
            Vector2 direction = (enemy.target.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, enemy.target.position, chaseSpeed * Time.deltaTime);
        }
    }
}