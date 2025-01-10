using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource
    public AudioClip[] normalTracks; // Normal tempo tracks
    public AudioClip[] fastTracks;   // Fast tempo tracks
    public AudioClip[] slowTracks;   // Slow tempo tracks

    public float tempoMeter = 0f; // The current tempo meter
    public float maxTempoMeter = 100f; // Maximum meter value
    public float tempoGainRate = 10f; // Meter gained per second while speeding up
    public float tempoBurnRate = 10f; // Meter lost per second while slowing down

    private string currentTempo = "normal"; // Tracks the current tempo
    private bool isSpeedingUp = false; // Tracks if the player is holding the speed-up key
    private bool isSlowingDown = false; // Tracks if the player is holding the slow-down key

    void Start()
    {
        PlayTrackAtTempo(0, "normal"); // Start with the first track at normal tempo
    }

    void Update()
    {
        HandleInput();
        UpdateTempoMeter();
    }

    void HandleInput()
    {
        // Speed up while holding Q
        if (Input.GetKey(KeyCode.Q))
        {
            isSpeedingUp = true;
            isSlowingDown = false; // Prevent simultaneous actions
            PlayTrackAtTempo(0, "fast");
        }
        // Slow down while holding E, only if tempo meter is sufficient
        else if (Input.GetKey(KeyCode.E) && tempoMeter > 0)
        {
            isSpeedingUp = false;
            isSlowingDown = true;
            PlayTrackAtTempo(0, "slow");
        }
        // Return to normal when no key is held
        else
        {
            isSpeedingUp = false;
            isSlowingDown = false;
            PlayTrackAtTempo(0, "normal");
        }
    }

    void UpdateTempoMeter()
    {
        // Gain tempo while speeding up
        if (isSpeedingUp)
        {
            tempoMeter = Mathf.Clamp(tempoMeter + tempoGainRate * Time.deltaTime, 0, maxTempoMeter);
        }
        // Lose tempo while slowing down
        else if (isSlowingDown)
        {
            tempoMeter = Mathf.Clamp(tempoMeter - tempoBurnRate * Time.deltaTime, 0, maxTempoMeter);
        }

        // Debug to track tempo meter value
        Debug.Log("Tempo Meter: " + tempoMeter);
    }

    public void PlayTrackAtTempo(int trackIndex, string tempo)
    {
        // Switch tracks only if tempo changes
        if (tempo != currentTempo)
        {
            // Calculate track progress as a percentage
            float trackProgress = audioSource.time / audioSource.clip.length;

            // Set the appropriate track
            switch (tempo)
            {
                case "fast":
                    audioSource.clip = fastTracks[trackIndex];
                    break;
                case "slow":
                    audioSource.clip = slowTracks[trackIndex];
                    break;
                default: // "normal"
                    audioSource.clip = normalTracks[trackIndex];
                    break;
            }

            // Play the new track and sync progress
            audioSource.Play();
            audioSource.time = trackProgress * audioSource.clip.length;

            // Update current tempo
            currentTempo = tempo;
        }
    }
}
