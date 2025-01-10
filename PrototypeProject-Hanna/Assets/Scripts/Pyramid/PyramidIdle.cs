using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PyramidIdle : StateMachineBehaviour
{
    [SerializeField] private Vector2 durationRange;
    private float duration;

    private Animator anim;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        anim = animator;
        duration = Random.Range(durationRange.x, durationRange.y);
        
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
        duration-= Time.deltaTime;
        if (duration <= 0f)
        {
            OnTimeIsUp();
            duration = int.MaxValue;
        }
    }

    private void OnTimeIsUp()
    {
        int x = Random.Range(1, 1);
        switch(x)
        {
            case 1:
                anim.SetTrigger("Attack1");
                break;
        }
    }
}
