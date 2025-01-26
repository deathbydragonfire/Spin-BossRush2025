using System;
using UnityEngine;


public abstract class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth; // Maximum health
    private float health; // Current health

    [Header("Death Screen")]
    public DeathScreenHandler deathScreenHandler; // Reference to the death screen manager

    void Start()
    {
        health = maxHealth; // Initialize health
        UIUpdate();
    }
    public void TakeDamageByCurrentHP(float damage)
    {
        health = Mathf.Max(health - (health * (damage / 100f)), 0f);
        UIUpdate();

        if (health <= 0)
        {
            HandleDeath();
        }
    }


    public void TakeHeal(float heal)
    {
        health = Mathf.Min(health + heal, maxHealth); // Increase health
        UIUpdate();
    }

    private void HandleDeath()
    {
        Debug.Log("Player is dead!"); // Debug log for testing
        if (deathScreenHandler != null)
        {
            deathScreenHandler.TriggerDeathScreen(); // Call the death screen handler
        }
        else
        {
            Debug.LogWarning("DeathScreenHandler is not assigned!");
        }
    }

    private void UIUpdate()
    {
        // Display the current health in the console for testing
        Debug.Log("Current Health: " + health);

        // Update health UI here (e.g., health bar)
    }

    internal void TakeDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0f); // Reduce health, ensure it doesn't go below 0
        UIUpdate(); // Update the UI if needed

        if (health <= 0)
        {
            HandleDeath(); // Trigger death logic
        }

        Debug.Log($"Took damage: {damage}. Current health: {health}"); // For testing
    }
}