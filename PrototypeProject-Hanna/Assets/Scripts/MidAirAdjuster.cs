using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody2D))]
public class MidAirAdjuster : MonoBehaviour
{
    // Rotation adjustment variables
    public float rotationSpeed = 100f;  // Speed of mid-air rotation adjustments
    private Rigidbody2D rb;
    private PlayerController playerController;

    // Toggle debug gizmos in the editor
    public bool showGroundedDebugGizmo = true;
    public float debugGizmoRadius = 0.25f;

    void Start()
    {
        // Cache the Rigidbody2D and PlayerController components
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        // Allow mid-air adjustment only when the player is not grounded
        if (!playerController.GetIsGrounded())
        {
            HandleMidAirAdjustment();
        }
    }

    private void HandleMidAirAdjustment()
    {
        // Get player input for rotation
        float tiltInput = Input.GetAxis("Horizontal");

        // Apply rotational torque to adjust the bike’s tilt
        if (Mathf.Abs(tiltInput) > 0.01f)
        {
            rb.AddTorque(-tiltInput * rotationSpeed * Time.deltaTime, ForceMode2D.Force);
        }
    }

    /// <summary>
    /// Draws a small sphere gizmo at the transform's position,
    /// color-coded to show whether we're grounded or not.
    /// This only appears in the Scene View (not in the game).
    /// </summary>
    void OnDrawGizmos()
    {
        if (!showGroundedDebugGizmo) return;

        // If we haven't started yet or if there's no PlayerController attached, just return
        if (!Application.isPlaying || playerController == null) return;

        // Change gizmo color based on grounded state
        Gizmos.color = playerController.GetIsGrounded() ? Color.green : Color.red;

        // Draw a sphere at the object’s position
        Gizmos.DrawSphere(transform.position, debugGizmoRadius);
    }
}
