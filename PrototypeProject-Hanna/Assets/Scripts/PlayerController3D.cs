using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed of movement along the radius
    public float rotationSpeed = 100f; // Speed of rotation along the circumference
    public float minRadius = 0.5f; // Minimum distance from the center to avoid getting stuck

    private CharacterController characterController;
    private Vector3 discCenter = Vector3.zero; // Center of the spinning disc

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (characterController == null)
        {
            Debug.LogError("CharacterController is required for PlayerController3D to function.");
        }
    }

    void Update()
    {
        MovePlayerOnDisc();
    }

    private void MovePlayerOnDisc()
    {
        // Get input from the player
        float horizontal = Input.GetAxis("Horizontal"); // Left/Right input
        float vertical = Input.GetAxis("Vertical");     // Up/Down input

        // Get the player's current position
        Vector3 playerPosition = transform.position;

        // Movement vector for the CharacterController
        Vector3 movement = Vector3.zero;

        // Move along the radius (toward or away from the center)
        if (Mathf.Abs(vertical) > 0.1f)
        {
            Vector3 directionToCenter = (playerPosition - discCenter).normalized;

            // Prevent movement toward the center if within the minimum radius
            float distanceToCenter = Vector3.Distance(playerPosition, discCenter);
            if (distanceToCenter > minRadius)
            {
                Vector3 radiusMovement = -directionToCenter * vertical * moveSpeed * Time.deltaTime; // Negative for inward movement
                movement += new Vector3(radiusMovement.x, 0, radiusMovement.z); // Ignore Y-axis
            }
        }

        // Move along the circumference (rotate around the disc's center)
        if (Mathf.Abs(horizontal) > 0.1f)
        {
            // Calculate the rotation angle
            float angle = -horizontal * rotationSpeed * Time.deltaTime;

            // Offset the player from the center of the disc
            Vector3 offset = playerPosition - discCenter;

            // Apply rotation around the center
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 rotatedOffset = rotation * offset;

            // Calculate the new position after rotation
            Vector3 newPosition = discCenter + rotatedOffset;

            // Add the change in position to the movement vector
            movement += new Vector3(newPosition.x - playerPosition.x, 0, newPosition.z - playerPosition.z); // Ignore Y-axis
        }

        // Apply the movement using the CharacterController
        characterController.Move(movement);
    }
}
