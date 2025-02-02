using System;
using UnityEngine;
public class ZAPRIOTHealth : Health
{


    protected override void HandleDeath()
    {
        Debug.Log($" {gameObject.name} has been defeated! Attempting to switch track...");
        //foreach (GameObject effect in GameObject.FindGameObjectsWithTag("BossEffect"))
        //{
            //Destroy(effect);
        //}

        gameObject.SetActive(false);

        BossManager bossManager = FindFirstObjectByType<BossManager>();
        if (bossManager != null)
        {
            Debug.Log(" BossManager found! Calling BossDefeated()...");
            bossManager.BossDefeated(gameObject);
        }
        else
        {
            Debug.LogError(" BossManager not found!");
        }
    }
}



