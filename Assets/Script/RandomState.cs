using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomState : StateMachineBehaviour
{
    #region Variables
    public int stateNumber;
    public float minRandomTime = 2;
    public float maxRandomTime = 5;

    private float _randomTime = 0;
    private int _randomIdle = Animator.StringToHash("RandomIdle");
    #endregion

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(_randomIdle, -1);
        _randomTime = Random.Range(minRandomTime, maxRandomTime);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(stateInfo.normalizedTime >= _randomTime && !animator.IsInTransition(0)){
           animator.SetInteger(_randomIdle, Random.Range(0, stateNumber));
       }
    }
}
