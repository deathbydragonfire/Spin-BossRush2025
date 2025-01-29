using System;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] public float maxHealth; // Maximum health
    private float health; // Current health

    public float CurrentHP // Public property to access current health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
            UIUpdate();

            if (health <= 0)
            {
                HandleDeath();
            }
        }
    }

    [Header("Death Screen")]
    public DeathScreenHandler deathScreenHandler; // Reference to the death screen manager

    void Start()
    {
        health = maxHealth; // Initialize health
        UIUpdate();
    }

    public void TakeDamageByCurrentHP(float damage)
    {
        CurrentHP = Mathf.Max(health - (health * (damage / 100f)), 0f);
    }

    public void TakeHeal(float heal)
    {
        CurrentHP = Mathf.Min(health + heal, maxHealth); // Increase health
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
        CurrentHP = Mathf.Max(health - damage, 0f); // Reduce health, ensure it doesn't go below 0
        Debug.Log($"Took damage: {damage}. Current health: {health}"); // For testing
    }
}
