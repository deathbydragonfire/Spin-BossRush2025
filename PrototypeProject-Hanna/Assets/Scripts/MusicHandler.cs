using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicHandler : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public Transform record; // The spinning record GameObject

    [Header("Playback Settings")]
    public float normalPlaybackSpeed = 1.0f; // Default playback speed
    public float speedUpMultiplier = 1.5f; // Speed-up factor for player input
    public float slowDownMultiplier = 0.5f; // Slow-down factor for player input

    [Header("Tempo Meter Settings")]
    public float maxTempo = 100f; // Maximum tempo meter value
    public float tempoBurnRate = 10f; // How quickly tempo burns when slowing down
    public float tempoBuildRate = 5f; // How quickly tempo builds when speeding up
    private float currentTempo; // Current tempo meter value

    private float baseMultiplier = 1f; // Base multiplier for external effects (e.g., Zap Riot)
    private float playerMultiplier = 1f; // Multiplier for player input
    public float currentPlaybackSpeed; // Current playback speed
    public float currentSpinSpeed; // Current spin speed

    [Header("Boss Track Management")]
    public List<AudioClip> bossTracks; // List of tracks for each boss
    private Dictionary<AudioClip, string> trackBossMap = new Dictionary<AudioClip, string>();
    private int currentTrackIndex = 0; // Index of the current track
    public BossManager bossManager; // Reference to the BossManager for handling boss logic
    private bool isInitialized = false;
    public bool isPaused = false; //  New flag to track pause state
    IEnumerator Start()
    {
        Debug.Log("[MusicHandler] Waiting for scene to stabilize...");
        yield return new WaitForSeconds(0.2f);

        Debug.Log("[MusicHandler] Scene stabilized. Initializing music.");
        currentPlaybackSpeed = normalPlaybackSpeed;
        currentSpinSpeed = 100f;
        currentTempo = maxTempo;

        if (bossManager != null && bossTracks.Count > 0)
        {
            trackBossMap.Clear();
            for (int i = 0; i < bossTracks.Count && i < bossManager.bosses.Count; i++)
            {
                trackBossMap[bossTracks[i]] = bossManager.bosses[i].boss.name;
            }

            // **Force the first track & boss to be DJ Emperor**
            //  Ensure this only happens ONCE
            if (!isInitialized)
            {
                isInitialized = true;  // **Set flag so it never re-initializes**

                // **Force the first track & boss to be DJ Emperor**
                currentTrackIndex = 0;
                audioSource.Stop(); //  Stop any residual audio
                yield return new WaitForSeconds(0.1f); //  Small delay to clear any old track

                audioSource.clip = bossTracks[currentTrackIndex];
                audioSource.Play();

                string firstBossName = bossManager.bosses[currentTrackIndex].boss.name;
                Debug.Log($"[MusicHandler] Forcing first boss: {firstBossName}");

               
                bossManager.ActivateBoss(firstBossName);
            }

        }
    }




        void Update()
    {
        HandlePlayerInputs(); // Allow player inputs to adjust speed
        ApplyCombinedSpeed(); // Combine base multiplier and player multiplier
        RotateRecord(); // Rotate the record
        if (!SimplePauseMenu.IsGamePaused) // Only run this if NOT paused
        {
            if (!isInitialized || SimplePauseMenu.IsGamePaused) return; // **Prevent interference**

            if (!audioSource.isPlaying && bossTracks.Count > 0)
            {
                Debug.Log("[MusicHandler] Track ended. Moving to next boss...");
                PlayNextTrack();
            }



        }
    }
    IEnumerator InitializeMusic()
    {
        yield return new WaitForSeconds(0.2f);

        Debug.Log("[MusicHandler] Scene stabilized. Initializing music.");

        if (bossManager != null && bossTracks.Count > 0)
        {
            trackBossMap.Clear();

            // Ensure bossTracks & bosses match order
            for (int i = 0; i < bossTracks.Count && i < bossManager.bosses.Count; i++)
            {
                trackBossMap[bossTracks[i]] = bossManager.bosses[i].boss.name;
            }

            yield return new WaitForSeconds(0.1f);
            PlayNextTrack();
        }
    }

    public void AdjustPlaybackAndSpinSpeed(float multiplier)
    {
        baseMultiplier = multiplier; // Update the base multiplier
        ApplyCombinedSpeed(); // Recalculate the combined speed
    }

    public void SetBaseMultiplier(float multiplier)
    {
        baseMultiplier = multiplier; // Update the base multiplier for external effects
    }

    private void HandlePlayerInputs()
    {
        if (Input.GetAxis("Speed") > 0) // 
        {
            playerMultiplier = speedUpMultiplier;
            currentTempo += Time.deltaTime * tempoBuildRate; // 
            currentTempo = Mathf.Min(currentTempo, maxTempo); // 
        }
        else if (Input.GetAxis("Speed") < 0 && currentTempo > 0) // 
        {
            playerMultiplier = slowDownMultiplier;
            currentTempo -= Time.deltaTime * tempoBurnRate; //
            currentTempo = Mathf.Max(currentTempo, 0); // Prevent it from going negative
        }
        else
        {
            playerMultiplier = 1f; // Reset to default multiplier
        }
    }



    private void ApplyCombinedSpeed()
    {
        float combinedMultiplier = baseMultiplier * playerMultiplier;

        // Adjust playback speed (can be negative for reverse playback)
        currentPlaybackSpeed = normalPlaybackSpeed * Mathf.Abs(combinedMultiplier);
        audioSource.pitch = Mathf.Sign(combinedMultiplier) * currentPlaybackSpeed; // Reverse audio if multiplier is negative

        // Adjust spin speed (reverse if multiplier is negative)
        currentSpinSpeed = 100f * combinedMultiplier; // Adjust as needed
    }

    private void RotateRecord()
    {
        if (record != null)
        {
            record.Rotate(Vector3.up, currentSpinSpeed * Time.deltaTime);
        }
    }

    public float GetCurrentTempo()
    {
        return currentTempo;
    }

    public float GetMaxTempo()
    {
        return maxTempo;
    }

    private void PlayNextTrack()
    {
        if (bossTracks.Count == 0) return;

        if (currentTrackIndex == 0 && audioSource.isPlaying)
        {
            Debug.LogWarning("[MusicHandler] Preventing unnecessary track skip on the first play.");
            return; // Ensures the first track isn't skipped on start
        }

        currentTrackIndex = (currentTrackIndex + 1) % bossTracks.Count;

        AudioClip nextTrack = bossTracks[currentTrackIndex];

        if (audioSource != null)
        {
            audioSource.clip = nextTrack;
            audioSource.Play();
        }

        if (trackBossMap.TryGetValue(nextTrack, out string bossName))
        {
            Debug.Log($"[MusicHandler] Now playing: {nextTrack.name} -> Summoning {bossName}");

            if (bossManager != null)
            {
                bossManager.DeactivateAllBosses();

                // **Force currentBossIndex to match track index**
                bossManager.SetCurrentBossIndex(currentTrackIndex);
                bossManager.ActivateBoss(bossName);
            }
        }
    }



    public void RemoveTrackAndBoss(string bossName)
    {
        int bossIndex = bossManager.bosses.FindIndex(b => b.boss.name.Equals(bossName, System.StringComparison.OrdinalIgnoreCase));

        if (bossIndex != -1)
        {
            AudioClip trackToRemove = bossTracks[bossIndex];
            bossTracks.Remove(trackToRemove);
            trackBossMap.Remove(trackToRemove);
            bossManager.bosses[bossIndex].isDefeated = true;

            if (bossTracks.Count > 0)
            {
                PlayNextTrack();
            }
            else
            {
                audioSource.Stop(); // Stop if no tracks remain
            }
        }
        else
        {
            Debug.LogError($"Boss with name {bossName} not found!");
        }
    }
}


