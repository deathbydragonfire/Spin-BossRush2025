<<<<<<< HEAD
=======
using System;
>>>>>>> c67864f66a66abf3cc20a9091e2227cfe3efd59c
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
    public static event Action OnTrackEnd; // Event triggered when a track ends

    private void Start()
    {
        PlayTrackAtTempo(0, "normal"); // Start with the first track at normal tempo
    }

    private void Update()
    {
        HandleInput();
        UpdateTempoMeter();
<<<<<<< HEAD
        UpdatePlaybackSpeed(); // Adjust playback speed dynamically
        UpdateSpinSpeed();     // Adjust spin speed dynamically
}
=======
        CheckTrackEnd(); // Check if the current track has ended
    }
>>>>>>> c67864f66a66abf3cc20a9091e2227cfe3efd59c

    private void HandleInput()
    {
        float speedInput = Input.GetAxis("Speed"); // Define "Speed" in Input Manager

        if (speedInput > 0) // Positive input for speeding up
        {
            PlayTrackAtTempo(0, "fast");
        }
        else if (speedInput < 0 && tempoMeter > 0) // Negative input for slowing down
        {
            PlayTrackAtTempo(0, "slow");
        }
        else // Neutral input for normal tempo
        {
            PlayTrackAtTempo(0, "normal");
        }
    }

    private void UpdateTempoMeter()
    {
        float speedInput = Input.GetAxis("Speed");

        if (speedInput > 0) // Positive input for speeding up
        {
            tempoMeter = Mathf.Clamp(tempoMeter + tempoGainRate * Time.deltaTime, 0, maxTempoMeter);
        }
        else if (speedInput < 0) // Negative input for slowing down
        {
            tempoMeter = Mathf.Clamp(tempoMeter - tempoBurnRate * Time.deltaTime, 0, maxTempoMeter);
        }

        // Debug to track tempo meter value
        //Debug.Log("Tempo Meter: " + tempoMeter);
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

    private void CheckTrackEnd()
    {
<<<<<<< HEAD
        float spinSpeed = normalSpinSpeed; // Default to normal spin speed

        if (isSpeedingUp)
=======
        if (!audioSource.isPlaying && audioSource.time >= audioSource.clip.length)
>>>>>>> c67864f66a66abf3cc20a9091e2227cfe3efd59c
        {
            OnTrackEnd?.Invoke(); // Trigger the track-end event
            Debug.Log("Track has ended!");
        }
    }
}