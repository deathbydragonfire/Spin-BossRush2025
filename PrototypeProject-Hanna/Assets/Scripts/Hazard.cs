using UnityEngine;

public class Hazard : MonoBehaviour
{
    // Time before the hazard destroys itself
    public float selfDestructTime = 5f;

    // Tag to identify the player
    public string playerTag = "Player";

    void Start()
    {
        // Automatically destroy the hazard after the specified time
        Invoke(nameof(DestroyHazard), selfDestructTime);
    }

    void OnTriggerEnter(Collider other)
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
}
