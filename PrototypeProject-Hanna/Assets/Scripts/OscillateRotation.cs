using UnityEngine;

public class OscillateRotation : MonoBehaviour
{
    public float rotationSpeed = 2.0f;  // Speed of oscillation
    public float minAngle = -15.0f;  // Minimum Z rotation
    public float maxAngle = 15.0f;   // Maximum Z rotation

    private float startZRotation;

    void Start()
    {
        startZRotation = transform.eulerAngles.z;
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * rotationSpeed) + 1) / 2; // Normalize to 0-1
        float zRotation = Mathf.Lerp(minAngle, maxAngle, t); // Lerp between min and max
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
    }
}
