using System;
using UnityEngine;
public class DJEMPERORHealth : Health
{


    protected override void HandleDeath()
    {
        Debug.Log($" {gameObject.name} has been defeated! Attempting to switch track...");

        gameObject.SetActive(false);

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

        ResetBossEffects();
    }


    private void ResetBossEffects()
    {
        Debug.Log(" Resetting DJ EMPEROR's lingering effects!");

        // Reset record speed
        MusicHandler musicHandler = FindFirstObjectByType<MusicHandler>();
        if (musicHandler != null)
        {
            musicHandler.SetBaseMultiplier(1f); // ✅ Restore normal speed
        }

        //  Reset any ongoing attacks
        DJEmperorController djController = GetComponent<DJEmperorController>();
        if (djController != null)
        {
            djController.StopAllCoroutines(); // ✅ Cancel all attacks immediately
        }
    }
}

