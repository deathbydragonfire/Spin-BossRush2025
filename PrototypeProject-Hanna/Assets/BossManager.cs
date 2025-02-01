using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
        if (!audioSource.isPlaying && !isLooping && !SimplePauseMenu.IsGamePaused)
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
        // **CLEAN UP LINGERING EFFECTS BEFORE SWITCHING BOSSES**
        foreach (GameObject effect in GameObject.FindGameObjectsWithTag("BossEffect"))
        {
            effect.SetActive(false); //  Instead of destroying, disable the objects
        }

        // Save the current boss's HP before deactivating
        BossData currentBossData = bosses[currentBossIndex];
        if (!currentBossData.isDefeated)
        {
            currentBossData.currentHP = currentBossData.boss.GetComponent<Health>().CurrentHP;
            currentBossData.boss.SetActive(false);
        }

        // Move to the next boss
        currentBossIndex = (currentBossIndex + 1) % bosses.Count;

        // Check if only one boss remains
        if (bosses.FindAll(b => !b.isDefeated).Count == 1)
        {
            isLooping = true;
        }

        PlayNextTrack();
    }



    public void BossDefeated(GameObject boss)
    {
        BossData defeatedBoss = bosses.Find(b => b.boss == boss);
         if (defeatedBoss != null && !defeatedBoss.isDefeated)
        {
            defeatedBoss.isDefeated = true;
            defeatedBoss.boss.SetActive(false);
            Debug.Log($" {boss.name} has been defeated. Removing its track...");

            // Remove boss track from playlist
            if (audioSource.clip == defeatedBoss.musicTrack)
            {
                Debug.Log($" Stopping {audioSource.clip.name} since {boss.name} is defeated.");
                audioSource.Stop();
            }

            bosses.Remove(defeatedBoss); //  REMOVE boss from the active list

            if (bosses.Count > 0)
            {
                Debug.Log(" Moving to next boss...");
                StartCoroutine(WaitAndPlayNextTrack());
            }
            else
            {
                Debug.Log(" All bosses defeated! Stopping music.");
                audioSource.Stop();
            }
        }
        else
        {
            Debug.LogError($" Boss {boss.name} not found in the list!");
        }
    }
    private IEnumerator WaitAndPlayNextTrack()
    {
        Debug.Log(" Waiting before switching track...");
        yield return new WaitForSeconds(1f); // Small delay

        Debug.Log(" Attempting to switch tracks...");

        if (bosses.FindAll(b => !b.isDefeated).Count > 0)
        {
            Debug.Log(" Playing next track!");
            CycleToNextBoss(); //  This directly moves to the next boss
        }
        else
        {
            Debug.Log(" No more bosses! Stopping music.");
            audioSource.Stop();
        }
    }

   public void ActivateBoss(string bossName)
{
    BossData bossData = bosses.Find(b => b.boss.name == bossName);

    if (bossData != null)
    {
        Debug.Log($"[BossManager] ACTIVATING BOSS: {bossName}");
        bossData.boss.SetActive(true);

   
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
