using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class Health : MonoBehaviour
{
    [SerializeField] public float maxHealth;
    private float health;
    public int currentHP = 100;

    //  Persistent HP storage for all bosses
    private static Dictionary<string, float> savedBossHP = new Dictionary<string, float>();

    public float CurrentHP
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

            //  **If this is a boss, save its HP**
            if (savedBossHP.ContainsKey(gameObject.name))
            {
                savedBossHP[gameObject.name] = health;
            }
        }
    }

    void Start()
    {
        this.currentHP = 100; // Force-set HP to make sure it's not getting set to 0
        Debug.Log("[Health] " + gameObject.name + " forced HP to 100.");

        if (CurrentHP <= 0)
        {
            Debug.LogError($"[Health] {gameObject.name} has no starting HP! Defaulting to 100.");
            CurrentHP = 100;
        }

        {
            Debug.Log($"[Health] {gameObject.name} has started with {CurrentHP} HP.");
        }

        //  **If the boss has saved HP, restore it**
        if (savedBossHP.ContainsKey(gameObject.name))
        {
            health = savedBossHP[gameObject.name];
        }
        else
        {
            health = maxHealth; // If no saved value, start fresh
            savedBossHP[gameObject.name] = maxHealth;
        }
        UIUpdate();
    }
    public void TakeBossDamage(float damage, GameObject attacker)
    {
        //  Only allow damage from the player's attack hitbox
        if (attacker.CompareTag("PlayerAttack"))
        {
            Debug.Log($"{gameObject.name} (Boss) took {damage} damage from {attacker.name}!");
            CurrentHP -= damage; //  Reduce HP
        }
        else
        {
            Debug.Log($"{gameObject.name} ignored damage from {attacker.name} (Not a PlayerAttack).");
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHP = Mathf.Max(health - damage, 0f);
    }

    public void TakeDamageByCurrentHP(float damage)
    {
        CurrentHP = Mathf.Max(health - (health * (damage / 100f)), 0f);
    }

    public void TakeHeal(float heal)
    {
        CurrentHP = Mathf.Min(health + heal, maxHealth);
    }

    protected virtual void HandleDeath()
    {
        Debug.Log($"{gameObject.name} has died! (DEFAULT HANDLING - Should be overridden)");

        // **When a boss dies, remove their saved HP so they reset next time**
        if (savedBossHP.ContainsKey(gameObject.name))
        {
            savedBossHP.Remove(gameObject.name);
        }
    }

    private void UIUpdate()
    {
        Debug.Log($"[Health] {gameObject.name} HP: {health}");
    }
    public void InitializeHealth()
    {
        if (this.currentHP <= 0)
        {
            this.currentHP = 100; // Default HP
            Debug.Log("[Health] Initialized HP for " + gameObject.name + " to 100.");
        }
    }
}
