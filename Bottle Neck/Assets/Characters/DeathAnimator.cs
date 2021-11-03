using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAnimator : StateMachineBehaviour
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.transform.parent.gameObject.GetComponent<EnemyMover>().dead = true;
        animator.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        if (animator.gameObject.transform.parent.gameObject.GetComponent<EnemyLife>().shop != null)
        {
            EnemyLife life = animator.gameObject.transform.parent.gameObject.GetComponent<EnemyLife>();
            Shop shop = animator.gameObject.transform.parent.gameObject.GetComponent<EnemyLife>().shop;
            shop.AddMoney(life.moneyDrop);

        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject.transform.parent.gameObject);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
