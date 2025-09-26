using UnityEngine;

public class JetpackSystem : LocomotionSystem
{
    public float fuel = 100f;

    public override void Activate()
    {
        Debug.Log("Jetpack activated!");
        // Add jetpack logic here (e.g., start applying upward force).
    }

    public override void Deactivate()
    {
        Debug.Log("Jetpack deactivated!");
        // Add jetpack cleanup logic here (e.g., stop upward force).
    }
}