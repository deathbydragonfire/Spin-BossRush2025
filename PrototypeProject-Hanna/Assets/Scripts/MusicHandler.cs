using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public AudioSource audioSource; // Attach the AudioSource component here
    public Transform record; // The spinning record GameObject
    public float normalSpinSpeed = 100f; // Default spin speed in degrees per second
    public float speedUpMultiplier = 1.5f; // Speed-up factor
    public float slowDownMultiplier = 0.5f; // Slow-down factor

    private float currentSpinSpeed;
    private bool isSpeedingUp = false;
    private bool isSlowingDown = false;

    void Start()
    {
        // Initialize spin speed to normal
        currentSpinSpeed = normalSpinSpeed;
    }

    void Update()
    {
        // Check for speed-up input
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartSpeedingUp();
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            StopSpeedingUp();
        }

        // Check for slow-down input
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartSlowingDown();
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            StopSlowingDown();
        }

        // Rotate the record based on the current spin speed
        RotateRecord();
    }

    void StartSpeedingUp()
    {
        isSpeedingUp = true;
        AdjustPlaybackAndSpinSpeed(speedUpMultiplier);
    }

    void StopSpeedingUp()
    {
        isSpeedingUp = false;
        AdjustPlaybackAndSpinSpeed(1.0f);
    }

    void StartSlowingDown()
    {
        isSlowingDown = true;
        AdjustPlaybackAndSpinSpeed(slowDownMultiplier);
    }

    void StopSlowingDown()
    {
        isSlowingDown = false;
        AdjustPlaybackAndSpinSpeed(1.0f);
    }

    void AdjustPlaybackAndSpinSpeed(float multiplier)
    {
        // Adjust music playback speed
        audioSource.pitch = multiplier;

        // Adjust spin speed of the record
        currentSpinSpeed = normalSpinSpeed * multiplier;
    }

    void RotateRecord()
    {
        if (record != null)
        {
            record.Rotate(Vector3.up, currentSpinSpeed * Time.deltaTime);
        }
    }
}
