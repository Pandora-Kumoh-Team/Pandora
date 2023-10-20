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
        if (Vector2.Distance(enemyBoss.player.position, bossTransform.position) > followDistance) // 제자리로 돌아가기
        {
            animator.SetBool("isBack", true);
            animator.SetBool("isFollow", false);
        }
        else if (Vector2.Distance(enemyBoss.player.position, bossTransform.position) > moveToward) //일정 거리 떨어져있으면 플레이어 따라가기
        {
            bossTransform.position = Vector2.MoveTowards(bossTransform.position, enemyBoss.player.position, Time.deltaTime * enemyBoss.moveSpeed);
        }
        else // Ready 상태 변환
        {
            animator.SetBool("isBack", false);
            animator.SetBool("isFollow", false);
        }
        //TODO 캐릭터 좌우 변환 Animator가 없어서 나중에 만들어서 적용하기
        enemyBoss.DiretionEnemy(enemyBoss.player.position.x, enemyBoss.player.position.y);//플레이어 방향에 따라서 캐릭터 좌우 변환하기 위해서
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
