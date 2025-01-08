using UnityEngine;
using System.Linq; // Required for LINQ methods like Select and Where

public class PlayerController3D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed of the player

    [Header("Playable Area Settings")]
    public string playableAreaLayerName = "PlayableArea"; // Name of the layer for playable areas

    private CharacterController characterController;
    private Collider[] playableAreaColliders;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (characterController == null)
        {
            Debug.LogError("CharacterController is required for PlayerController3D to function.");
        }

        // Find all colliders on objects in the specified layer
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        playableAreaColliders = allObjects
            .Where(obj => obj.layer == LayerMask.NameToLayer(playableAreaLayerName))
            .Select(obj => obj.GetComponent<Collider>())
            .Where(collider => collider != null)
            .ToArray();

        if (playableAreaColliders.Length == 0)
        {
            Debug.LogError($"No Colliders found on objects in the '{playableAreaLayerName}' layer.");
        }
    }

    void Update()
    {
        if (playableAreaColliders != null && playableAreaColliders.Length > 0)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        // Get input from the player
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Combine input into a direction vector
        Vector3 direction = new Vector3(horizontal, 0, vertical);

        if (direction.magnitude > 0.1f)
        {
            // Normalize the direction vector to avoid diagonal speed increase
            direction = direction.normalized;

            // Calculate potential new position
            Vector3 movement = direction * moveSpeed * Time.deltaTime;
            Vector3 potentialPosition = transform.position + movement;

            // Check if the new position is within any playable area collider
            if (IsWithinPlayableArea(potentialPosition))
            {
                characterController.Move(movement);
            }
        }
    }

    private bool IsWithinPlayableArea(Vector3 position)
    {
        foreach (Collider collider in playableAreaColliders)
        {
            if (collider.bounds.Contains(position))
            {
                return true;
            }
        }
        return false;
    }
}
