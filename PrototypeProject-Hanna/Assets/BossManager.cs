using UnityEngine;
using System.Collections.Generic;

public class BossManager : MonoBehaviour
{
    [System.Serializable]
    public class BossData
    {
        public GameObject boss; // The boss GameObject
        public AudioClip musicTrack; // The associated music track
        [HideInInspector] public float currentHP; // Saved HP for the boss
        [HideInInspector] public bool isDefeated; // Whether the boss is defeated
    }

    public List<BossData> bosses; // List of all bosses and their tracks
    public AudioSource audioSource; // AudioSource for playing tracks

    private int currentBossIndex = 0; // Index of the currently active boss
    private bool isLooping = false; // Whether only one boss remains

    void Start()
    {
        // Initialize bosses
        foreach (var bossData in bosses)
        {
            bossData.currentHP = bossData.boss.GetComponent<Health>().maxHealth;
            bossData.boss.SetActive(false); // Deactivate all bosses initially
        }

        PlayNextTrack();
    }

    void Update()
    {
        // Check if the current track has ended
        if (!audioSource.isPlaying && !isLooping)
        {
            CycleToNextBoss();
        }

        if (Input.GetKeyDown(KeyCode.P)) //Debugging tool, remove me
        {
            CycleToNextBoss();
        }
        
    }

    void PlayNextTrack()
    {
        // Get the next boss and track
        BossData currentBossData = bosses[currentBossIndex];

        if (!currentBossData.isDefeated)
        {
            string bossName = GetBossNameByIndex(currentBossIndex);
            ActivateBoss(bossName);
            ;

            // Play their music track
            audioSource.clip = currentBossData.musicTrack;
            audioSource.Play();
        }
        else
        {
            CycleToNextBoss();
        }
    }
    public void InitializeBosses()
    {
        Debug.Log("[BossManager] Initializing bosses AFTER tracks are assigned.");

        foreach (var bossData in bosses)
        {
            bossData.currentHP = bossData.boss.GetComponent<Health>().maxHealth;
            bossData.boss.SetActive(false);
        }

    }

    void CycleToNextBoss()
    {
        // Save the current boss's state
        BossData currentBossData = bosses[currentBossIndex];
        if (!currentBossData.isDefeated)
        {
            currentBossData.currentHP = currentBossData.boss.GetComponent<Health>().CurrentHP;
            currentBossData.boss.SetActive(false);
        }

        // Move to the next boss
        currentBossIndex = (currentBossIndex + 1) % bosses.Count;

        // Check if all bosses but one are defeated
        if (bosses.FindAll(b => !b.isDefeated).Count == 1)
        {
            isLooping = true;
        }

        PlayNextTrack();
    }

    public void BossDefeated(GameObject boss)
    {
        // Mark the boss as defeated
        BossData defeatedBoss = bosses.Find(b => b.boss == boss);
        if (defeatedBoss != null)
        {
            defeatedBoss.isDefeated = true;
            defeatedBoss.boss.SetActive(false);

            // Remove their track from the playlist if only one boss remains
            if (bosses.FindAll(b => !b.isDefeated).Count == 1)
            {
                isLooping = true;
            }
        }
    }

    public void ActivateBoss(string bossName)
    {
        BossData bossData = bosses.Find(b => b.boss.name == bossName);

        if (bossData != null)
        {
            Debug.Log($"[BossManager] Activating Boss: {bossName}");
            bossData.boss.SetActive(true);
            bossData.boss.GetComponent<Health>().CurrentHP = bossData.currentHP;
        }
        else
        {
            Debug.LogError($"[ERROR] Boss not found: {bossName}");
        }
    }



    public string GetBossNameByIndex(int index)
    {
        if (index >= 0 && index < bosses.Count)
        {
            return bosses[index].boss.name; // Return the actual boss GameObject's name
        }
        Debug.LogError($"[BossManager] Index {index} is out of range!");
        return null;
    }
    public void DeactivateAllBosses()
    {
        foreach (var bossData in bosses)
        {
            bossData.boss.SetActive(false);
        }

    }

    public float GetCurrentBossHP()
    {
        return bosses[currentBossIndex].currentHP;
    }
}
