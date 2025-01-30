using UnityEngine;

public class PlayerHealth : Health
{
    protected override void HandleDeath()
    {
        Debug.Log("Player is dead! Triggering Death Screen.");
        if (deathScreenHandler != null)
        {
            deathScreenHandler.TriggerDeathScreen(); // ✅ This ONLY happens for the player
        }
        else
        {
            Debug.LogWarning("DeathScreenHandler is not assigned!");
        }
    }
}
