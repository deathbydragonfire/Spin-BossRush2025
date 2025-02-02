using UnityEngine;
using System.Collections;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DJEmperorController : MonoBehaviour
{
    public UnityEngine.Transform player; // Reference to the player
    public float slamRadius = 3f; // Radius of the slam attack
    public float slamDamage = 20f; // Damage dealt by the slam
    public float proximityTime = 4f; // Time the player must stay close to trigger the slam
    public float slamDelay = 1f; // Delay before the slam happens
    public GameObject slamEffectPrefab; // Optional: Visual effect for the slam
    public float eraSlamRadius = 5f; // Radius of the ERA-Slam attack
    public float eraSlamDamage = 25f; // Damage dealt by the ERA-Slam
    public float eraSlamDelay = 1f; // Delay before the ERA-Slam happens
    public GameObject eraSlamEffectPrefab; // Visual effect for the ERA-Slam
    public float eraCooldown = 5f; // Cooldown before ERA-ERA can be used again
    private Animator Dj_Emperor;

    private float playerProximityTimer = 0f; // Tracks how long the player has been close
    private bool isSlamming = false; // Prevents multiple slams at once

    public GameObject stingPrefab; // Prefab for the sting object
    public float stingRange = 5f; // Range within which the sting can target the player
    public float stingDamage = 15f; // Damage dealt by the sting
    public float stingCooldown = 4f; // Cooldown between sting attacks
    public float stingSpeed = 10f; // Speed at which the sting moves
    private bool canSting = true; // Cooldown tracker
    public float stingAttackRange = 5f; // Adjust this as neede

    public GameObject eraStingPrefab; // Prefab for the ERA-Sting object
    public float eraStingRange = 5f; // Range within which the ERA-Sting can target the player
    public float eraStingDamage = 20f; // Damage dealt by the ERA-Sting
    public float eraStingSpeed = 15f; // Speed at which the ERA-Sting moves

    public GameObject poisonProjectilePrefab; // Assign in Inspector
    public UnityEngine.Transform poisonSpawnPoint; // Assign where the projectile should start
    public float poisonCooldown = 5f; // Cooldown between poison attacks
    private bool canPoison = true; // Cooldown tracker
    public float eraStartDelay = 3f; // How long before he can use ERA ERA
    public float trackStartTime; // When his track started



    public float pushPullDuration = 3f; // Duration of the push/pull effect
    public float pushPullStrength = 10f; // Strength of the push/pull effect

    private bool isPerformingERA = false;
    private bool isStinging = false;

    void Start()
    {
        
        Dj_Emperor = GetComponentInChildren<Animator>(); // Get animator from sprite child object
    }

    void Update()
    {
        CheckPlayerProximity();

        // Press "K" to test the sting attack
        if (Input.GetKeyDown(KeyCode.K))
        {
            PerformSting();
        }

        // Press "L" to test ERA-ERA
        if (Input.GetKeyDown(KeyCode.L))
        {
            PerformERAERA();
        }
    }
    void Awake()
    {
        if (GetComponent<DJLogic>() == null)
        {
            Debug.LogWarning("[DJLogic] Missing on DJ Emperor! Adding dynamically.");
            gameObject.AddComponent<DJLogic>(); //  Adds it at runtime
        }
    }

    public void PerformSting()
    {
        if (canSting && Vector3.Distance(transform.position, player.position) <= stingRange)
        {
            StartCoroutine(StingRoutine());
        }
    }

    public void PerformERAERA()
    {
        if (!isPerformingERA)
        {
            StartCoroutine(ERAERASequence());
        }
    }

    private void CheckPlayerProximity()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If the player is within the slam radius
        if (distanceToPlayer <= slamRadius && !isSlamming)
        {
            playerProximityTimer += Time.deltaTime;

            if (playerProximityTimer >= proximityTime)
            {
                StartCoroutine(PerformSlam());
            }
        }
        else
        {
            // Reset the timer if the player moves out of range
            playerProximityTimer = 0f;
        }
    }
    public void PerformPoisonAttack()
    {
        if (canPoison)
        {
            StartCoroutine(PoisonAttackRoutine());
        }
    }

    public IEnumerator PoisonAttackRoutine()
    {
        canPoison = false;

        int poisonCount = Random.Range(3, 6); // Fire between 3-5 projectiles

        for (int i = 0; i < poisonCount; i++)
        {
            if (poisonProjectilePrefab != null && poisonSpawnPoint != null)
            {
                Debug.Log($"DJ EMPEROR fires poison shot {i + 1}");

                // Spawn poison projectile
                GameObject poisonProjectile = Instantiate(poisonProjectilePrefab, poisonSpawnPoint.position, Quaternion.identity);

                // Give it a random spread angle for variety
                float spreadAngle = Random.Range(-15f, 15f);
                poisonProjectile.transform.Rotate(0, spreadAngle, 0);

                // Ensure it has a script to handle its movement
                NethertoxinProjectile projectile = poisonProjectile.GetComponent<NethertoxinProjectile>();
                if (projectile != null)
                {
                    projectile.enabled = true; // Make sure it's active
                }
                else
                {
                    Debug.LogError("NethertoxinProjectile script missing on poisonPrefab!");
                }

                yield return new WaitForSeconds(0.3f); // Short delay between each shot
            }
            else
            {
                Debug.LogError("Poison prefab or spawn point not assigned!");
            }
        }

        // Wait before DJ EMPEROR can use poison again
        yield return new WaitForSeconds(poisonCooldown);
        canPoison = true;
    }

    private void AdjustRecordSpeed(float adjustment)
    {
        MusicHandler musicHandler = FindObjectOfType<MusicHandler>();
        if (musicHandler != null)
        {
            musicHandler.AdjustPlaybackAndSpinSpeed(adjustment);
        }
        else
        {
            Debug.LogError("MusicHandler not found!");
        }
    }

    private IEnumerator PerformSlam()
    {
        isSlamming = true;
        Dj_Emperor.SetBool("isSlamming", true);
        Dj_Emperor.Play("dj_slam");
        // Optional: Play telegraph animation or sound
        Debug.Log("Slam telegraph!");

        yield return new WaitForSeconds(slamDelay);

        // Perform the slam
        Debug.Log("Slam attack!");

        // Spawn slam effect
        if (slamEffectPrefab != null)
        {
            GameObject slamEffect = Instantiate(slamEffectPrefab, transform.position, Quaternion.identity);
            Destroy(slamEffect, 2f); // Destroys the slam effect after 2 seconds
        }

        // Deal damage to the player if they're within the slam radius
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= slamRadius)
        {
            player.GetComponent<Health>()?.TakeDamage(slamDamage);
        }

        // Reset proximity timer
        playerProximityTimer = 0f;

        // Optional: Cooldown or delay before another slam can occur
        yield return new WaitForSeconds(2f);
        isSlamming = false;
        Dj_Emperor.SetBool("isSlamming", false);
    }
    public IEnumerator ERAERASequence()
    {
        while (Time.time - trackStartTime < eraStartDelay)
        {
            yield return null; // Wait until enough time has passed
        }

        isPerformingERA = true;

        // Start the animation and let it run while the attack is happening
        Dj_Emperor.SetBool("isSpinning", true);
        Dj_Emperor.Play("dj_spin");

        // Allow the animation to run while the attack continues
        float animationDuration = Dj_Emperor.GetCurrentAnimatorStateInfo(0).length;
        StartCoroutine(ReturnToIdleAfterDelay(animationDuration)); // Schedule return to idle

        // Step 1: Slam
        Debug.Log("ERA ERA - Slam!");
        yield return StartCoroutine(PerformERASlam());

        // Step 2: Pull Record Toward DJ (Reverse spin)
        Debug.Log("ERA ERA - Pull Record Toward DJ!");
        float pullDuration = 2f;
        AdjustRecordSpeed(-1.5f);
        yield return new WaitForSeconds(pullDuration);

        // Step 3: Push Record Forward
        Debug.Log("ERA ERA - Push Record Forward!");
        float pushDuration = 2f;
        AdjustRecordSpeed(1.5f);
        yield return new WaitForSeconds(pushDuration);

        // Step 4: Reset Record Speed to Normal
        Debug.Log("ERA ERA - Reset Record Speed!");
        AdjustRecordSpeed(1f);

        yield return new WaitForSeconds(eraCooldown);
        isPerformingERA = false;
    }

    // Helper function: Makes sure DJ Emperor returns to idle after the animation is done
    private IEnumerator ReturnToIdleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Dj_Emperor.SetBool("isSpinning", false); // Stop spinning
        Dj_Emperor.Play("Idle"); // Return to idle
    }



    public IEnumerator PerformERASlam()
    {
        Debug.Log("Performing ERA-Slam!");

        // Optional: Telegraph animation or sound
        yield return new WaitForSeconds(eraSlamDelay);

        // Spawn Slam Effect
        if (eraSlamEffectPrefab != null)
        {
            GameObject slamEffect = Instantiate(eraSlamEffectPrefab, transform.position, Quaternion.identity);
            Destroy(slamEffect, 2f); // Destroy effect after 2 seconds
        }

        // Deal damage in radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, eraSlamRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Debug.Log("Player hit by ERA-Slam!");
                collider.GetComponent<Health>()?.TakeDamage(eraSlamDamage);
            }
        }

        yield return null;
    }

    private IEnumerator PerformERASting()
    {
        Debug.Log("Performing ERA-Sting!");

        // Spawn the sting projectile
        Vector3 spawnPosition = transform.position + Vector3.up * 5f; // Spawn above DJ
        GameObject sting = Instantiate(eraStingPrefab, spawnPosition, Quaternion.identity);

        // Move the sting toward the player
        while (sting != null && Vector3.Distance(sting.transform.position, player.position) > 0.1f)
        {
            sting.transform.position = Vector3.MoveTowards(sting.transform.position, player.position, eraStingSpeed * Time.deltaTime);
            yield return null;
        }

        // Deal damage if the sting hits the player
        if (sting != null && Vector3.Distance(sting.transform.position, player.position) <= 0.5f)
        {
            Debug.Log("ERA-Sting hit the player!");
            player.GetComponent<Health>()?.TakeDamage(eraStingDamage);
        }

        // Destroy the sting
        if (sting != null) Destroy(sting);

        yield return null;
    }

    public IEnumerator StingRoutine()
    {
        canSting = false;

        int stingCount = 9; // Number of times the sting will be fired
        float stingDelay = 0.3f; // Delay between each sting (adjust for balance)

        for (int i = 0; i < stingCount; i++)
        {
            // **Force-reset animation so it plays again**
            Dj_Emperor.SetBool("isStinging", false);
            yield return null; // Ensure Animator updates before re-triggering

            // **Trigger the animation before each sting**
        
         

            // Capture the player's position at this moment
            Vector3 targetPosition = player.position;

            // Wait for the animation to finish before firing
            yield return new WaitForSeconds(Dj_Emperor.GetCurrentAnimatorStateInfo(0).length * 0.9f);

            // Spawn the sting above DJ EMPEROR
            Vector3 spawnPosition = transform.position + new Vector3(0, 5f, 0); // Adjust height as needed
            GameObject sting = Instantiate(stingPrefab, spawnPosition, Quaternion.identity);

            // Move the sting toward the locked position
            StartCoroutine(MoveSting(sting, targetPosition));

            // Small delay before next sting (to prevent instant looping)
            yield return new WaitForSeconds(stingDelay);
        }

        // **Reset DJ Emperor to Idle when done**
        Dj_Emperor.SetBool("isStinging", false);

        // Cooldown before allowing the attack again
        yield return new WaitForSeconds(stingCooldown);
        canSting = true;
    }

    private IEnumerator MoveSting(GameObject sting, Vector3 targetPosition)
    {
        while (sting != null && Vector3.Distance(sting.transform.position, targetPosition) > 0.1f)
        {
            sting.transform.position = Vector3.MoveTowards(sting.transform.position, targetPosition, stingSpeed * Time.deltaTime);
            yield return null;
        }

        // Destroy the sting after reaching the location
        if (sting != null)
        {
            Destroy(sting);
        }
    }
}