using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float health;


    //[Header("UI")]
    
    void Start()
    {
        health = maxHealth;
        UIUpdate();
    }

    private void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0f);
        UIUpdate();
    }

    public void TakeHeal(float heal)
    {
        health = Mathf.Min(health + heal, maxHealth);
        UIUpdate();
    }

    public void TakeDamageByCurrentHP(float damage)
    {
        health = Mathf.Max(health - (health * (damage/100f)) , 0f);
        UIUpdate();
    }

    public void TakeHealByCurrentHP(float heal)
    {
        health = Mathf.Min(health - (health * (heal/100f)) , maxHealth);
        UIUpdate();
    }

    public void TakeDamageByMaxHP(float damage)
    {
        health = Mathf.Max(health - (maxHealth * (damage / 100f)), 0f);
        UIUpdate();
    }

    public void TakeHealByMaxHP(float heal)
    {
        health = Mathf.Max(health - (maxHealth * (heal / 100f)), maxHealth);
        UIUpdate();
    }

    private void UIUpdate()
    {

    }
}
