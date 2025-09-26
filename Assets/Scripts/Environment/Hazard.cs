using UnityEngine;

public class Hazard : MonoBehaviour
{
    public enum HazardType { Poison, Lava }
    public HazardType hazardType;
    public int damagePerTick = 10;
    public float damageInterval = 1f;
    private float nextDamageTime;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damagePerTick);
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }
}