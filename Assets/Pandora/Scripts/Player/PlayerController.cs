using System;
using System.Collections;
using System.Collections.Generic;
using Pandora.Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Components
    private Rigidbody2D rb;
    private Animator anim;
    
    // Tmp Status
    public float tmpSpeed = 5f;
    private float tmpAttackCoolTime;
    
    // Variables
    // 이동 관련
    private Vector2 moveDir;

    // 공격 관련
    protected Vector2 attackDir;
    private bool isOnAttack;
    
    // 태그 관련
    private bool isOnControl;
    public bool onControlInit = true;
    
    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        isOnControl = onControlInit;
    }

    void Update()
    {
        // 조작 중 상태
        if(isOnControl)
        {
            // 이동
            rb.velocity = moveDir * tmpSpeed;
            
            // 공격
            if(tmpAttackCoolTime > 0)
            {
                tmpAttackCoolTime -= Time.deltaTime;
            }
            else
            {
                if(isOnAttack)
                {
                    Attack();
                    tmpAttackCoolTime = 0.5f;
                }
            }
        }
        
        // Ai 이동 상태
        else
        {
            // stop
            rb.velocity = Vector2.zero;
            // TODO : move by AI
        }
        
    }

    private void Attack()
    {
        //anim.SetTrigger("Attack");
        StartCoroutine(AttackCoroutine());
    }
    
    protected virtual IEnumerator AttackCoroutine()
    {
        // 공격 타입별로 하위 클래스에서 정의
        yield return null;
    }
    
    #region InputSystemEvents

    public void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }
    
    public void OnTag(InputValue value)
    {
        isOnControl = !isOnControl;
    }
    
    public void OnAttack(InputValue value)
    {
        // press 여부 저장
        attackDir = value.Get<Vector2>();
        if(attackDir.magnitude > 0.5f)
        {
            isOnAttack = true;
        }
        else
        {
            isOnAttack = false;
        }
    }

    #endregion
}
