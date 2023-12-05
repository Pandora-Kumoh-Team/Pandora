using UnityEngine;
namespace Pandora.Scripts.Enemy
{
    public class BossAI : MonoBehaviour
    {
        // Components
        private Vector3 direction;
        private GameObject target;
        private string parentName;
        private Vector3 myPos;
        private Animator parentAnimation;

        //Status
        private float speed;
        private float timer;
        private int waitingTime;

        public float attackRange = 2f;
        Vector3 attackRangePos;

        private void Start()
        {
            timer = 0.0f;
            waitingTime = 2;
            parentName = transform.parent.name;
            attackRangePos = GameObject.Find(parentName).transform.Find("AttackRange").transform.localPosition;
            myPos = transform.position;
            parentAnimation = transform.parent.GetComponent<Animator>();
            speed = transform.parent.GetComponent<FirstBossController>()._enemyStatus.Speed; //보스에 설정된 스피드로 설정
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
            if (collision.gameObject.tag.Equals("Player"))
            {
                target = collision.gameObject;
            }
            //플레이어와의 거리 측정
            if (target != null)
            {
                //자신의 부모[보스 obj와 플레이어와의 거리
                distance = Vector2.Distance(transform.parent.position, target.transform.position);
            }
            if (timer > waitingTime && target == collision.gameObject)
            {
                setDirection();
                selectBehavior(distance);
            }

        }
        private void setDirection()
        {
            direction = target.transform.position - transform.parent.position; //바라보는 방향
            direction.Normalize(); //정규화

            //방향 전환
            if (direction.x < 0) //왼쪽을 바라보는 경우
            {
                transform.parent.GetComponent<Animator>().SetFloat("Direction", -1);
                GameObject.Find(parentName).transform.Find("AttackRange").transform.localPosition = new Vector3(-attackRangePos.x, attackRangePos.y, 0);
            }
            else
            {
                transform.parent.GetComponent<Animator>().SetFloat("Direction", 1);
                GameObject.Find(parentName).transform.Find("AttackRange").transform.localPosition = new Vector3(attackRangePos.x, attackRangePos.y, 0);
            }
        }
        public void selectBehavior(float distance)
        {
            if (distance > attackRange) //거리가 공격 범위 밖에 있을 때
            {
                transform.parent.position += direction * speed * Time.deltaTime; //플레이어 방향으로 이동
                parentAnimation.SetBool("isFollow", true); //애니메이션을 플레이어 따라가는 걸로 조정
            }
            else //공격 범위 내에 있다면
            {
                parentAnimation.SetTrigger("Attack");
                parentAnimation.SetBool("isFollow", false); //Idle 상태
                timer = 0;
                GameObject.Find(parentName).transform.Find("AttackRange").gameObject.SetActive(true);
            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            if (target == collision.gameObject)
            {
                target = null;
            }
        }
    }
}