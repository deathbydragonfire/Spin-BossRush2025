using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    public float acceleration = 10f;    // Acceleration speed
    public float maxSpeed = 15f;        // Maximum movement speed
    public float brakeForce = 20f;      // Braking force
    public bool isFacingRight = true;   // Tracks current facing direction
    public float stopThreshold = 0.1f; // Speed below which the bike is considered "stopped"

    // Ground check
    public LayerMask groundLayer;       // Assign this to the "Ground" layer in the Inspector
    public Transform groundCheck;       // Empty GameObject placed slightly below the bike
    public float groundCheckRadius = 0.2f; // Radius for ground detection
    public bool isGrounded { get; private set; } // Public getter for grounded state

    // Components
    private Rigidbody2D rb;

    // Input variables
    private float moveInput;

    void Start()
    {
        // Cache the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get horizontal input
        moveInput = Input.GetAxis("Horizontal");

        // Check if the player is grounded
        CheckGrounded();

        // Check for direction change
        if (isGrounded)
        {
            HandleDirectionChange();
        }
    }

    void FixedUpdate()
    {
        // Handle movement physics
        if (isGrounded)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        // Apply horizontal force based on input
        if (moveInput != 0)
        {
            rb.AddForce(new Vector2(moveInput * acceleration, 0f), ForceMode2D.Force);

            // Limit speed
            if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed)
            {
                rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * maxSpeed, rb.linearVelocity.y);
            }
        }

        // Apply braking if no input
        if (moveInput == 0 && Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x * (1 - brakeForce * Time.fixedDeltaTime), rb.linearVelocity.y);
        }
    }

    private void HandleDirectionChange()
    {
        // Flip the bike if the player changes direction, is grounded, and has stopped or nearly stopped
        if (Mathf.Abs(rb.linearVelocity.x) < stopThreshold)
        {
            if (moveInput > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (moveInput < 0 && isFacingRight)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        // Flip the direction
        isFacingRight = !isFacingRight;

        // Multiply the scale's X-axis by -1 to flip the sprite
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;

        Debug.Log("Player flipped direction.");
    }

    private void CheckGrounded()
    {
        // Use a Physics2D.OverlapCircle to check for the ground layer
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }
}

 
