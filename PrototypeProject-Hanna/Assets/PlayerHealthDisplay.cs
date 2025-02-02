using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    public string deathScreen;

    protected override void HandleDeath()
    {
        Debug.Log("PLAYER HAS DIED!");
        SceneManager.LoadScene(deathScreen); // Reload the current level


    }
}
