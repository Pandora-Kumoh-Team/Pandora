using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyState : StateMachineBehaviour
{
    Transform bossTransform;
    BossController enemyBoss;
    [SerializeField] private float followDistance = 1.0f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyBoss = animator.GetComponent<BossController>();
        bossTransform = animator.GetComponent<Transform>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemyBoss.attackDelay <= 0) //�����̰� 0���� ������ ���Ÿ� ����
        {
            animator.SetTrigger("Attack");

        }else if (Vector2.Distance(enemyBoss.player.position, bossTransform.position) > followDistance)
        {
            animator.SetBool("isFollow", true);
        }

        enemyBoss.DiretionEnemy(enemyBoss.player.position.x, bossTransform.position.x);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
