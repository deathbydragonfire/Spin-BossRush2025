using UnityEngine;
using System.Collections;

public class ZapRiotLogic : MonoBehaviour
{
    public ZapRiotController controller;
    private bool isAttacking = false;
    private float attackCooldown = 1.5f;

    private delegate IEnumerator AttackMethod();
    private AttackMethod[] attackMethods;




    public IEnumerator BossLogicLoop()
    {
        while (true)
        {
            if (!isAttacking)
            {
                yield return new WaitForSeconds(attackCooldown);
                ChooseAndPerformAttack(); // ✅ Calls attack selection properly!
            }
            yield return null;
        }
    }





    private void ChooseAndPerformAttack()
    {
        if (isAttacking) return;

        AttackMethod chosenAttack = WeightedAttackSelection();
        StartCoroutine(PerformAttack(chosenAttack));
    }

    private IEnumerator PerformAttack(AttackMethod attack)
    {
        isAttacking = true;
        yield return StartCoroutine(attack());
        Debug.Log("Attack complete!");
        isAttacking = false; // ✅ Make sure he can attack again
    }

    private AttackMethod WeightedAttackSelection()
    {
        int roll = Random.Range(1, 101); // Roll 1-100

        if (roll <= 60) return controller.PerformSlashSequence;  // 60% chance
        else if (roll <= 80) return controller.SpeedUpAttack; // 20% chance
        else return controller.LightningAttack; // 20% chance
    }
}
