using UnityEngine;
using System.Collections;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEditor.Overlays;

public class ZapRiotController : MonoBehaviour
{
    public GameObject slashHitbox; // Reference to the slash hitbox object
    public float slashActiveTime = 0.5f; // How long the hitbox stays active
    public float staggerTime = 1.5f; // How long Zap Riot is staggered
    private int slashesDodged = 0; // Tracks how many slashes the player dodged
    public UnityEngine.Transform player; // Reference to the player
    public UnityEngine.Transform record; // Reference to the record
    public float moveSpeed = 5f; // Movement speed
    public float heightOffset = 4f;
    private bool isAttacking = false; // To prevent movement while attacking
    public GameObject lightningTrailPrefab; // Lightning trail prefab
    public float trailSpawnInterval = 0.5f; // Time between trail spawns
    public GameObject lightningSpherePrefab; // Prefab for the damaging sphere during hype-up
    public float sphereDuration = 3f; // Duration of the damaging sphere
    public float sphereDamageRate = 0.5f; // How often the sphere deals damage
    public float sphereRadius = 5f; // Radius of the damaging sphere
    public float dashDuration = 5f; // Duration of the speed-up phase
    public float recordSpeedMultiplier = 2f; // Multiplier for the record speed
    public float trailExtendedLifetime = 2f; // Extended lifetime for the lightning trail during the speed-up phase
    public UnityEngine.Transform rightPosition; // Target position for Zap Riot's dash
    public float zapMoveSpeed = 10f; // Movement speed for the dash
    public LayerMask playerLayer; // Layer mask to detect the player
    public float attackDuration = 5f; // How long the speed-up lasts
    private float baseRecordSpeedMultiplier = 1f; // Base speed multiplier for the record
    public UnityEngine.Transform[] lightningZonesUp; // 3 zones where lightning shoots up
    public UnityEngine.Transform[] lightningZonesDown; // 3 zones where lightning lands
    public float soloDuration = 6f; // Total duration of the solo
    public float lightningUpTime = 1f; // Time before lightning crashes down
    public int lightningStrikes = 5; // Number of lightning strikes per sequence
    public float intervalBetweenStrikes = 1.5f; // Time between each strike
    public UnityEngine.Transform backPosition; // Position at the back of the record for the solo
    private bool isPerformingAttack = false;
    private bool zapIsAttacking = false; // To prevent overlapping attacks
    public float lightningDelay = 1f; // Time between each lightning strike
    public GameObject lightningPrefab; // Prefab for the lightning bolt
    public LayerMask recordLayer; // Layer mask for the record
    void Start()
    {
        // Start spawning the lightning trail as Zap Riot moves
        StartCoroutine(SpawnLightningTrail());
    }

    void Update()
    {
        if (isAttacking) return; // Skip movement logic if attacking

        // Perform a raycast to find the height of the record's surface below Zap Riot
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("record")))
        {
            float surfaceY = hit.point.y; // Y-position of the record's surface
            float desiredY = surfaceY + heightOffset; // Keep Zap Riot slightly above the record
            transform.position = new Vector3(transform.position.x, desiredY, transform.position.z);
        }

        // Move Zap Riot toward the player while maintaining height
        Vector3 targetPosition = player.position;
        targetPosition.y = transform.position.y; // Maintain adjusted height
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Trigger slash sequence with key 9 for testing
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            StartCoroutine(PerformSlashSequence());
        }
        {
            if (Input.GetKeyDown(KeyCode.Alpha8) && !isAttacking)
            {
                StartCoroutine(SpeedUpAttack());
            }
        }
        {
            // Trigger the lightning attack with the "7" key for testing
            if (Input.GetKeyDown(KeyCode.Alpha7) && !isPerformingAttack)
            {
                StartCoroutine(LightningAttack());
            }
        }
    }


    private IEnumerator PerformSlashSequence()
    {
        slashesDodged = 0; // Reset the dodge counter at the start of the sequence

        for (int i = 0; i < 3; i++) // Perform 3 slashes
        {
            Debug.Log($"Performing Slash {i + 1}");

            // Move toward the player before slashing
            yield return StartCoroutine(MoveToPlayer());

            // Activate the hitbox
            slashHitbox.SetActive(true);

            yield return new WaitForSeconds(slashActiveTime); // Keep it active for a short time

            // Deactivate the hitbox
            slashHitbox.SetActive(false);

            // Wait before the next slash
            yield return new WaitForSeconds(0.5f);
        }

        // Check if the player dodged all slashes
        if (slashesDodged == 3)
        {
            Debug.Log("Zap Riot is staggered!");
            yield return StartCoroutine(Stagger());
        }
    }


    private IEnumerator MoveToPlayer()
    {
        float moveDuration = 1f; // Max time to move toward the player
        float distanceThreshold = 1.5f; // Stop moving when within this distance
        float desiredY = record.position.y + heightOffset; // Set height to stay above the record

        float timer = 0f;
        while (timer < moveDuration)
        {
            // Keep Zap Riot at the correct height
            transform.position = new Vector3(transform.position.x, desiredY, transform.position.z);

            // Move Zap Riot toward the player
            Vector3 direction = (player.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, player.position);

            // Stop moving if close enough to the player
            if (distance <= distanceThreshold)
            {
                Debug.Log("Zap Riot is close enough to the player.");
                break;
            }

            transform.position += direction * moveSpeed * Time.deltaTime;

            timer += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Zap Riot finished moving toward the player.");
    }
    private IEnumerator SpawnLightningTrail()
    {
        while (true)
        {
            if (!isAttacking) // Only spawn trail when not attacking
            {
                RaycastHit hit;

                // Ensure the raycast looks for the Record layer specifically
                int recordLayer = LayerMask.GetMask("record");

                if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, recordLayer))
                {
                    // Spawn the lightning trail at the hit point
                    Vector3 spawnPosition = hit.point;
                    GameObject trail = Instantiate(lightningTrailPrefab, spawnPosition, Quaternion.identity);

                    // Destroy the trail after a short duration
                    Destroy(trail, 1f); // Adjust the time (e.g., 1 second) as needed
                }
                else
                {
                    Debug.LogWarning($"Record surface not found for trail spawn! Zap Riot position: {transform.position}");
                }
            }
            yield return new WaitForSeconds(trailSpawnInterval);
        }
    }


    private IEnumerator Stagger()
    {
        // Logic for when Zap Riot is staggered
        Debug.Log("Zap Riot is staggered!");
        yield return new WaitForSeconds(staggerTime); // Stagger duration
        Debug.Log("Zap Riot recovered from stagger!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by slash!");
            // Deal damage or apply other effects to the player here
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player dodged the slash!");
            slashesDodged++;
        }
    }



    private IEnumerator SpeedUpAttack()
    {
        isAttacking = true;

        // Step 1: Move to the right position
        Debug.Log("Zap Riot dashes to the right side!");
        while (Vector3.Distance(transform.position, rightPosition.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, rightPosition.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = rightPosition.position; // Snap to position

        // Step 2: Spawn the lightning sphere
        Debug.Log("Zap Riot creates a lightning sphere!");
        GameObject sphere = Instantiate(lightningSpherePrefab, transform.position, Quaternion.identity);
        sphere.transform.localScale = Vector3.one * sphereRadius * 2f; // Scale the sphere to match the radius
        Destroy(sphere, sphereDuration);

        yield return new WaitForSeconds(sphereDuration);

        // Step 3: Speed up the record
        Debug.Log("Zap Riot speeds up the record!");
        MusicHandler musicHandler = FindObjectOfType<MusicHandler>();
        if (musicHandler != null)
        {
            musicHandler.SetBaseMultiplier(recordSpeedMultiplier); // Set the base speed multiplier
        }
        else
        {
            Debug.LogError("MusicHandler not found!");
        }

        yield return new WaitForSeconds(attackDuration);

        // Step 4: Reset the record speed
        Debug.Log("Zap Riot resets the record speed!");
        if (musicHandler != null)
        {
            musicHandler.SetBaseMultiplier(1f); // Reset to normal speed
        }

        isAttacking = false;
    }



   private IEnumerator LightningAttack()
    {
        isAttacking = true;

        // Step 1: Move Zap Riot to the back position, maintaining height
        Debug.Log("Zap Riot moves to the back position for the solo!");
        float snapTimer = 2.5f; // Allow up to 1.5 seconds to reach the position

        while (Vector3.Distance(transform.position, backPosition.position) > 0.1f && snapTimer > 0f)
        {
            MaintainHeightAboveRecord(); // Lock his height during movement
            Vector3 targetPosition = new Vector3(backPosition.position.x, transform.position.y, backPosition.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            snapTimer -= Time.deltaTime; // Decrease the timer
            yield return null;
        }

        // Snap to position if the timer runs out
        transform.position = new Vector3(backPosition.position.x, backPosition.position.y + heightOffset, backPosition.position.z);

        // Step 2: Lightning Solo - Perform the attack sequence
        Debug.Log("Zap Riot performs the lightning solo!");
        float soloTimer = 0f;

        while (soloTimer < soloDuration)
        {
            // Choose a random "up" zone and its corresponding "down" zone
            int index = Random.Range(0, lightningZonesUp.Length);
            UnityEngine.Transform upZone = lightningZonesUp[index];
            UnityEngine.Transform downZone = lightningZonesDown[index];

            if (upZone == null || downZone == null)
            {
                Debug.LogError("Lightning zones are not assigned properly!");
                break;
            }

            // Step 2.1: Launch the lightning from the back zone
            Debug.Log($"Launching lightning from {upZone.position}");
            yield return StartCoroutine(LaunchLightning(upZone.position));

            // Step 2.2: Strike the lightning at the front zone
            yield return new WaitForSeconds(lightningUpTime); // Wait for lightning to "travel"
            Debug.Log($"Striking lightning at {downZone.position}");
            yield return StartCoroutine(LandLightning(downZone.position));

            // Wait before the next lightning strike
            yield return new WaitForSeconds(intervalBetweenStrikes);
            soloTimer += intervalBetweenStrikes + lightningUpTime; // Account for delays
        }

        // Step 3: Reset Behavior
        Debug.Log("Zap Riot ends the lightning solo!");
        isAttacking = false;
    }

    private IEnumerator LaunchLightning(Vector3 startPosition)
    {
        GameObject lightningUp = Instantiate(lightningPrefab, startPosition, Quaternion.identity);

        // Animate the lightning going up
        float travelTime = 0.5f; // Time to travel up
        Vector3 targetPosition = startPosition + Vector3.up * 10f; // Adjust the height for the launch

        float timer = 0f;
        while (timer < travelTime)
        {
            lightningUp.transform.position = Vector3.Lerp(startPosition, targetPosition, timer / travelTime);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure it snaps to the target position at the end
        lightningUp.transform.position = targetPosition;

        Destroy(lightningUp, 1f); // Clean up after it's finished
    }

    private IEnumerator LandLightning(Vector3 targetPosition)
    {
        GameObject lightningDown = Instantiate(lightningPrefab, targetPosition + Vector3.up * 5f, Quaternion.identity);

        // Animate the lightning falling down
        float travelTime = 0.5f; // Time to fall
        Vector3 startPosition = targetPosition + Vector3.up * 5f; // Start above the target

        float timer = 0f;
        while (timer < travelTime)
        {
            lightningDown.transform.position = Vector3.Lerp(startPosition, targetPosition, timer / travelTime);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure it snaps to the target position at the end
        lightningDown.transform.position = targetPosition;

        Destroy(lightningDown, 1f); // Clean up after it's finished
    }



    private void MaintainHeightAboveRecord()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, recordLayer))
        {
            float surfaceY = hit.point.y; // Y-position of the record's surface
            transform.position = new Vector3(transform.position.x, surfaceY + heightOffset, transform.position.z);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        // Visualize the back position in the editor
        if (backPosition != null)
        {
            Gizmos.DrawWireSphere(backPosition.position, 0.5f);
        }

        // Visualize the lightning zones in the editor
        foreach (var zone in lightningZonesUp)
        {
            Gizmos.DrawWireSphere(zone.position, 0.5f);
        }
        foreach (var zone in lightningZonesDown)
        {
            Gizmos.DrawWireSphere(zone.position, 0.5f);
        }
    }
}
