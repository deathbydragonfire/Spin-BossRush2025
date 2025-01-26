using UnityEngine;
using System.Collections;

public class MusicHandler : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public UnityEngine.Transform record; // The spinning record GameObject

    [Header("Playback Settings")]
    public float normalPlaybackSpeed = 1.0f; // Default playback speed
    public float speedUpMultiplier = 1.5f; // Speed-up factor
    public float slowDownMultiplier = 0.5f; // Slow-down factor

    [Header("Tempo Meter Settings")]
    public float maxTempo = 100f; // Maximum tempo meter value
    public float tempoBurnRate = 10f; // How quickly tempo burns when slowing down
    public float tempoBuildRate = 5f; // How quickly tempo builds when speeding up
    private float currentTempo; // Current tempo meter value

    private float currentPlaybackSpeed; // Current playback speed
    public float currentSpinSpeed; // Current spin speed
    public void PushAndPull(float pullSpeed, float pushSpeed, float duration)
    {
        StartCoroutine(PushAndPullSequence(pullSpeed, pushSpeed, duration));
    }

    void Start()
    {
        // Initialize values
        currentPlaybackSpeed = normalPlaybackSpeed;
        currentSpinSpeed = 100f; // Default spin speed
        currentTempo = maxTempo; // Start with a full tempo meter
    }

    void Update()
    {
        // Handle input for speeding up and slowing down
        if (Input.GetKey(KeyCode.Q))
        {
            StartSpeedingUp();
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            StopSpeedingUp();
        }

        if (Input.GetKey(KeyCode.E))
        {
            StartSlowingDown();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            StopSlowingDown();
        }

        // Rotate the record
        RotateRecord();
    }

    void StartSpeedingUp()
    {
        ;
        AdjustPlaybackAndSpinSpeed(speedUpMultiplier);

        // Build tempo while speeding up
        currentTempo = Mathf.Min(currentTempo + tempoBuildRate * Time.deltaTime, maxTempo);
    }

    void StopSpeedingUp()
    {

        AdjustPlaybackAndSpinSpeed(normalPlaybackSpeed);
    }

    void StartSlowingDown()
    {
        if (currentTempo > 0)
        {
            AdjustPlaybackAndSpinSpeed(slowDownMultiplier);

            // Burn tempo while slowing down
            currentTempo = Mathf.Max(currentTempo - tempoBurnRate * Time.deltaTime, 0);

            // Stop slowing if tempo reaches zero
            if (currentTempo <= 0)
            {
                StopSlowingDown();
            }
        }
        else
        {
            StopSlowingDown();
        }
    }

    void StopSlowingDown()
    {
        AdjustPlaybackAndSpinSpeed(normalPlaybackSpeed);
    }

    void AdjustPlaybackAndSpinSpeed(float multiplier)
    {
        // Adjust playback speed
        currentPlaybackSpeed = normalPlaybackSpeed * multiplier;
        audioSource.pitch = currentPlaybackSpeed;

        // Adjust spin speed
        currentSpinSpeed = 100f * multiplier; // Adjust this value as needed
    }

    void RotateRecord()
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
    public IEnumerator PushAndPullSequence(float pullSpeed, float pushSpeed, float duration)
    {
        // Step 1: Pull the record (reverse spin direction)
        AdjustPlaybackAndSpinSpeed(-pullSpeed); // Negative speed for pulling
        yield return new WaitForSeconds(duration / 2f); // Half the duration for pulling

        // Step 2: Push the record (increase spin speed forward)
        AdjustPlaybackAndSpinSpeed(pushSpeed);
        yield return new WaitForSeconds(duration / 2f);

        // Step 3: Reset to normal playback and spin speed
        AdjustPlaybackAndSpinSpeed(normalPlaybackSpeed);
    }
}
