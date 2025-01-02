using UnityEngine;

public class PlayerRighting : MonoBehaviour
{
    // Adjust the height to teleport above the terrain
    public float resetHeightOffset = 1f;

    // Input key for resetting the player
    public KeyCode resetKey = KeyCode.X;

    // Reference to the Rigidbody2D for physics adjustments
    private Rigidbody2D rb;

    void Start()
    {
        // Cache the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check for the reset input
        if (Input.GetKeyDown(resetKey))
        {
            RightPlayer();
        }
    }

    private void RightPlayer()
    {
        // Reset rotation to 0
        rb.rotation = 0;

        // Teleport the player upward
        Vector2 currentPosition = rb.position;
        rb.position = new Vector2(currentPosition.x, currentPosition.y + resetHeightOffset);

        // Reset velocity to prevent lingering forces
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        Debug.Log("Player righted and repositioned.");
    }
}
