using System;
using UnityEngine;
using System.Collections;
public class DJEMPERORHealth : Health
{

    protected override void HandleDeath()
    {
        Debug.Log($" {gameObject.name} has been defeated! Attempting to switch track...");

        Animator bossAnimator = GetComponentInChildren<Animator>();
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("Death"); // Play the death animation
            Debug.Log($"{gameObject.name} Death animation triggered.");
        }
        else
        {
            Debug.LogError($"{gameObject.name} has no Animator component!");
        }

        BossManager bossManager = FindFirstObjectByType<BossManager>();
        if (bossManager != null)
        {
            Debug.Log(" BossManager found! Calling BossDefeated()...");
            bossManager.BossDefeated(gameObject);
        }
        else
        {
            Debug.LogError(" BossManager not found!");
        }

        // **Wait for death animation to finish before deactivating**
        StartCoroutine(WaitAndDisable());
    }

    private IEnumerator WaitAndDisable()
    {
        Animator bossAnimator = GetComponentInChildren<Animator>();

        if (bossAnimator != null)
        {
            float deathAnimDuration = bossAnimator.GetCurrentAnimatorStateInfo(0).length;
            Debug.Log($"Waiting {deathAnimDuration} seconds for death animation to finish.");
            yield return new WaitForSeconds(deathAnimDuration);
        }
        else
        {
            yield return new WaitForSeconds(2f); // Default wait time
        }

        Debug.Log($"{gameObject.name} fully defeated. Disabling.");
        gameObject.SetActive(false);
    }





}