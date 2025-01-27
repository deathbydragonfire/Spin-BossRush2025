using UnityEngine;
using System.Collections;

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

    void Start()
    {
        // Initialize values
        currentPlaybackSpeed = normalPlaybackSpeed;
        currentSpinSpeed = 100f; // Default spin speed
        currentTempo = maxTempo; // Start with a full tempo meter
    }

    void Update()
    {
        HandlePlayerInputs(); // Allow player inputs to adjust speed
        ApplyCombinedSpeed(); // Combine base multiplier and player multiplier
        RotateRecord(); // Rotate the record
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
        if (Input.GetKey(KeyCode.Q)) // Speed up
        {
            playerMultiplier = speedUpMultiplier;
        }
        else if (Input.GetKey(KeyCode.E)) // Slow down
        {
            playerMultiplier = slowDownMultiplier;
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
}
