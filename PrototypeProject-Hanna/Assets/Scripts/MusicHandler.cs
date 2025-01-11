using Unity.VisualScripting;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource
    public SpinForever spinForever; // Reference to SpinForever script

    // Playback speed variables
    public float normalPlaybackSpeed = 1.0f; // Default playback speed
    public float fastPlaybackMultiplier = 1.5f; // Multiplier for speeding up playback
    public float slowPlaybackMultiplier = 0.75f; // Multiplier for slowing down playback

    // Record spin speed variables
    public float normalSpinSpeed = 360f; // Default spin speed in degrees per second
    public float fastSpinMultiplier = 1.2f; // Multiplier for speeding up spin
    public float slowSpinMultiplier = 0.8f; // Multiplier for slowing down spin

    // Tempo meter
    public float tempoMeter = 50f; // Current tempo meter
    public float maxTempoMeter = 100f; // Maximum tempo meter value
    public float tempoGainRate = 10f; // Tempo gain per second while speeding up
    public float tempoBurnRate = 10f; // Tempo loss per second while slowing down

    private bool isSpeedingUp = false; // Tracks if speeding up
    private bool isSlowingDown = false; // Tracks if slowing down

    void Start()
    {
        // Set initial playback and spin speeds
        audioSource.pitch = normalPlaybackSpeed;
        spinForever.SetRotationSpeed(normalSpinSpeed);
    }

    void Update()
    {
        HandleInput();
        UpdateTempoMeter();
        UpdatePlaybackSpeed(); // Adjust playback speed dynamically
        UpdateSpinSpeed();     // Adjust spin speed dynamically
    }

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.Q)) // Speed up
        {
            isSpeedingUp = true;
            isSlowingDown = false;
        }
        else if (Input.GetKey(KeyCode.E) && tempoMeter > 0) // Slow down
        {
            isSpeedingUp = false;
            isSlowingDown = true;
        }
        else // Normal speed
        {
            isSpeedingUp = false;
            isSlowingDown = false;
        }
    }

    void UpdateTempoMeter()
    {
        if (isSpeedingUp)
        {
            tempoMeter = Mathf.Clamp(tempoMeter + tempoGainRate * Time.deltaTime, 0, maxTempoMeter);
        }
        else if (isSlowingDown)
        {
            tempoMeter = Mathf.Clamp(tempoMeter - tempoBurnRate * Time.deltaTime, 0, maxTempoMeter);
        }
        Debug.Log("Tempo Meter: " + tempoMeter);
    }

    void UpdatePlaybackSpeed()
    {
        float playbackSpeed = normalPlaybackSpeed; // Default to normal speed

        if (isSpeedingUp)
        {
            playbackSpeed = normalPlaybackSpeed * fastPlaybackMultiplier;
        }
        else if (isSlowingDown && tempoMeter > 0)
        {
            playbackSpeed = normalPlaybackSpeed * slowPlaybackMultiplier;
        }

        // Apply playback speed to the audio source
        audioSource.pitch = playbackSpeed;
        Debug.Log($"Playback Speed: {playbackSpeed}");
    }

    void UpdateSpinSpeed()
    {
        float
 spinSpeed = normalSpinSpeed; // Default to normal spin speed

        if (isSpeedingUp)
        {
            spinSpeed = normalSpinSpeed * fastSpinMultiplier;
        }
        else if (isSlowingDown && tempoMeter > 0)
        {
            spinSpeed = normalSpinSpeed * slowSpinMultiplier;
        }

        // Apply spin speed to the SpinForever script
        spinForever.SetRotationSpeed(spinSpeed);
        Debug.Log($"Spin Speed: {spinSpeed}");
    }
}
