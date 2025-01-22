using UnityEngine;

public class SlashHitbox : MonoBehaviour
{
    public float damage = 20f; // Damage dealt by the Slash

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter triggered by: " + other.name);

        Health targetHealth = other.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
            Debug.Log("Slash hit: " + other.name);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
    }

    private void Awake()
    {
        // Ensure there's at least one collider on this GameObject or its children
        Collider[] colliders = GetComponentsInChildren<Collider>();
        if (colliders.Length == 0)
        {
            Debug.LogError("No colliders attached to SlashHitbox GameObject or its children!");
        }
    }
}
