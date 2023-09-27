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
    private static readonly int WalkDir = Animator.StringToHash("WalkDir");

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
    
    // 공격 타입별로 하위 클래스에서 정의
    protected virtual IEnumerator AttackCoroutine()
    {
        yield return null;
    }
    
    #region InputSystemEvents

    public void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
        if(moveDir.magnitude < 0.1f)
        {
            anim.SetInteger(WalkDir, -1);
        }
        else
        {
            SetMoveAnimation(moveDir);
        }
    }

    private void SetMoveAnimation(Vector2 moveDir)
    {
        // Vector2.right 와 moveDir 사이의 각도 계산
        float angle = Vector2.SignedAngle(Vector2.right, moveDir);
        // 각도에 따라 8방향으로 애니메이션 설정
        if (angle >= -22.5f && angle < 22.5f)
        {
            anim.SetInteger(WalkDir, 0);
        }
        else if (angle >= 22.5f && angle < 67.5f)
        {
            anim.SetInteger(WalkDir, 1);
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            anim.SetInteger(WalkDir, 2);
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            anim.SetInteger(WalkDir, 3);
        }
        else if (angle >= 157.5f || angle < -157.5f)
        {
            anim.SetInteger(WalkDir, 4);
        }
        else if (angle >= -157.5f && angle < -112.5f)
        {
            anim.SetInteger(WalkDir, 5);
        }
        else if (angle >= -112.5f && angle < -67.5f)
        {
            anim.SetInteger(WalkDir, 6);
        }
        else if (angle >= -67.5f && angle < -22.5f)
        {
            anim.SetInteger(WalkDir, 7);
        }
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
