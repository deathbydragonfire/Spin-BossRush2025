using UnityEngine;

public class Attack1Shoot : StateMachineBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Vector3 spawnPos;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Instantiate(projectilePrefab , spawnPos + animator.transform.position , Quaternion.identity);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}