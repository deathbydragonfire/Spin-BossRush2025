using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]  // <-- This makes the BossManager class serializable
public class BossManager : MonoBehaviour
{
    [System.Serializable]  // <-- This ensures BossData is visible in the Inspector
    public class BossData
    {
        public GameObject boss; // The boss GameObject
        public AudioClip musicTrack; // The associated music track
        [HideInInspector] public float currentHP; // Saved HP for the boss
        [HideInInspector] public bool isDefeated; // Whether the boss is defeated
        public string spawnTriggerName; // <-- This should now be visible
    }


    public List<BossData> bosses; // List of all bosses and their tracks
    public AudioSource audioSource; // AudioSource for playing tracks

    private int currentBossIndex = 0; // Index of the currently active boss
    private bool isLooping = false; // Whether only one boss remains

    public Animator animator;

    void Start()
    {
        // Initialize bosses
        foreach (var bossData in bosses)
        {
            bossData.currentHP = bossData.boss.GetComponent<Health>().maxHealth;
            bossData.boss.SetActive(false); // Deactivate all bosses initially
        }

    }

    void Update()
    {
        // Check if the current track has ended
        if (!audioSource.isPlaying && !isLooping && !SimplePauseMenu.IsGamePaused)
        {
            CycleToNextBoss();
        }


    }
    public void SetCurrentBossIndex(int index)
{
    currentBossIndex = index;
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
        //Debug.Log("[BossManager] Initializing bosses AFTER tracks are assigned.");

        foreach (var bossData in bosses)
        {
            bossData.currentHP = bossData.boss.GetComponent<Health>().maxHealth;
            bossData.boss.SetActive(false);
        }

    }

    void CycleToNextBoss()
    {
        if (bosses.Count == 0)
        {
            //Debug.Log("[BossManager] No more bosses left! Stopping music.");
            audioSource.Stop();
            return;
        }

        if (animator)
        {
            animator.SetTrigger("NextBoss");
        }

        //Debug.Log($"[BossManager] Attempting to cycle from {bosses[currentBossIndex].boss.name}");

        // Disable effects for the current boss
        string currentBossTag = $"{bosses[currentBossIndex].boss.name}Effect";
        foreach (GameObject effect in GameObject.FindGameObjectsWithTag(currentBossTag))
        {
            effect.SetActive(false);
        }

        // Save HP & Deactivate Boss
        BossData currentBossData = bosses[currentBossIndex];
        if (!currentBossData.isDefeated)
        {
            currentBossData.currentHP = currentBossData.boss.GetComponent<Health>().currentHP;
            currentBossData.boss.SetActive(false);
        }

        // **Find next valid boss** (skip defeated ones)
        int initialIndex = currentBossIndex;
        do
        {
            currentBossIndex = (currentBossIndex + 1) % bosses.Count;
            //Debug.Log($"[BossManager] Checking boss: {bosses[currentBossIndex].boss.name} (Defeated: {bosses[currentBossIndex].isDefeated})");

        } while (bosses[currentBossIndex].isDefeated && currentBossIndex != initialIndex);

        // **If all bosses are dead, stop music**
        if (bosses[currentBossIndex].isDefeated)
        {
            //Debug.Log("[BossManager] No more bosses left! Stopping music.");
            audioSource.Stop();
            return;
        }

        // **Ensure correct track is played**
        PlayNextTrack();
    }




    public void BossDefeated(GameObject boss)
    {
        BossData defeatedBoss = bosses.Find(b => b.boss == boss);
        if (defeatedBoss != null && !defeatedBoss.isDefeated)
        {
            defeatedBoss.isDefeated = true;
            Debug.Log($"[BossManager] {boss.name} has been defeated. Removing its track...");

            // Stop the track if it's playing
            if (audioSource.clip == defeatedBoss.musicTrack)
            {
                //Debug.Log($"[BossManager] Stopping {audioSource.clip.name} since {boss.name} is defeated.");
                audioSource.Stop();
            }

            // **Defer track removal**
            StartCoroutine(DeferredTrackRemoval(boss.name));

            // **Cycle to the next boss first**
            CycleToNextBoss();
        }
    }

    // **New Coroutine: Wait, then remove the track**
    private IEnumerator DeferredTrackRemoval(string bossName)
    {
        yield return new WaitForSeconds(0.5f); // Short delay to allow cycling

        MusicHandler musicHandler = FindObjectOfType<MusicHandler>();
        if (musicHandler != null)
        {
            musicHandler.RemoveTrackAndBoss(bossName);
        }
        else
        {
            Debug.LogError("[BossManager] MusicHandler not found!");
        }
    }







    public void ActivateBoss(string bossName)
    {
        Debug.Log($"[BossManager] Attempting to Activate: {bossName}");

        // **Step 1: Ensure all bosses are turned OFF before activating a new one**
        foreach (BossData boss in bosses)
        {
            if (boss.boss.activeSelf)
            {
                boss.boss.SetActive(false);
                Debug.Log($"[BossManager] {boss.boss.name} forcibly deactivated.");
            }
        }

        // **Step 2: Find and Activate the correct boss**
        BossData bossData = bosses.Find(b => b.boss.name == bossName);
        if (bossData == null)
        {
            Debug.LogError($"[ERROR] Boss not found: {bossName}");
            return;
        }

        bossData.boss.SetActive(true);
        Debug.Log($"[BossManager] Activated: {bossName}");

        // **Step 3: Restore HP**
        Health bossHealth = bossData.boss.GetComponent<Health>();
        if (bossHealth != null)
        {
            bossHealth.currentHP = bossData.currentHP; // Restore HP
            Debug.Log($"[BossManager] {bossName} HP restored to {bossHealth.currentHP}");
        }

        // **Step 4: Completely Restart AI Scripts**
        MonoBehaviour[] scripts = bossData.boss.GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != null)
            {
                script.enabled = false;
                script.enabled = true;
            }
        }
        Debug.Log($"[BossManager] {bossName} AI Fully Reset!");

        // **Step 5: Manually Trigger `Start()` for AI Scripts (If Needed)**
        foreach (MonoBehaviour script in scripts)
        {
            if (script != null)
            {
                System.Reflection.MethodInfo startMethod = script.GetType().GetMethod("Start", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
                if (startMethod != null)
                {
                    startMethod.Invoke(script, null);
                    Debug.Log($"[BossManager] Manually triggered Start() on {script.GetType().Name} for {bossName}");
                }
            }
        }

        // **Step 6: Restart Animator & Play Spawn Animation**
        Animator bossAnimator = bossData.boss.GetComponentInChildren<Animator>();
        if (bossAnimator != null)
        {
            bossAnimator.Rebind();
            bossAnimator.Play(bossData.spawnTriggerName, 0, 0);
            //Debug.Log($"[BossManager] Played spawn animation for {bossName}");
        }

        // **Step 7: Reset Visual Effects**
        string newBossTag = $"{bossName}Effect";
        foreach (GameObject effect in GameObject.FindGameObjectsWithTag(newBossTag))
        {
            effect.SetActive(false);
            effect.SetActive(true);
            Debug.Log($"[BossManager] Reset effect: {effect.name}");
        }
    }


    private IEnumerator SetSpawnComplete(Animator animator)
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        if (animator != null)
        {
            animator.SetBool("SpawnComplete", true); // **Tells the animator to go Idle**
            //Debug.Log("[BossManager] Forced transition to Idle.");
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
        return bosses[currentBossIndex].boss.GetComponent<Health>().currentHP;
    }
}
