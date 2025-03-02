using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JS
{
    public class ResetAnimatorBool : StateMachineBehaviour
    {
        CharaterManager controller;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (controller == null)
            {
                controller = animator.GetComponent<CharaterManager>();
            }

            controller.isPerformingAction = false;
            controller.applyRootMotion = false;
            controller.canMove = true;
            controller.canRotate = true;
            //animator.SetBool("isPerformingAction", false);
            //animator.SetBool("isPerformingBackStep", false);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

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
}
