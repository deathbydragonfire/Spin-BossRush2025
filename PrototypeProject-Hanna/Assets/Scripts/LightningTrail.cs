using UnityEngine;

public class LightningTrail : MonoBehaviour
{
    public float duration = 1.5f; // Default duration for the trail
    public float damage = 10f; // Damage dealt to the player
    private float originalDuration; // To store the original duration for resetting

    void Start()
    {
        originalDuration = duration; // Store the initial duration
        Destroy(gameObject, duration); // Destroy after the duration ends
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched the lightning trail!");
            other.GetComponent<Health>()?.TakeDamage(damage); // Apply damage to the player
        }
    }

    // Method to dynamically update the trail's lifetime
    public void SetLifetime(float newDuration)
    {
        duration = newDuration;

        // Destroy the trail after the new duration
        CancelInvoke(nameof(DestroyTrail));
        Invoke(nameof(DestroyTrail), duration);
    }

    // Method to reset the trail's lifetime to the original duration
    public void ResetLifetime()
    {
        SetLifetime(originalDuration);
    }

    private void DestroyTrail()
    {
        Destroy(gameObject);
    }
}
