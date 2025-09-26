using UnityEngine;

public class ClimbingSystem : LocomotionSystem
{
    public override void Activate()
    {
        Debug.Log("Climbing System activated!");
        // Add climbing logic here (e.g., snap to wall, enable vertical movement).
    }

    public override void Deactivate()
    {
        Debug.Log("Climbing System deactivated!");
        // Add climbing cleanup logic here (e.g., detach from wall).
    }
}