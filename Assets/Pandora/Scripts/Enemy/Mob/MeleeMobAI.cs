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
    [HideInInspector]
    public float speed;

    public float attackRange = 0.5f; //�ӽ�
    private Vector3 attackRangePos;
    private Vector2 capOffset;

    private void Start()
    {
        isConduct = false;
        timer = 0.0f;
        waitingTime = 2.0f;
        parentName = transform.parent.name;
        attackRangePos = GameObject.Find(parentName).transform.Find("AttackRange").transform.localPosition;
        capOffset = transform.parent.GetComponent<CapsuleCollider2D>().offset;
        speed = transform.parent.gameObject.transform.GetComponent<EnemyController>()._enemyStatus.Speed;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.5)
            transform.parent.transform.Find("AttackRange").gameObject.SetActive(false);

        //�ǰ� �� ���� �ð� �ʱ�ȭ
        if(transform.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            transform.parent.GetComponent<Animator>().SetFloat("Speed", 0);
            transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            timer = 0.8f;
        }

        if (!isConduct) //��� �ൿ�� �ϰ� ���� ������
        {
            //�����ϰ� �̵�
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
                // rigidbody�� ����
                transform.parent.GetComponent<Rigidbody2D>().velocity = ranVec * speed;
                transform.parent.GetComponent<Animator>().SetFloat("Speed", ranVec.magnitude);
                Flip(ranVec);
            }
        }
        randomMoveTime += Time.deltaTime;

        if (randomMoveTime >= 6)
            randomMoveTime = 0;

        speed = transform.parent.gameObject.transform.GetComponent<EnemyController>()._enemyStatus.Speed;
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
            isConduct = true;
            direction = target.transform.position - transform.parent.position;
            direction.Normalize();

            //���� ��ȯ
            Flip(direction);

            //���� �����Ÿ����̸� ���� ���� �÷��̾ ����
            if (distance > attackRange)
            {
                // transform.parent.position += direction * speed * Time.deltaTime;
                transform.parent.GetComponent<Rigidbody2D>().velocity = direction * speed;
                transform.parent.GetComponent<Animator>().SetFloat("Speed", direction.magnitude);
            }
            //���� �����Ÿ��� ������ ��� ����
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
        EnemyStatus enemyStatus = GameObject.Find(parentName).GetComponent<EnemyController>()._enemyStatus;

        if ( enemyStatus.Code >= 150 && enemyStatus.Code <= 199) //���ʺ��� �ִ� �����̵�
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
