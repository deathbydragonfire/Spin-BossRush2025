using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class VamVamController : MonoBehaviour
{
    // General Settings
    public UnityEngine.Transform player; // Reference to the player's position
    public float vamVamSpeed = 5f; // Speed for Vam-Vam's movement
    public bool isAttacking = false; // Whether Vam-Vam is performing an attack
    public Camera mainCamera;

    // Slash Attack Settings
    public GameObject slashHitboxParent; // Parent object for Slash hitboxes
    public float pauseDuration = 1f; // Pause before attacking
    public float attackCooldown = 2f; // Time between attacks

    // Concert Attack Settings
    public GameObject stageTrussPrefab; // The stage truss prefab
    public UnityEngine.Transform playableArea; // Bounds of the playable area
    public LineRenderer laserPrefab; // Prefab for lasers
    public int totalLasers = 9; // Total lasers to fire
    public float laserFireDelay = 0.5f; // Delay between each laser
    public float trussSpeed = 1f; // Speed at which the truss descends
    public float vamVamStartSpeed = 5f; // Speed for moving to the starting position
    public float vamVamEndSpeed = 8f;   // Speed for moving to the ending position
    public List<GameObject> laserLights;
    public GameObject dimmingPanel;
    public GameObject LaserHitBoxPrefab;
    public GameObject casketPrefab; // Assign the casket prefab in the inspector
    public UnityEngine.Transform recordCenter; // Center point of the record
    public float recordRadius = 5f; // Radius of the playable area


    public GameObject VIPAreaPrefab; // Flat circle object for VIP area
    public float vipDuration = 5f; // How long the VIP area stays active
    public float damagePeriod = 2f; // How long the damage period lasts
    public float vipSpeed = 10f; // Speed at which the VIP area moves around the record
    private bool isVipActive = false; // To track if the attack is active

    public UnityEngine.Transform record; // Reference to the spinning record object




    void Start()
    {
        VIPAreaPrefab.SetActive(false); // Ensure the circle is initially inactive
    }


    void Update()
    {
        // Testing triggers
        if (Input.GetKeyDown(KeyCode.Space)) // Press Space for Slash
        {
            PerformSlashAttack();
        }

        if (Input.GetKeyDown(KeyCode.C)) // Press C for Concert
        {
            PerformConcertAttack();
        }
        if (Input.GetKeyDown(KeyCode.V)) // Press C for Concert
        {
            StartVIPAreaAttack();
        }
    }






    // Slash Attack
    public void PerformSlashAttack()
    {
        if (!isAttacking)
        {
            StartCoroutine(SlashSequence());
        }
    }
    public void StartVIPAreaAttack()
    {
        if (!isAttacking)
        {
            StartCoroutine(VIPAreaSequence());
        }
    }


    private Vector3 GetRandomPositionNearPlayer(float radius)
    {
        // Generate a random angle in radians
        float angle = Random.Range(0f, Mathf.PI * 2f);

        // Generate a random distance within the specified radius
        float distance = Random.Range(0f, radius);

        // Calculate offsets for x and z (circular targeting around player)
        float offsetX = Mathf.Cos(angle) * distance;
        float offsetZ = Mathf.Sin(angle) * distance;

        // Return a new position near the player, keeping the y-axis unchanged
        return new Vector3(player.position.x + offsetX, player.position.y, player.position.z + offsetZ);
    }


    private IEnumerator SlashSequence()
    {
        isAttacking = true;

        // Move in front of the player
        Vector3 attackPosition = player.position + new Vector3(2f, 1f, 0); // Adjust X-offset for "in front"
        while (Vector3.Distance(transform.position, attackPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, attackPosition, vamVamSpeed * Time.deltaTime);
            yield return null;
        }

        // Pause before attacking
        yield return new WaitForSeconds(pauseDuration);

        // Perform two swipes with hitbox activation
        Debug.Log("Swipe 1");
        slashHitboxParent.SetActive(true);
        yield return new WaitForSeconds(0.5f); // Adjust as needed
        slashHitboxParent.SetActive(false);

        Debug.Log("Swipe 2");
        slashHitboxParent.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        slashHitboxParent.SetActive(false);

        // Return to hover position
        HoverInCorner();

        // Cooldown
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    // Hover in the upper corner
    private void HoverInCorner()
    {
        Vector3 hoverPosition = new Vector3(-5f, 5f, 0f); // Adjust position as needed
        transform.position = Vector3.Lerp(transform.position, hoverPosition, Time.deltaTime * vamVamSpeed);
    }

    // Concert Attack
    public void PerformConcertAttack()
    {
        if (!isAttacking)
        {
            StartCoroutine(ConcertSequence());
        }
    }

    private IEnumerator ConcertSequence()
    {
        isAttacking = true;

        // Step 1: Move Vam-Vam to her starting position
        Vector3 startPosition = new Vector3(8.48f, 4.18f, -3.01f);
        while (Vector3.Distance(transform.position, startPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, vamVamSpeed * Time.deltaTime);
            yield return null;
        }

        dimmingPanel.SetActive(true);

        // Step 2: Spawn and descend the truss
        GameObject truss = Instantiate(stageTrussPrefab, new Vector3(0, 10f, 0), Quaternion.identity);

        // Find all laser light children in the instantiated truss
        UnityEngine.Transform[] trussLights = truss.GetComponentsInChildren<UnityEngine.Transform>();
        var activeLaserLights = new List<UnityEngine.Transform>();
        foreach (UnityEngine.Transform child in trussLights)
        {
            if (child.name.Contains("LaserLight")) // Adjust this to match the name of your laser light objects
            {
                activeLaserLights.Add(child);
            }
        }

        // Step 3: Descend the truss
        while (truss.transform.position.y > 6.5f)
        {
            truss.transform.position += Vector3.down * trussSpeed * Time.deltaTime;
            yield return null;
        }

        // Step 4: Start firing lasers and descending Vam-Vam simultaneously
        Coroutine fireLasers = StartCoroutine(FireLasers());
        Coroutine vamVamDescent = StartCoroutine(DescendToSecondPosition(vamVamEndSpeed));
        yield return fireLasers;
        yield return vamVamDescent;

        // Step 5: Vam-Vam pauses briefly
        yield return new WaitForSeconds(2f);

        // Step 6: Move Vam-Vam back to her starting position
        while (Vector3.Distance(transform.position, startPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, vamVamSpeed * Time.deltaTime);
            yield return null;
        }
        dimmingPanel.SetActive(false);

        // Step 7: Make the truss ascend
        yield return StartCoroutine(TrussAscend(truss));

        isAttacking = false;
    }


    private IEnumerator DescendToSecondPosition(float speed)
    {
        Vector3 secondPosition = new Vector3(0.25f, 2.13f, -13.25f);
        while (Vector3.Distance(transform.position, secondPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, secondPosition, speed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator TrussAscend(GameObject truss)
    {
        // Target position off the top of the screen
        Vector3 targetPosition = new Vector3(truss.transform.position.x, 10f, truss.transform.position.z);

        // Move the truss upward
        while (truss.transform.position.y < targetPosition.y)
        {
            truss.transform.position += Vector3.up * trussSpeed * Time.deltaTime;
            yield return null;
        }

        // Destroy the truss once it ascends completely
        Destroy(truss);
    }

    private IEnumerator FireLasers()
    {
        float targetRadius = 3f; // Radius around the player

        if (laserLights == null || laserLights.Count == 0)
        {
            Debug.LogError("LaserLights list is empty or null!");
            yield break;
        }

        for (int i = 0; i < totalLasers; i++)
        {
            // Choose a random laser light as the origin
            int randomIndex = Random.Range(0, laserLights.Count);
            UnityEngine.Transform chosenLaserLight = laserLights[randomIndex].transform;

            // Generate a random position near the player
            Vector3 laserTarget = GetRandomPositionNearPlayer(targetRadius);

            // Create the laser at the origin position
            LineRenderer laser = Instantiate(laserPrefab, chosenLaserLight.position, Quaternion.identity);

            // Set laser appearance (Prefire phase)
            laser.startWidth = 0.1f;
            laser.endWidth = 0.1f;
            laser.startColor = Color.red;
            laser.endColor = Color.red;
            laser.SetPosition(0, chosenLaserLight.position); // Start point
            laser.SetPosition(1, laserTarget);              // End point

            // Show prefire indicator
            Debug.DrawRay(chosenLaserLight.position, (laserTarget - chosenLaserLight.position), Color.red, 1f);
            yield return new WaitForSeconds(laserFireDelay);

            // Activate the laser (yellow phase)
            laser.startColor = Color.yellow;
            laser.endColor = Color.yellow;

            // Spawn a hitbox at the target position
            GameObject hitbox = Instantiate(LaserHitBoxPrefab, laserTarget, Quaternion.identity);

            // Explicitly destroy the hitbox after the duration
            Destroy(hitbox, 0.1f); // Adjust duration as needed

            // Keep the laser active for a short time
            yield return new WaitForSeconds(0.5f);

            // Destroy the laser
            Destroy(laser.gameObject);
        }
    }
    private IEnumerator VIPAreaSequence()
    {
        isVipActive = true;

        // Spawn the VIP Area
        GameObject vipArea = Instantiate(VIPAreaPrefab);
        vipArea.transform.position = record.position + Vector3.up * 0.1f; // Position slightly above the record

        float rotationTime = vipDuration;
        float angle = 0f;

        // VIP Area moves around the record
        while (rotationTime > 0)
        {
            angle += vipSpeed * Time.deltaTime;
            float x = record.position.x + Mathf.Cos(angle) * record.localScale.x / 2f;
            float z = record.position.z + Mathf.Sin(angle) * record.localScale.z / 2f;
            vipArea.transform.position = new Vector3(x, record.position.y + 0.1f, z);

            rotationTime -= Time.deltaTime;
            yield return null;
        }

        // Damage period
        vipArea.GetComponent<Renderer>().material.color = Color.red; // Change color to indicate danger
        float damageTime = damagePeriod;

        while (damageTime > 0)
        {
            if (!IsPlayerInVIPArea(player, vipArea))
            {
                // Deal damage to the player if outside the VIP Area
                player.GetComponent<Health>()?.TakeDamage(10f); // Adjust damage value as needed
            }
            damageTime -= Time.deltaTime;
            yield return null;
        }

        // Clean up VIP area
        Destroy(vipArea);
        isVipActive = false;
    }

    private bool IsPlayerInVIPArea(UnityEngine.Transform player, GameObject vipArea)
    {
        // Get the bounds of the Mesh Collider
        MeshCollider meshCollider = vipArea.GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            Debug.LogError("VIP Area does not have a MeshCollider!");
            return false;
        }

        // Calculate the bounds of the Mesh Collider
        Bounds bounds = meshCollider.bounds;

        // Check if the player's position is within the bounds
        return bounds.Contains(player.position);
    }
}