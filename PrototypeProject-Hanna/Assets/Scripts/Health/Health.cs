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
                HandleDeath(); // ✅ Call the appropriate death behavior (player OR boss)
            }
        }
    }

    [Header("Death Screen")]
    public DeathScreenHandler deathScreenHandler; // Reference to the death screen manager (ONLY for Player)

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

    protected virtual void HandleDeath() // ✅ Now this can be overridden by subclasses
    {
        Debug.Log($"{gameObject.name} has died! (DEFAULT HANDLING - Should be overridden)");
    }

    private void UIUpdate()
    {
        Debug.Log("Current Health: " + health); // ✅ Debug health updates
    }

    internal void TakeDamage(float damage)
    {
        CurrentHP = Mathf.Max(health - damage, 0f);
        Debug.Log($"{gameObject.name} took {damage} damage! Current HP: {health}");
    }
}
