using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource
    public AudioClip[] normalTracks; // Normal tempo tracks
    public AudioClip[] fastTracks;   // Fast tempo tracks
    public AudioClip[] slowTracks;   // Slow tempo tracks

    private string currentTempo = "normal"; // Tracks the current tempo

    void Start()
    {
        PlayTrackAtTempo(0, "normal"); // Start with the first track at normal tempo
    }

    public void PlayTrackAtTempo(int trackIndex, string tempo)
    {
        // Calculate the percentage of the track completed
        float trackProgress = audioSource.time / audioSource.clip.length;

        // Determine which track list to use
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

        // Play the new track and set its playback position
        audioSource.Play();
        audioSource.time = trackProgress * audioSource.clip.length; // Synchronize playback position
        currentTempo = tempo; // Update the current tempo
    }

    void Update()
    {
        // Example input handling for tempo changes
        if (Input.GetKeyDown(KeyCode.Q)) // Speed up
        {
            PlayTrackAtTempo(0, "fast"); // Adjust to fast tempo
        }

        if (Input.GetKeyDown(KeyCode.E)) // Slow down
        {
            PlayTrackAtTempo(0, "slow"); // Adjust to slow tempo
        }

        if (Input.GetKeyDown(KeyCode.Space)) // Reset to normal
        {
            PlayTrackAtTempo(0, "normal"); // Adjust to normal tempo
        }
    }
}
