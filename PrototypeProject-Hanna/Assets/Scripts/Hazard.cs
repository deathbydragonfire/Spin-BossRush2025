using UnityEngine;

public class Hazard : MonoBehaviour
{
    // Default time before the hazard destroys itself
    public float selfDestructTime = 5f;

    // Tag to identify the player
    public string playerTag = "Player";

    private void Start()
    {
        // Automatically destroy the hazard after the default time
        Invoke(nameof(DestroyHazard), selfDestructTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag(playerTag))
        {
            DestroyHazard();
        }
    }

    // Public method to manually destroy the hazard
    public void DestroyHazard()
    {
        Destroy(gameObject);
    }

    // Public method to set the hazard's self-destruct time
    public void SetKillTime(float time)
    {
        // Cancel any existing destruction calls
        CancelInvoke(nameof(DestroyHazard));

        // Set the new destruction time
        selfDestructTime = time;

        // Schedule the new destruction time
        Invoke(nameof(DestroyHazard), selfDestructTime);
    }
}
