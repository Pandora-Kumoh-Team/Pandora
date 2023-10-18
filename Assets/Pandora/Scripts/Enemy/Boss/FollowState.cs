using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : StateMachineBehaviour
{
    Transform bossTransform;
    BossController enemyBoss;
    [SerializeField] private float followDistance = 4.0f;
    [SerializeField] private float moveToward = 1.0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyBoss = animator.GetComponent<BossController>();
        bossTransform = animator.GetComponent<Transform>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(enemyBoss.player.position, bossTransform.position) > followDistance) // ���ڸ��� ���ư���
        {
            animator.SetBool("isBack", true);
            animator.SetBool("isFollow", false);
        }
        else if (Vector2.Distance(enemyBoss.player.position, bossTransform.position) > moveToward) //���� �Ÿ� ������������ �÷��̾� ���󰡱�
        {
            bossTransform.position = Vector2.MoveTowards(bossTransform.position, enemyBoss.player.position, Time.deltaTime * enemyBoss.moveSpeed);
        }
        else // Ready ���� ��ȯ
        {
            animator.SetBool("isBack", false);
            animator.SetBool("isFollow", false);
        }
        //TODO ĳ���� �¿� ��ȯ Animator�� ��� ���߿� ���� �����ϱ�
        enemyBoss.DiretionEnemy(enemyBoss.player.position.x, enemyBoss.player.position.y);//�÷��̾� ���⿡ ���� ĳ���� �¿� ��ȯ�ϱ� ���ؼ�
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
