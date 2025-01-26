using UnityEngine;
using System;
using Mono.Cecil.Cil;
using System.Collections;

public class DJEmperorController : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float slamRadius = 3f; // Radius of the slam attack
    public float slamDamage = 20f; // Damage dealt by the slam
    public float proximityTime = 4f; // Time the player must stay close to trigger the slam
    public float slamDelay = 1f; // Delay before the slam happens
    public GameObject slamEffectPrefab; // Optional: Visual effect for the slam

    private float playerProximityTimer = 0f; // Tracks how long the player has been close
    private bool isSlamming = false; // Prevents multiple slams at once

    public GameObject stingPrefab; // Prefab for the sting object
    public float stingRange = 5f; // Range within which the sting can target the player
    public float stingDamage = 15f; // Damage dealt by the sting
    public float stingCooldown = 4f; // Cooldown between sting attacks
    public float stingSpeed = 10f; // Speed at which the sting moves
    private bool canSting = true; // Cooldown tracker
                                  // ERA-ERA Attack
    public float pullDuration = 2f; // Duration of the pull phase
    public float pushDuration = 2f; // Duration of the push phase
    public float pullStrength = 10f; // Strength of the pull
    public float pushStrength = 10f; // Strength of the push


    private bool isPerformingEraEra = false;




    void Update()
    {
        CheckPlayerProximity();
        {
            // Press "K" to test the sting attack
            if (Input.GetKeyDown(KeyCode.K))
            {
                PerformSting();
            }
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L)) // Press "L" to test ERA-ERA
            {
                PerformEraEra();
            }
        }

    }


    public void PerformSting()
    {
        if (canSting && Vector3.Distance(transform.position, player.position) <= stingRange)
        {
            StartCoroutine(StingRoutine());
        }
    }
      public void PerformEraEra()
    {
        if (!isPerformingEraEra)
        {
            StartCoroutine(EraEraRoutine());
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

    private void OnDrawGizmosSelected()
    {
        // Draw the slam radius in the scene view for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slamRadius);
    }
    // Sting Attack Routine
    private IEnumerator StingRoutine()
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