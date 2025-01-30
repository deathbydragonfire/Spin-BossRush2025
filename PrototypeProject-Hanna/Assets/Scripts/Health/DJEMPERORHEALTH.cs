using UnityEngine;

public class DJEMPERORHealth : Health
{
    protected override void HandleDeath()
    {
        Debug.Log($"{gameObject.name} has been defeated! Disabling...");
        gameObject.SetActive(false); // ✅ Boss disappears when dead
    }
}
