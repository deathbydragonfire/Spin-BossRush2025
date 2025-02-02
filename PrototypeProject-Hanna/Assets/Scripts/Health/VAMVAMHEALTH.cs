using System;
using UnityEngine;
using System.Collections; // Needed for IEnumerator

public class VAMVAMHealth : Health
{
    protected override void HandleDeath()
    {
        Debug.Log($" {gameObject.name} has been defeated! Attempting to switch track...");

        Animator bossAnimator = GetComponentInChildren<Animator>();
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("Death"); // Play the death animation
        }

        // **Find the BossManager**
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

        ResetBossEffects(); // Clears any lingering effects

        // **Wait for death animation to finish before disabling the boss**
        StartCoroutine(WaitAndDisable());
    }

    private IEnumerator WaitAndDisable()
    {
        yield return new WaitForSeconds(2f); // Adjust based on death animation length

        Debug.Log($"{gameObject.name} fully defeated. Disabling.");
        gameObject.SetActive(false);
    }

    private void ResetBossEffects()
    {
       
        //  Reset any ongoing attacks
        VamVamController vamController = GetComponent<VamVamController>();
        if (vamController != null)
        {
            vamController.StopAllCoroutines(); // Cancel all attacks immediately
        }
    }
}
