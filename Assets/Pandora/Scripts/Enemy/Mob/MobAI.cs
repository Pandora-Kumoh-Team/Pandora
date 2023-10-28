using Pandora.Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAI : MonoBehaviour
{
    // Components
    private Vector3 direction;
    private GameObject target;
    private string parentName;

    //Status
    public float speed = 1.0f; //�ӽ�

    private float timer;
    private int waitingTime;

    public float attackRange = 1f; //�ӽ�
    Vector3 attackRangePos;

    private void Start()
    {
        timer = 0.0f;
        waitingTime = 2;
        parentName = transform.parent.name;
        attackRangePos = GameObject.Find(parentName).transform.Find("AttackRange").transform.localPosition;
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

        //�÷��̾� �ĺ�
        if (collision.gameObject.tag == "Player" && target == null)
            target = collision.gameObject;

        //�÷��̾���� �Ÿ� ����
        if (target != null)
            distance = Vector2.Distance(transform.parent.position, target.transform.position);

        //���ð��� �ƴ� ���
        if (timer > waitingTime && target == collision.gameObject)
        {
            direction = target.transform.position - transform.parent.position;
            direction.Normalize();

            //���� ��ȯ
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

            //���� �����Ÿ����̸� ���� ���� �÷��̾ ����
            if (distance > attackRange)
            {
                transform.parent.position += direction * speed * Time.deltaTime;
                transform.parent.GetComponent<Animator>().SetFloat("Speed", direction.magnitude);
            }
            //���� �����Ÿ��� ������ ��� ����
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
        }
    }
}
