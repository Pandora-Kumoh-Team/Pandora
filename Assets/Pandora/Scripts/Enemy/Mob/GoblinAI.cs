using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAI : MonoBehaviour
{
    // Components
    public float speed = 1f; //임시
    private Vector3 direction;
    private GameObject target;
    public float attackRange = 2f; //임시

    private float timer;
    private int waitingTime;

    private void Start()
    {
        timer = 0.0f;
        waitingTime = 2;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.5)
            transform.parent.transform.Find("AttackRange").gameObject.SetActive(false);
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

        if (timer > waitingTime && target == collision.gameObject)
        {
            //공격 사정거리밖이면 범위 내의 플레이어를 추적
            if (distance > attackRange)
            {
                direction = target.transform.position - transform.parent.position;
                direction.Normalize();
                transform.parent.position += direction * speed * Time.deltaTime;
                transform.parent.GetComponent<Animator>().SetFloat("Speed", direction.magnitude);

                if (direction.x < 0)
                    transform.parent.GetComponent<SpriteRenderer>().flipX = true;
                else
                    transform.parent.GetComponent<SpriteRenderer>().flipX = false;

            }
            //공격 사정거리에 들어왔을 경우
            else
            {
                transform.parent.GetComponent<Animator>().SetFloat("Speed", 0);
                GameObject.Find("Goblin").transform.Find("AttackRange").gameObject.SetActive(true);
                transform.parent.GetComponent<Animator>().SetTrigger("Attack");
                timer = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (target == collision.gameObject)
        {
            target = null;
            transform.parent.GetComponent<Animator>().SetFloat("Speed", 0);
        }
    }
}
