using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DJLogic : MonoBehaviour
{
    public DJEmperorController controller; // Reference to DJ's controller
    public float idleTime = 2f; // Time between attacks
    public Transform player; // Reference to player (assign this in Inspector)
    public float stingAttackRange = 10f; // Range to allow Sting Attack
    private bool isAttacking = false; // To track attack state

    // Define a delegate-based attack system
    private delegate IEnumerator AttackMethod();
    private AttackMethod[] attackMethods;

    void Start()
    {

        if (controller == null)
        {
            Debug.LogError("DJLogic: Controller is not assigned!");
            return;
        }
        // Assign attack methods correctly using method references, NOT calling them
        attackMethods = new AttackMethod[]
        {
            controller.StingRoutine,
            controller.ERAERASequence,
            controller.PoisonAttackRoutine
        };
        StartCoroutine(BossLogicLoop());
    }

    IEnumerator BossLogicLoop()
    {
        Debug.Log("[DJLogic] Boss Logic Loop Started!"); // ADD THIS
        while (true)
        {
            if (!isAttacking) // Only choose attack if not already attacking
            {
                yield return new WaitForSeconds(idleTime);
                Debug.Log("[DJLogic] Choosing Attack Now..."); // ADD THIS
                ChooseAndPerformAttack();
            }
            yield return null;
        }
    }


    private void ChooseAndPerformAttack()
    {
        if (isAttacking)
        {
            Debug.Log("[DJLogic] Already attacking, skipping...");
            return; // Don't interrupt ongoing attacks
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        List<int> validAttacks = new List<int>();

        for (int i = 0; i < attackMethods.Length; i++)
        {
            if (i == 0 && distanceToPlayer > stingAttackRange) continue; // Only allow Sting if player is close
            validAttacks.Add(i);
        }

        Debug.Log($"[DJLogic] Found {validAttacks.Count} valid attacks.");

        if (validAttacks.Count == 0) return; // If no valid attacks, do nothing

        // **Include Poison Attack as an Option (30% Chance)**
        if (Random.value <= 0.3f)
        {
            Debug.Log("[DJLogic] Choosing Poison Attack!");
            controller.PerformPoisonAttack();
            return; // Prevent choosing another attack
        }

        int randomIndex = validAttacks[Random.Range(0, validAttacks.Count)];
        Debug.Log($"[DJLogic] Selecting attack index {randomIndex}...");
        StartCoroutine(PerformAttack(randomIndex));
    }

    private IEnumerator PerformAttack(int attackIndex)
    {
        Debug.Log($"[DJLogic] Executing attack {attackIndex}...");
        isAttacking = true;

        yield return StartCoroutine(attackMethods[attackIndex]()); // This should trigger the attack

        Debug.Log($"[DJLogic] Attack {attackIndex} finished.");
        isAttacking = false;
    }
}