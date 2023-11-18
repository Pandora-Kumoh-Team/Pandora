using Pandora.Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMobAI : MonoBehaviour
{
    private float timer;
    private float waitingTime;
    private float randomMoveTime;
    private bool isConduct;
    private float ranDir1;
    private float ranDir2;

    // Components
    private Vector3 direction;
    private GameObject target;
    private string parentName;

    //Status
    public float speed = 1.0f; //임시

    public float attackRange = 0.5f; //임시
    private Vector3 attackRangePos;
    private Vector2 capOffset;

    private void Start()
    {
        isConduct = false;
        timer = 0.0f;
        waitingTime = 1.0f;
        parentName = transform.parent.name;
        attackRangePos = GameObject.Find(parentName).transform.Find("AttackRange").transform.localPosition;
        capOffset = transform.parent.GetComponent<CapsuleCollider2D>().offset;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.5)
            transform.parent.transform.Find("AttackRange").gameObject.SetActive(false);

        //피격 시 경직 시간 초기화
        if(transform.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            transform.parent.GetComponent<Animator>().SetFloat("Speed", 0);
            transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            timer = 0.8f;
        }

        if (!isConduct) //어떠한 행동도 하고 있지 않을때
        {
            //랜덤하게 이동
            if (randomMoveTime == 0)
            {
                ranDir1 = Random.Range(-1f, 1f);
                ranDir2 = Random.Range(-1f, 1f);
                transform.parent.GetComponent<Animator>().SetFloat("Speed", 0);
                transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            else if (randomMoveTime >= 3 && randomMoveTime < 6)
            {
                Vector3 ranVec = new Vector3(ranDir1, ranDir2, 0);
                //transform.parent.position += ranVec * speed * Time.deltaTime;
                // rigidbody로 변경
                transform.parent.GetComponent<Rigidbody2D>().velocity = ranVec * speed;
                transform.parent.GetComponent<Animator>().SetFloat("Speed", ranVec.magnitude);
                Flip(ranVec);
            }
        }
        randomMoveTime += Time.deltaTime;

        if (randomMoveTime >= 6)
            randomMoveTime = 0;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        float distance = 0.0f;

        //플레이어 식별
        if (collision.gameObject.tag == "Player" && target == null)
            target = collision.gameObject;

        //플레이어와의 거리 측정
        if (target != null)
            distance = Vector2.Distance(transform.parent.position, target.transform.position);

        //대기시간이 아닐 경우
        if (timer > waitingTime && target == collision.gameObject)
        {
            isConduct = true;
            direction = target.transform.position - transform.parent.position;
            direction.Normalize();

            //방향 전환
            Flip(direction);

            //공격 사정거리밖이면 범위 내의 플레이어를 추적
            if (distance > attackRange)
            {
                // transform.parent.position += direction * speed * Time.deltaTime;
                transform.parent.GetComponent<Rigidbody2D>().velocity = direction * speed;
                transform.parent.GetComponent<Animator>().SetFloat("Speed", direction.magnitude);
            }
            //공격 사정거리에 들어왔을 경우 공격
            else
            {
                transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                transform.parent.GetComponent<Animator>().SetFloat("Speed", 0);
                transform.parent.GetComponent<Animator>().SetTrigger("Attack");
                timer = 0;
                transform.parent.Find("AttackRange").gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (target == collision.gameObject)
        {
            target = null;
            transform.parent.GetComponent<Animator>().SetFloat("Speed", 0);
            transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            isConduct = false;
        }
    }

    private void Flip(Vector3 direction)
    {
        //TODO : 스프라이트 자체가 중앙에서 너무 먼 몹의 경우 플립했을 때 순간이동하는 듯한 현상있음. 플립할때 보정을 해주는 식으로 수정 필요
        // 보정 값을 몹status에서 가지고 있는 것이 나을 듯

        EnemyStatus enemyStatus = GameObject.Find(parentName).GetComponent<EnemyController>()._enemyStatus;

        if ( enemyStatus.Code >= 103 && enemyStatus.Code <= 106) //기존에 오른쪽보고 있는 금쪽이들
        {
            if (direction.x > 0)
            {
                transform.parent.GetComponent<SpriteRenderer>().flipX = true;
                transform.parent.Find("AttackRange").transform.localPosition = new Vector3(-attackRangePos.x, attackRangePos.y, 0);
                transform.parent.GetComponent<CapsuleCollider2D>().offset = new Vector2(-capOffset.x, capOffset.y);
            }
            else
            {
                transform.parent.GetComponent<SpriteRenderer>().flipX = false;
                transform.parent.Find("AttackRange").transform.localPosition = new Vector3(attackRangePos.x, attackRangePos.y, 0);
                transform.parent.GetComponent<CapsuleCollider2D>().offset = new Vector2(capOffset.x, capOffset.y);
            }
        }
        else
        {
            if (direction.x < 0)
            {
                transform.parent.GetComponent<SpriteRenderer>().flipX = true;
                transform.parent.Find("AttackRange").transform.localPosition = new Vector3(-attackRangePos.x, attackRangePos.y, 0);
                transform.parent.GetComponent<CapsuleCollider2D>().offset = new Vector2(-capOffset.x, capOffset.y);
            }
            else
            {
                transform.parent.GetComponent<SpriteRenderer>().flipX = false;
                transform.parent.Find("AttackRange").transform.localPosition = new Vector3(attackRangePos.x, attackRangePos.y, 0);
                transform.parent.GetComponent<CapsuleCollider2D>().offset = new Vector2(capOffset.x, capOffset.y);
            }
        }
    }
}
