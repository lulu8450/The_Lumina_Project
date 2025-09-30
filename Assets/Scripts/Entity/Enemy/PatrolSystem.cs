// using UnityEngine;

// public class PatrolSystem : MonoBehaviour
// {
//     public Transform[] patrolPoints;
//     public float moveSpeed = 2f;
//     private int currentPointIndex;

//     void Update()
//     {
//         // Simple patrol logic
//         if (patrolPoints.Length > 0)
//         {
//             transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPointIndex].position, moveSpeed * Time.deltaTime);

//             if (Vector2.Distance(transform.position, patrolPoints[currentPointIndex].position) < 0.1f)
//             {
//                 currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
//             }
//         }
//     }
// }

using UnityEngine;

public class PatrolSystem : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed = 2f;
    public int currentPointIndex;

    private void Start()
    {
        // Ensure the enemy starts at the first patrol point's position
        if (patrolPoints.Length > 0)
        {
            transform.position = patrolPoints[0].position;
        }
    }

    void Update()
    {
        // This script's Update only runs when Gunner.cs enables it.
        // Simple patrol logic
        if (patrolPoints.Length > 0)
        {
            // Move towards the current patrol point
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPointIndex].position, moveSpeed * Time.deltaTime);

            // Check if the enemy has reached the current patrol point
            if (Vector2.Distance(transform.position, patrolPoints[currentPointIndex].position) < 0.1f)
            {
                // Move to the next patrol point
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            }
        }
    }
}
