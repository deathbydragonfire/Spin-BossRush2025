using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VamVamLogic : MonoBehaviour
{
    public VamVamController controller; // Reference to Vam-Vam's controller
    public float idleTime = 3f; // Time between attacks
    private bool isAttacking = false; // To track attack state

    // Define a delegate-based attack system
    private delegate IEnumerator AttackMethod();
    private AttackMethod[] attackMethods;

    void Start()
    {
        // Assign attack methods to array
        attackMethods = new AttackMethod[]
        {
            controller.PerformSlashAttack, // Standard slash attack
            controller.PerformConcertAttack, // Concert laser show attack
            controller.StartVIPAreaAttack // VIP Area attack
        };

        StartCoroutine(BossLogicLoop());
    }

    IEnumerator BossLogicLoop()
    {
        while (true)
        {
            if (!isAttacking) // If not attacking, move & wait for next attack
            {
                StartCoroutine(HoverAround()); // Move around the screen
                yield return new WaitForSeconds(idleTime);
                ChooseAndPerformAttack();
            }
            yield return null;
        }
    }


    private void ChooseAndPerformAttack()
    {
        if (isAttacking) return; // Prevent overlapping attacks

        int randomIndex = Random.Range(0, attackMethods.Length); // Pick random attack
        StartCoroutine(PerformAttack(randomIndex));
    }

    private IEnumerator PerformAttack(int attackIndex)
    {
        isAttacking = true;
        yield return StartCoroutine(attackMethods[attackIndex]()); // Execute attack
        isAttacking = false;
    }
    private IEnumerator HoverAround()
    {
        Vector3 targetPosition = new Vector3(
            Random.Range(-7f, 7f),  // Random X position
            Random.Range(4f, 6f),   // Stay high in the air
            Random.Range(-3f, 3f)   // Random Z position
        );

        while (Vector3.Distance(controller.transform.position, targetPosition) > 0.1f)
        {
            controller.transform.position = Vector3.MoveTowards(
                controller.transform.position, targetPosition,
                controller.vamVamSpeed * Time.deltaTime
            );
            yield return null;
        }
    }
}

