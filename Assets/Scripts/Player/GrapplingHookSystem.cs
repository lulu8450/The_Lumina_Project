using UnityEngine;

public class GrapplingHookSystem : LocomotionSystem
{
    public override void Activate()
    {
        Debug.Log("Grappling Hook activated!");
        // TODO : Add grappling hook logic here (e.g., shoot hook, start swinging).
    }

    public override void Deactivate()
    {
        Debug.Log("Grappling Hook deactivated!");
        // TODO : Add grappling hook cleanup logic here (e.g., release hook).
    }
}