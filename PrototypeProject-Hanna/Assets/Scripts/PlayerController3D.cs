using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed of movement along the radius
    public float rotationSpeed = 100f; // Speed of rotation along the circumference
    public float minRadius = 0.5f; // Minimum distance from the center
    public float maxRadius = 5f; // Maximum distance from the center
    private Animator animator;
    private bool continueCombo = false;

    private int comboStep = 0; // Current step in the combo
    private float comboTimer = 0f; // Timer to track the combo window
    [SerializeField] private float comboResetTime = 1f; // Time window to continue the combo

    private CharacterController characterController;
    private Vector3 discCenter = Vector3.zero; // Center of the spinning disc

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (characterController == null)
        {
            Debug.LogError("CharacterController is required for PlayerController3D to function.");
        }
    }

    void Update()
    {
        MovePlayerOnDisc();
        if (Input.GetButtonDown("Attack"))
        {
            Debug.Log("attack input");
            continueCombo = true;
            HandleComboAttack();
        }

        
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

        // Adjust vertical input to account for negative Z-axis alignment
        vertical = -vertical;
        animator.SetBool("IsRunning", horizontal > 0);
        animator.SetBool("isBackwards", horizontal < 0);
        // Move along the radius (toward or away from the center)
        if (Mathf.Abs(vertical) > 0.1f)
        {
            Vector3 directionToCenter = (playerPosition - discCenter).normalized;

            // Calculate the distance to the center
            float distanceToCenter = Vector3.Distance(playerPosition, discCenter);

            // Skip movement if it would exceed bounds
            if ((vertical > 0 && distanceToCenter >= maxRadius) || (vertical < 0 && distanceToCenter <= minRadius))
            {
                // Do nothing (skip movement)
            }
            else
            {
                // Apply valid radial movement
                Vector3 radiusMovement = directionToCenter * vertical * moveSpeed * Time.deltaTime;
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

            // Restrict circumferential movement to the front (-Z) half
            if (newPosition.z <= 0) // Only allow movement if the resulting position is in the front (-Z) half
            {
                
                movement += new Vector3(newPosition.x - playerPosition.x, 0, newPosition.z - playerPosition.z);
            }
        }

        // Apply the movement using the CharacterController
        characterController.Move(movement);
    }

    private void HandleComboAttack()
    {
        // Check the current combo step
        if (comboStep == 0) // First attack
        {
            Debug.Log("attack1");
            animator.SetTrigger("Attack1");
            comboStep = 1;
            continueCombo = false;
        }
        else if (comboStep == 1 && comboTimer > 0) // Second attack
        {
            Debug.Log("attack2");
            animator.SetTrigger("Attack2");
            comboStep = 2;
            continueCombo = false;
        }
        else if (comboStep == 2 && comboTimer > 0) // Third attack
        {
            Debug.Log("attack3");
            animator.SetTrigger("Attack3");
            comboStep = 0; // Reset combo after the third attack
            continueCombo = false;
        }

        // Reset the combo timer for the next input
        comboTimer = comboResetTime;
    }

        public void ResetCombo()
        {
            if (!continueCombo)
            {
                comboStep = 0;
                comboTimer = 0f;
                Debug.Log("Combo Reset");
            } else
                {
                Debug.Log("Combo Continues");
               }
            continueCombo = false;
         }

    private void OnDrawGizmos()
    {
        // Draw the minimum radius in red
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(discCenter, minRadius);

        // Draw the maximum radius in green
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(discCenter, maxRadius);

        // Draw a line to visualize the front (-Z) half boundary
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(discCenter, discCenter + new Vector3(maxRadius, 0, 0)); // Right
        Gizmos.DrawLine(discCenter, discCenter + new Vector3(-maxRadius, 0, 0)); // Left
    }
}
