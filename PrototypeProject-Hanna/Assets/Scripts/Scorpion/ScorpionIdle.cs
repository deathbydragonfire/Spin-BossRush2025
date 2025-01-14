using UnityEngine;

public class ScorpionIdle : StateMachineBehaviour
{
    private Animator animator;
    [SerializeField] private Vector2 rangeOfDuration;
    [SerializeField] private string[] nextStates;
    private float duration;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
        duration = Random.Range(rangeOfDuration.x , rangeOfDuration.y);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Timer();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    private void Timer()
    {
        Debug.Log(Mathf.Round(duration * 100) / 100f);
        duration-= Time.deltaTime;
        if(duration <= 0)
        {
            OnTimerEnds();
            duration = float.MaxValue;
        }
    }
   
    private void OnTimerEnds()
    {
        int nextStateIndex = Random.Range(0 , nextStates.Length);
        animator.SetTrigger(nextStates[nextStateIndex]);
    }
}
