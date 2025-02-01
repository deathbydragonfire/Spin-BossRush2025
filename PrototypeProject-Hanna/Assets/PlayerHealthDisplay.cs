using UnityEngine;

public class PlayerHealth : Health
{
    [Header("Death Screen")]
    public DeathScreenHandler deathScreenHandler;

    protected override void HandleDeath()
    {
        Debug.Log("PLAYER HAS DIED!");

        if (deathScreenHandler != null)
        {
            deathScreenHandler.TriggerDeathScreen();
        }
        else
        {
            Debug.LogError("DeathScreenHandler is not assigned!");
        }
    }
}
