using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damageAmount = 10f; // Amount of damage this hazard deals

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has a Health component
        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth != null)
        {
            // Apply damage to the target
            targetHealth.TakeDamage(damageAmount);
        }
    }
}
