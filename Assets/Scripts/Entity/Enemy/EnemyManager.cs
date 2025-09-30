using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemies;
    public Transform playerTarget;

    void Start()
    {
        // Find all enemies in the scene and assign the player as their target.
        enemies = new List<Enemy>(FindObjectsOfType<Enemy>());
        if (playerTarget == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerTarget = playerObject.transform;
            }
        }

        foreach (Enemy enemy in enemies)
        {
            enemy.target = playerTarget;
        }
    }
}
