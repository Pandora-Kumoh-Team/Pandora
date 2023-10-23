using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAI : MonoBehaviour
{
    // Components
    public float speed = 1f; //�ӽ�
    private Vector3 direction;
    private GameObject target;
    public float attackRange = 2f; //�ӽ�

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

        //�÷��̾� �ĺ�
        if (collision.gameObject.tag == "Player" && target == null)
            target = collision.gameObject;

        //�÷��̾���� �Ÿ� ����
        if (target != null)
            distance = Vector2.Distance(transform.parent.position, target.transform.position);


        if (timer > waitingTime && target == collision.gameObject)
        {
            direction = target.transform.position - transform.parent.position;
            direction.Normalize();

            if (direction.x < 0)
            {
                transform.parent.GetComponent<SpriteRenderer>().flipX = true;
                GameObject.Find("Mushroom").transform.Find("AttackRange").transform.localPosition = new Vector3(-0.5f, -0.2f, 0);
            }
            else
            {
                transform.parent.GetComponent<SpriteRenderer>().flipX = false;
                GameObject.Find("Mushroom").transform.Find("AttackRange").transform.localPosition = new Vector3(0.5f, -0.2f, 0);
            }

            //���� �����Ÿ����̸� ���� ���� �÷��̾ ����
            if (distance > attackRange)
            {
                transform.parent.position += direction * speed * Time.deltaTime;
                transform.parent.GetComponent<Animator>().SetFloat("Speed", direction.magnitude);
            }
            //���� �����Ÿ��� ������ ���
            else
            {
                transform.parent.GetComponent<Animator>().SetTrigger("Attack");
                transform.parent.GetComponent<Animator>().SetFloat("Speed", 0);
                timer = 0;
                GameObject.Find("Mushroom").transform.Find("AttackRange").gameObject.SetActive(true);
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
