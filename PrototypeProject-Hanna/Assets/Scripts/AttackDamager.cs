using UnityEngine;

public class AttackDamager : MonoBehaviour
{
    public float attackDamage = 10f;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Hit detected on: {other.gameObject.name} (Layer: {other.gameObject.layer})");

        //  Make sure we are NOT trying to damage the object doing the attack!
        if (other.gameObject == gameObject) return;

        // Check if the target is a boss OR specifically on the "fuckallwalls" layer
        if (other.CompareTag("boss") || other.gameObject.layer == LayerMask.NameToLayer("fuckallwalls"))
        {
            Health targetHealth = other.GetComponent<Health>();

            //  Ignore objects that don’t have Health (this prevents the error)
            if (targetHealth == null)
            {
                Debug.Log($"{other.gameObject.name} has NO Health script! Skipping.");
                return;
            }

            Debug.Log($"Dealing {attackDamage} damage to {other.gameObject.name}");
            targetHealth.TakeDamage(attackDamage);
        }
    }
}
