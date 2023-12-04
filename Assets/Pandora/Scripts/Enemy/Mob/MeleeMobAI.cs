using System;
using Pandora.Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using Pandora.Scripts.DebugConsole.Player;
using Pandora.Scripts.Player;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeMobAI : MonoBehaviour
{
    // Components
    private Animator animator;
    private Rigidbody2D rb;
    private EnemyController enemyController;
    private GameObject parent;
    private GameObject dangerRange;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D collider;
    
    private float randomMoveTime;
    private bool isConduct;
    private float ranDir1;
    private float ranDir2;

    private Vector2 direction;
    private GameObject target;
    private Vector2 targetPos;
    private float nowTargetDistance;

    //Status
    [HideInInspector]
    public float speed;

    public Vector2 attackOffset; //임시
    private Vector3 attackRangePos;
    private Vector2 capOffset;

    private bool isAttacking;
    public float attackBeforeDelay = 0.3f;
    public float attackingTime = 0.2f;
    public float attackAfterDelay = 0.3f;
    private bool canAttack = true;

    private void Start()
    {
        isConduct = false;
        parent = transform.parent.gameObject;
        capOffset = parent.GetComponent<CapsuleCollider2D>().offset;
        enemyController = parent.GetComponent<EnemyController>();
        animator = parent.GetComponent<Animator>();
        spriteRenderer = parent.GetComponent<SpriteRenderer>();
        collider = parent.GetComponent<CapsuleCollider2D>();
        rb = parent.GetComponent<Rigidbody2D>();
        dangerRange = parent.transform.Find("AttackRange").gameObject;
        attackRangePos =dangerRange.transform.localPosition;
        speed = enemyController._enemyStatus.Speed;
        nowTargetDistance = float.MaxValue;
    }

    private void Update()
    {
        //피격 시 경직 시간 초기화
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            animator.SetFloat("Speed", 0);
            rb.velocity = Vector2.zero;
        }

        if (!isConduct) //어떠한 행동도 하고 있지 않을때
        {
            //랜덤하게 이동
            if (randomMoveTime == 0)
            {
                ranDir1 = Random.Range(-1f, 1f);
                ranDir2 = Random.Range(-1f, 1f);
                animator.SetFloat("Speed", 0);
                rb.velocity = Vector2.zero;
            }
            else if (randomMoveTime >= 3 && randomMoveTime < 6)
            {
                Vector3 ranVec = new Vector3(ranDir1, ranDir2, 0);
                //transform.parent.position += ranVec * speed * Time.deltaTime;
                // rigidbody로 변경
                rb.velocity = ranVec * speed;
                animator.SetFloat("Speed", ranVec.magnitude);
                Flip(ranVec);
            }
        }
        randomMoveTime += Time.deltaTime;

        if (randomMoveTime >= 6)
            randomMoveTime = 0;

        speed = enemyController._enemyStatus.Speed;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //플레이어 식별
        if (!collision.gameObject.CompareTag("Player")) return;
        if (target == null) target = collision.gameObject;
        if (isAttacking)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        // 타겟 좌우 점 찾기
        var nowTargetPos = (Vector2)collision.transform.position;
        var targetLeftPos = nowTargetPos + attackOffset;
        var targetRightPos = nowTargetPos + new Vector2(attackOffset.x * -1, attackOffset.y);
        
        // 좌우 점 과의 거리 계산
        var myPos = transform.parent.position;
        var leftPosDistance = Vector2.Distance(myPos, targetLeftPos);
        var rightPosDistance = Vector2.Distance(myPos, targetRightPos);
        
        nowTargetDistance = Vector2.Distance(myPos, target.transform.position);
        
        // 좌우 점 중 가까운 점을 타겟으로 설정
        if (leftPosDistance < nowTargetDistance)
        {
            nowTargetDistance = leftPosDistance;
            targetPos = targetLeftPos;
            target = collision.gameObject;
        }
        if (rightPosDistance < nowTargetDistance)
        {
            nowTargetDistance = rightPosDistance;
            targetPos = targetRightPos;
            target = collision.gameObject;
        }
        
        // 타겟과 어느정도 공격범위내에 있다면
        var x = dangerRange.GetComponent<BoxCollider2D>().size.x;
        var y = dangerRange.GetComponent<BoxCollider2D>().size.y;
        var maxDistance = Mathf.Sqrt(x * x + y * y);
        if(nowTargetDistance <= maxDistance)
        {
            // targetPos가 벽 너머에 있거나 콜라이더 크기때문에 접근 불가하다면 강제 공격 
            var raycastHit = Physics2D.Raycast(collider.bounds.center, targetPos - (Vector2)myPos, 3f,
                LayerMask.GetMask("Wall", "Player"));
            if (raycastHit.collider != null)
            {
                if (raycastHit.distance <
                    Math.Sqrt(collider.size.x * collider.size.x + collider.size.y * collider.size.y) / 2)
                {
                    if (canAttack)
                        StartCoroutine(Attack());
                    return;
                }
            }
        }
        
        //플레이어와의 거리 측정
        var distance = Vector2.Distance(myPos, targetPos);

        //대기시간이 아닐 경우
        if (!isAttacking && target == collision.gameObject)
        {
            isConduct = true;
            direction = targetPos - (Vector2)transform.parent.position;
            direction.Normalize();

            //방향 전환
            var lookDir = target.transform.position - myPos;
            Flip(lookDir);

            //공격 사정거리밖이면 범위 내의 플레이어를 추적
            if (distance > 0.1f)
            {
                // transform.parent.position += direction * speed * Time.deltaTime;
                rb.velocity = direction * speed;
                animator.SetFloat("Speed", direction.magnitude);
            }
            //공격 사정거리에 들어왔을 경우
            else if(canAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        canAttack = false;
        // get AttackAnimation speed
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Attack");
        animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(attackBeforeDelay);
        dangerRange.SetActive(true);
        yield return new WaitForSeconds(attackingTime);
        dangerRange.SetActive(false);
        yield return new WaitForSeconds(attackAfterDelay);
        animator.SetBool("IsAttacking", false);
        animator.SetFloat("Speed", 0);
        isAttacking = false;
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (target == collision.gameObject)
        {
            target = null;
            animator.SetFloat("Speed", 0);
            rb.velocity = Vector2.zero;
            isConduct = false;
        }
    }

    private void Flip(Vector3 direction)
    {
        EnemyStatus enemyStatus = enemyController._enemyStatus;

        if ( enemyStatus.Code >= 150 && enemyStatus.Code <= 199) //왼쪽보고 있는 금쪽이들
        {
            if (direction.x > 0)
            {
                spriteRenderer.flipX = true;
                dangerRange.transform.localPosition = new Vector3(-attackRangePos.x, attackRangePos.y, 0);
                collider.offset = new Vector2(-capOffset.x, capOffset.y);
            }
            else
            {
                spriteRenderer.flipX = false;
                dangerRange.transform.localPosition = new Vector3(attackRangePos.x, attackRangePos.y, 0);
                collider.offset = new Vector2(capOffset.x, capOffset.y);
            }
        }
        else
        {
            if (direction.x < 0)
            {
                spriteRenderer.flipX = true;
                dangerRange.transform.localPosition = new Vector3(-attackRangePos.x, attackRangePos.y, 0);
                collider.offset = new Vector2(-capOffset.x, capOffset.y);
            }
            else
            {
                spriteRenderer.flipX = false;
                dangerRange.transform.localPosition = new Vector3(attackRangePos.x, attackRangePos.y, 0);
                collider.offset = new Vector2(capOffset.x, capOffset.y);
            }
        }
    }
}
