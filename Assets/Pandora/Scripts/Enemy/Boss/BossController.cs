using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    Animator animator;
    public Transform player;
    public Vector2 home;
    public float moveSpeed = 1.0f;

    public float attackCooltime = 2;
    public float attackDelay;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        home = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackDelay >= 0)
        {
            attackDelay -= Time.deltaTime;
        }
    }

    //TODO ĳ���� �� Animator�� ��� ���߿� �߰��ؼ� �����ϱ�.
    public void DiretionEnemy(float target, float baseObj)
    {
        if(target < baseObj)
        {
            animator.SetFloat("Direction",-1);
        }
        else
        {
            animator.SetFloat("Direction", 1);
        }
    }
}
