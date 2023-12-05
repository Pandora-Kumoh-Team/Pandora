using System.Collections;
using System.Collections.Generic;
using Pandora.Scripts.Player.Controller;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class NormalMobAI : MonoBehaviour
{
    //몹 코드 별 패턴 분류 작업 필요

    private float patternMoveTime;
    private Vector3 patternVec1;
    private Vector3 patternVec2;
    private Vector3 patternVec3;
    private Vector3 patternVec4;
    private float patternSpeed = 1f;
    private Vector3 chaseVec1;
    private Vector3 chaseVec2;
    private float chaseMoveTime;
    private bool isConduct= false;
    private float chaseRange = 0.5f;

    // Components
    private Vector3 direction;
    private GameObject target;

    //Status
    public float speed = 1.0f; //임시
    public float attackDamage = 1.0f; // 임시
    private float attackCool = 0.3f;

    private void Start()
    {
        isConduct = false;

        patternVec1 = new Vector3(-1, 1, 0);
        patternVec2 = new Vector3(1, -1, 0);
        patternVec3 = new Vector3(1, 1, 0);
        patternVec4 = new Vector4(-1, -1, 0);

        chaseVec1 = new Vector3(0, 1, 0);
        chaseVec2 = new Vector3(0, -1, 0);

        patternMoveTime = 0;
        chaseMoveTime = 0;
}

    private void Update()
    {
        Vector3 patternVec = new Vector3();
        if (!isConduct)
        {
            if (patternMoveTime >= 0 && patternMoveTime < 0.25)
                patternVec = patternVec1;
            else if (patternMoveTime >= 0.25 && patternMoveTime < 0.5)
                patternVec = patternVec2;
            else if (patternMoveTime >= 0.5 && patternMoveTime < 0.75)
                patternVec = patternVec3;
            else if (patternMoveTime >= 0.75 && patternMoveTime < 1)
                patternVec = patternVec4;
            else
                patternMoveTime = 0;
            transform.parent.GetComponent<Rigidbody2D>().velocity = patternVec.normalized * speed;
        }

        patternMoveTime += Time.deltaTime;
        chaseMoveTime += Time.deltaTime;
        
        if(attackCool > 0)
            attackCool -= Time.deltaTime;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //플레이어 식별
        if (!collision.gameObject.CompareTag("Player")) return;
        
        float distance = 0.0f;
        target = collision.gameObject;

        distance = Vector2.Distance(transform.parent.position, target.transform.position);
        isConduct = true;
        // 임시 공격
        if(distance < 1f && attackCool <= 0 && collision.gameObject.CompareTag("Player"))
        {
            target.GetComponent<PlayerController>().Hurt(attackDamage, null, gameObject);
            attackCool = 0.3f;
        }

        direction = target.transform.position - transform.parent.position;
        direction.Normalize();
        transform.parent.GetComponent<Rigidbody2D>().velocity = direction * speed;
        if (direction.x < 0)
            transform.parent.GetComponent<SpriteRenderer>().flipX = true;
        else
            transform.parent.GetComponent<SpriteRenderer>().flipX = false;
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (target == collision.gameObject)
        {
            isConduct = false;
            target = null;
        }
    }
}
