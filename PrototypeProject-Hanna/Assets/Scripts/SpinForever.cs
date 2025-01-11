using UnityEngine;

public class SpinForever : MonoBehaviour
{
    public float rotationSpeed = 360f; // Default rotation speed in degrees per second
    private Vector3 rotationAxis = Vector3.up; // Default rotation axis (Y-axis)

    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }

    public void SetRotationSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
    }
}
