using System;
using UnityEngine;
using UnityEngine.Audio;

public class MusicHandler : MonoBehaviour
{
    public AudioSource audioSource; // The AudioSource that plays music
    public AudioClip[] musicTracks; // Array to hold your music tracks
    private int currentTrackIndex = 0; // Keeps track of the current track being played

    void Start()
    {
        if (musicTracks.Length > 0)
        {
            PlayTrack(currentTrackIndex); // Start with the first track
        }
    }

    void Update()
    {
        // If the current track finishes, move to the next one
        if (!audioSource.isPlaying && musicTracks.Length > 0)
        {
            NextTrack();
        }
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                AdjustPlaybackSpeedUp(audioSource.pitch + 0.1f); // Speed up by 0.1
            }

            // Check for slow down (E key)
            if (Input.GetKeyDown(KeyCode.E))
            {
                AdjustPlaybackSpeedDown(audioSource.pitch - 0.1f); // Slow down by 0.1
            }

        }
    }

    public void PlayTrack(int trackIndex)
    {
        if (trackIndex >= 0 && trackIndex < musicTracks.Length)
        {
            audioSource.clip = musicTracks[trackIndex]; // Set the current track
            audioSource.Play(); // Play the track
        }
    }

    public void NextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % musicTracks.Length; // Move to the next track (loop back to start)
        PlayTrack(currentTrackIndex);
    }

    public void AdjustPlaybackSpeedUp(float speed)
    {
        speed = Mathf.Clamp(speed, 0.5f, 2.0f); // Clamp speed
        audioSource.pitch = speed;
        Debug.Log("Playback Speed Increased: " + speed);
    }

    public void AdjustPlaybackSpeedDown(float speed)
    {
        speed = Mathf.Clamp(speed, 0.5f, 2.0f); // Clamp speed
        audioSource.pitch = speed;
        Debug.Log("Playback Speed Decreased: " + speed);
    }
}