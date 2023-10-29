using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ResetState : StateMachineBehaviour
{
    public string stateName;
    public bool status;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(stateName, status);
    }
}