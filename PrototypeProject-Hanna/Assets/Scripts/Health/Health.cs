using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class Health : MonoBehaviour
{
    [SerializeField] public float maxHealth = 100f;
    private static Dictionary<string, float> savedBossHP = new Dictionary<string, float>();

    public float currentHP;

    protected virtual void Start()
    {
        if (maxHealth <= 0)
        {
            Debug.LogWarning($"[Health] {gameObject.name} has invalid maxHealth! Setting to default 100.");
            maxHealth = 100f;
        }

        if (savedBossHP.TryGetValue(gameObject.name, out float savedHP))
        {
            currentHP = savedHP;
            Debug.Log($"[Health] {gameObject.name} restored to {currentHP} HP.");
        }
        else
        {
            currentHP = maxHealth;
            savedBossHP[gameObject.name] = currentHP;
            Debug.Log($"[Health] {gameObject.name} started with {currentHP} HP.");
        }

        UIUpdate();
    }

    public void TakeDamage(float damage)
    {
        currentHP = Mathf.Clamp(currentHP - damage, 0, maxHealth);
        Debug.Log($"[Health] {gameObject.name} took {damage} damage. Remaining HP: {currentHP}");
        CheckDeath();
        SaveBossHP();
    }

    public void TakeDamageByCurrentHP(float percentage)
    {
        float damage = currentHP * (percentage / 100f);
        TakeDamage(damage);
    }

    public void TakeHeal(float heal)
    {
        currentHP = Mathf.Clamp(currentHP + heal, 0, maxHealth);
        Debug.Log($"[Health] {gameObject.name} healed {heal} HP. New HP: {currentHP}");
        SaveBossHP();
    }

    private void SaveBossHP()
    {
        if (savedBossHP.ContainsKey(gameObject.name))
        {
            savedBossHP[gameObject.name] = currentHP;
        }
    }

    private void CheckDeath()
    {
        if (currentHP <= 0)
        {
            HandleDeath();
        }
    }

    protected virtual void HandleDeath()
    {
        Debug.Log($"{gameObject.name} has died! (DEFAULT HANDLING - Should be overridden)");
        savedBossHP.Remove(gameObject.name);
        Destroy(gameObject);
    }

    private void UIUpdate()
    {
        Debug.Log($"[Health] {gameObject.name} HP: {currentHP}/{maxHealth}");
    }
}
