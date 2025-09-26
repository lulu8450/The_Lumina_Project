using UnityEngine;

public class RangeDetection : MonoBehaviour
{
    public float detectionRange = 5f;
    public bool isTargetDetected = false;
    private Enemy enemy;

    void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    void Update()
    {
        if (enemy.target != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, enemy.target.position);
            isTargetDetected = distanceToTarget < detectionRange;
        }
    }
}