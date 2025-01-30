using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DJLogic : MonoBehaviour
{
    public DJEmperorController controller; // Reference to DJ's controller
    public float idleTime = 2f; // Time between attacks
    public Transform player; // Reference to player (assign this in Inspector)
    public float stingAttackRange = 5f; // Range to allow Sting Attack
    private bool isAttacking = false; // To track attack state

    // Define a delegate-based attack system
    private delegate IEnumerator AttackMethod();
    private AttackMethod[] attackMethods;

    void Start()
    {
        // Assign attack methods correctly using method references, NOT calling them
        attackMethods = new AttackMethod[]
        {
            controller.StingRoutine,
            controller.ERAERASequence
        };

        StartCoroutine(BossLogicLoop());
    }

    IEnumerator BossLogicLoop()
    {
        while (true)
        {
            if (!isAttacking) // Only choose attack if not already attacking
            {
                yield return new WaitForSeconds(idleTime);
                ChooseAndPerformAttack();
            }
            yield return null;
        }
    }

    private void ChooseAndPerformAttack()
    {
        if (isAttacking) return; // Don't interrupt ongoing attacks

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        List<int> validAttacks = new List<int>();

        // Check if player is within range for Sting Attack
        for (int i = 0; i < attackMethods.Length; i++)
        {
            if (i != 0 || distanceToPlayer <= stingAttackRange) // Only allow Sting if player is close
            {
                validAttacks.Add(i);
            }
        }

        if (validAttacks.Count == 0) return; // If no valid attacks, do nothing

        int randomIndex = validAttacks[Random.Range(0, validAttacks.Count)];
        StartCoroutine(PerformAttack(randomIndex));
    }

    private IEnumerator PerformAttack(int attackIndex)
    {
        isAttacking = true;
        yield return StartCoroutine(attackMethods[attackIndex]());
        isAttacking = false;
    }
}
