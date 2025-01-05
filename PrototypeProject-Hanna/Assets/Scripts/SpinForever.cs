using UnityEngine;

public class SpinForever : MonoBehaviour
{
    // Rotation speed in degrees per second
    public float rotationSpeed = 100f;

    // Axis of rotation (change if needed)
    private Vector3 rotationAxis = Vector3.up; // Default: Y-axis

    void Update()
    {
        // Rotate the object based on the rotation speed and deltaTime
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }

    // Optional: Allow changing the axis in code
    public void SetRotationAxis(Vector3 axis)
    {
        rotationAxis = axis.normalized;
    }
}
