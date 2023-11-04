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
    Vector3 attackRangePos;

    private void Start()
    {
        isConduct = false;
        timer = 0.0f;
        waitingTime = 1.0f;
        parentName = transform.parent.name;
        attackRangePos = GameObject.Find(parentName).transform.Find("AttackRange").transform.localPosition;
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
            }
            else if (randomMoveTime >= 3 && randomMoveTime < 6)
            {
                Vector3 ranVec = new Vector3(ranDir1, ranDir2, 0);
                transform.parent.position += ranVec * speed * Time.deltaTime;
                transform.parent.GetComponent<Animator>().SetFloat("Speed", ranVec.magnitude);
                if (ranVec.x < 0)
                    transform.parent.GetComponent<SpriteRenderer>().flipX = true;
                else
                    transform.parent.GetComponent<SpriteRenderer>().flipX = false;
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
            if (direction.x < 0)
            {
                transform.parent.GetComponent<SpriteRenderer>().flipX = true;
                GameObject.Find(parentName).transform.Find("AttackRange").transform.localPosition = new Vector3(-attackRangePos.x, attackRangePos.y, 0);
            }
            else
            {
                transform.parent.GetComponent<SpriteRenderer>().flipX = false;
                GameObject.Find(parentName).transform.Find("AttackRange").transform.localPosition = new Vector3(attackRangePos.x, attackRangePos.y, 0);
            }

            //공격 사정거리밖이면 범위 내의 플레이어를 추적
            if (distance > attackRange)
            {
                transform.parent.position += direction * speed * Time.deltaTime;
                transform.parent.GetComponent<Animator>().SetFloat("Speed", direction.magnitude);
            }
            //공격 사정거리에 들어왔을 경우 공격
            else
            {
                transform.parent.GetComponent<Animator>().SetTrigger("Attack");
                transform.parent.GetComponent<Animator>().SetFloat("Speed", 0);
                timer = 0;
                GameObject.Find(parentName).transform.Find("AttackRange").gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (target == collision.gameObject)
        {
            target = null;
            transform.parent.GetComponent<Animator>().SetFloat("Speed", 0);
            isConduct = false;
        }
    }
}
