
using UnityEngine;

public class LaserHitbox : MonoBehaviour
{
    public float damage = 10f; // Damage dealt by the laser
    public float duration = 0.05f;                           

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Player hit by laser!");
            }
        }
    }

    private void Start()
    {
        Destroy(gameObject, duration); // Auto-destroy after duration
    }
}
