using UnityEngine;

// This is an abstract class, serving as a template for other locomotion systems.
public abstract class LocomotionSystem : MonoBehaviour
{
    public abstract void Activate();
    public abstract void Deactivate();
}