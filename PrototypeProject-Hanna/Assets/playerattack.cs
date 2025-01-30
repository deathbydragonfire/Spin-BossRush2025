using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackDamage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Hit detected on: {other.gameObject.name}"); // ✅ Check if anything gets hit

        Health enemyHealth = other.GetComponent<Health>();

        if (enemyHealth != null)
        {
            Debug.Log($"Dealing {attackDamage} damage to {other.gameObject.name}");
            enemyHealth.TakeDamage(attackDamage);
        }
        else
        {
            Debug.Log($"{other.gameObject.name} has NO Health script! Attack ignored.");
        }
    }
}
