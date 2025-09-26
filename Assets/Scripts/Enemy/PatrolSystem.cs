using UnityEngine;

public class PatrolSystem : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed = 2f;
    private int currentPointIndex;

    void Update()
    {
        // Simple patrol logic
        if (patrolPoints.Length > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPointIndex].position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, patrolPoints[currentPointIndex].position) < 0.1f)
            {
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            }
        }
    }
}