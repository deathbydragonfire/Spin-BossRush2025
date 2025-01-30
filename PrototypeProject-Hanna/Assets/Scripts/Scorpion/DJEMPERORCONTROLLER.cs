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

    public float pushPullDuration = 3f; // Duration of the push/pull effect
    public float pushPullStrength = 10f; // Strength of the push/pull effect

    private bool isPerformingERA = false;
    private bool isStinging = false;

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
    }

    public  IEnumerator ERAERASequence()
    {
        isPerformingERA = true;

        // Step 1: Slam
        Debug.Log("ERA ERA - Slam!");
        yield return StartCoroutine(PerformERASlam());

        // Step 2: Push Record Forward
        Debug.Log("ERA ERA - Push Record Forward!");
        float pushDuration = 2f; // Duration of the push phase
        AdjustRecordSpeed(1.5f); // Push forward (multiplier set to 1.5x normal speed)
        yield return new WaitForSeconds(pushDuration);

        // Step 3: Pull Record Toward DJ (Reverse spin)
        Debug.Log("ERA ERA - Pull Record Toward DJ!");
        float pullDuration = 2f; // Duration of the pull phase
        AdjustRecordSpeed(-1.5f); // Reverse spin
        yield return new WaitForSeconds(pullDuration);

        // Step 4: Reset Record Speed to Normal
        Debug.Log("ERA ERA - Reset Record Speed!");
        AdjustRecordSpeed(1f); // Reset to normal speed

        // Cooldown before ERA ERA can be used again
        yield return new WaitForSeconds(eraCooldown);

        isPerformingERA = false;
    }



    public  IEnumerator PerformERASlam()
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

    public  IEnumerator StingRoutine()
    {
        canSting = false;

        // Spawn the sting above DJ EMPEROR
        Vector3 spawnPosition = transform.position + new Vector3(0, 5f, 0); // Adjust height as needed
        GameObject sting = Instantiate(stingPrefab, spawnPosition, Quaternion.identity);

        // Move the sting toward the player
        while (sting != null && Vector3.Distance(sting.transform.position, player.position) > 0.1f)
        {
            sting.transform.position = Vector3.MoveTowards(sting.transform.position, player.position, stingSpeed * Time.deltaTime);
            yield return null;
        }

        // Deal damage if the sting reaches the player
        if (sting != null && Vector3.Distance(sting.transform.position, player.position) <= 0.5f)
        {
            Debug.Log("Sting hit the player!");
            player.GetComponent<Health>()?.TakeDamage(stingDamage); // Ensure the player has a Health component
        }

        // Destroy the sting
        if (sting != null)
        {
            Destroy(sting);
        }

        // Wait for cooldown
        yield return new WaitForSeconds(stingCooldown);
        canSting = true;
    }
}