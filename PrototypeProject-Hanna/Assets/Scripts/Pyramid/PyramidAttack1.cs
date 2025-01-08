using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PyramidAttack1 : StateMachineBehaviour
{
    [SerializeField] private Vector2 durationRange;
    [SerializeField] private GameObject headPrefab;
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] private Quaternion spawnRot;
    private GameObject headObj;
    private float duration;

    private Animator anim;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        anim = animator;
        duration = Random.Range(durationRange.x, durationRange.y);

        headObj = Instantiate(headPrefab , spawnPos , spawnRot);
        headObj.transform.parent = null;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Timer();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(headObj);
    }

    private void Timer()
    {
        duration -= Time.deltaTime;
        if (duration <= 0f)
        {
            OnTimeIsUp();
            duration = int.MaxValue;
        }
    }

    private void OnTimeIsUp()
    {
        anim.SetTrigger("Idle");
    }
}
