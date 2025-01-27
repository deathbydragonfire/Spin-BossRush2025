using UnityEngine;

public class LockPlayerRotation : MonoBehaviour
{
    private Quaternion initialRotation; // To store the player's initial rotation

    void Start()
    {
        // Store the initial rotation when the game starts
        initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        // Constantly reset the player's rotation to the initial value
        transform.rotation = initialRotation;
    }
}
