using UnityEngine;

public class ScorpionAttack1 : StateMachineBehaviour
{
    private Animator animator;
    [SerializeField] private Vector2 rangeOfDuration;
    [SerializeField] private Vector2 rangeOfAttackRate;

    private float duration;
    private float attackRate;

    private static float remainingDuration = -1;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;

        duration = remainingDuration < 0 ? Random.Range(rangeOfDuration.x, rangeOfDuration.y) : remainingDuration;
        attackRate = Random.Range(rangeOfAttackRate.x, rangeOfAttackRate.y);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Timer();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        remainingDuration = duration;

        if (animator.GetNextAnimatorStateInfo(layerIndex).IsName("Idle"))
        {
            ResetTimer();
        }
    }

    private void Timer()
    {
        duration -= Time.deltaTime;
        attackRate -= Time.deltaTime;

        if (duration <= 0)
        {
            animator.SetTrigger("Idle");
            duration = float.MaxValue; 
        }

        if (attackRate <= 0)
        {
            animator.SetTrigger("Shoot");
            attackRate = float.MaxValue;
        }
    }

    private void ResetTimer()
    {
        remainingDuration = -1;
    }
}