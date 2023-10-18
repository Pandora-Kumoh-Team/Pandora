using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackState : StateMachineBehaviour
{
    Transform bossTransform;
    BossController enemyBoss;
    [SerializeField] private float followDistance = 4.0f;
    [SerializeField] private float backDistance = 0.1f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyBoss = animator.GetComponent<BossController>();
        bossTransform = animator.GetComponent<Transform>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Vector2.Distance(enemyBoss.home, bossTransform.position) < backDistance || Vector2.Distance(bossTransform.position, enemyBoss.player.position) < followDistance)
        {
            animator.SetBool("isBack", false);
        }
        else
        {
            enemyBoss.DiretionEnemy(enemyBoss.home.x, bossTransform.position.x);
            bossTransform.position = Vector2.MoveTowards(bossTransform.position, enemyBoss.home, Time.deltaTime * enemyBoss.moveSpeed);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }


}
