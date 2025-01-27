using UnityEngine;

public class Attack1Shoot : StateMachineBehaviour
{
    [SerializeField] GameObject projectilePrefab; // The projectile prefab to spawn
    [SerializeField] Vector3 spawnOffset = new Vector3(0, 1, 0); // Offset from the animator's position
    [SerializeField] Transform record; // Reference to the record object

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile Prefab is not assigned!");
            return;
        }

        // Spawn the projectile
        GameObject projectile = Instantiate(
            projectilePrefab,
            animator.transform.position + spawnOffset, // Use spawn offset relative to the animator's position
            Quaternion.identity
        );

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Optional: Add logic for updating the attack during the animation state
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Optional: Add cleanup logic when exiting the attack state
    }
}
