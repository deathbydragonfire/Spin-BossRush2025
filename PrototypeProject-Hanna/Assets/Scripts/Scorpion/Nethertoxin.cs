using System.Collections;
using UnityEngine;

public class Nethertoxin : MonoBehaviour
{
    [SerializeField] private Vector2 rangeOfDuration;
    private float duration;
    [SerializeField] private float damage;
    [SerializeField] private float damageRate;
    [SerializeField] private float radius;

    private Transform originalParent; // To store the player's original parent

    void Start()
    {
        duration = Random.Range(rangeOfDuration.x, rangeOfDuration.y);
        Destroy(gameObject, duration);
        StartCoroutine(Damage());
    }

    private IEnumerator Damage()
    {
        while (true)
        {
            DealDamage();
            yield return new WaitForSeconds(damageRate);
        }
    }

    private void DealDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            PlayerHealth playerHealth = collider.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                Debug.Log($"Player takes {damage} from Nethertoxin!");
                playerHealth.TakeDamageByCurrentHP(damage);
            }
            else
            {
                Debug.Log($"{collider.name} is NOT the player. No damage applied.");
            }


            // If the player touches the hazard, attach them to the Vinyldisk
            if (collider.CompareTag("Player"))
            {
                Debug.Log("Player touched the hazard. Attaching to the Vinyldisk.");

                Transform record = transform.parent; // Get the parent record (Vinyldisk)
                if (record != null && record.name == "Vinyldisk")
                {
                    originalParent = collider.transform.parent; // Store the player's original parent
                    Transform playerTransform = collider.transform; // Get the Transform of the player
                    playerTransform.SetParent(record); // Attach the player to the record
                    playerTransform.localRotation = Quaternion.identity; // Reset player rotation relative to record

                    // Unparent the player after 0.5 seconds or when touching the Backstop
                    StartCoroutine(UnparentPlayer(playerTransform));
                }
                else
                {
                    Debug.LogError("Vinyldisk reference not found!");
                }
            }
        }
    }

    private IEnumerator UnparentPlayer(Transform playerTransform)
    {
        Debug.Log("UnparentPlayer coroutine started.");
        float timer = 0.5f; // Unparent after half a second

        while (timer > 0f)
        {
            // Check if the player touches the Backstop
            if (Physics.CheckSphere(playerTransform.position, 0.5f, LayerMask.GetMask("Backstop"))) // Ensure "Backstop" is on a specific layer
            {
                Debug.Log("Player hit the Backstop. Unparenting now.");
                break; // Exit early if Backstop is touched
            }

            Debug.Log($"Timer countdown: {timer}");
            timer -= Time.deltaTime;
            yield return null;
        }

        // Unparent the player
        Debug.Log("Unparenting the player.");
        playerTransform.SetParent(null); 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
