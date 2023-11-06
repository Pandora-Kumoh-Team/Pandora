using System.Collections;
using System.Collections.Generic;
using Pandora.Scripts.Effect;
using Pandora.Scripts.System;
using Pandora.Scripts.System.Event;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Pandora.Scripts.Player.Controller
{
    public class PlayerController : MonoBehaviour
    {
        // Components
        private Rigidbody2D rb;
        private Animator anim;
        private PlayerAI ai;
    
        /// <summary>
        /// 플레이어 캐릭터 고유번호, UI와 연동
        /// </summary>
        public int playerCharacterId = -1;
        
        // Stat
        public PlayerStat _playerStat;
    
        // Variables
        // 이동 관련
        public Vector2 moveDir;

        // 공격 관련
        public Vector2 attackDir;
        private float attackCoolTime;
        private bool isAttackKeyPressed;
    
        // 태그 관련
        public bool isOnControl;
        public bool isDead;
        public bool onControlInit = true;
        
        private static readonly int CachedMoveDir = Animator.StringToHash("WalkDir");
        private static readonly int Attack1 = Animator.StringToHash("Attack");
        private static readonly int CachedAttackDir = Animator.StringToHash("AttackDir");

        public virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            ai = GetComponent<PlayerAI>();
            
            isOnControl = onControlInit;
            ai.enabled = !onControlInit;
            anim.SetInteger(CachedMoveDir, -1);
            _playerStat = new PlayerStat();
            
            if(playerCharacterId == -1)
            {
                Debug.LogError("Player Character Id is not set");
            }
            CallHealthChangedEvent();
        }

        void Update()
        {
            // 이동
            rb.velocity = moveDir * _playerStat.Speed;
            if(moveDir.magnitude < 0.1f)
            {
                anim.SetInteger(CachedMoveDir, -1);
            }
            else
            {
                SetMoveAnimation(moveDir);
            }
            
            if(!CanAttack())
            {
                attackCoolTime -= Time.deltaTime;
            }
            if(isOnControl && isAttackKeyPressed && CanAttack())
            {
                Attack();
            }
        }
    

        #region 피격 관련

        /// <summary>
        /// 플레이어가 피격을 받았을 때 호출되는 함수
        /// </summary>
        /// <param name="damage">피해랑</param>
        /// <param name="buffs">적용될 디버프 없으면 null</param>
        /// <param name="attacker">공격한 오브젝트</param>
        public void Hurt(float damage, List<Buff> buffs, GameObject attacker)
        {
            // 회피 판정
            var rand = Random.Range(0, 100);
            if (rand < _playerStat.DodgeChance)
            {
                Dodge();
                return;
            }
            
            // 피격 피해 적용
            _playerStat.NowHealth -= damage * (1f - _playerStat.DefencePower);
            CallHealthChangedEvent();
            
            // AI 공격 대상 변경
            if (!isOnControl)
            {
                ai._target = attacker;
                ai._currentState = PlayerAI.AIState.MoveToTarget;
            }
            
            // 이펙트 출력
            var coll = GetComponent<CircleCollider2D>();
            var position = transform.position + new Vector3(0, coll.radius, 0);
            var damageEffect = Instantiate(GameManager.Instance.damageEffect, position, Quaternion.identity, transform);
            damageEffect.GetComponent<FadeTextEffect>()
                .Init(damage.ToString(), Color.red, 1f, 0.5f, 0.05f, Vector3.up);

            
            if (_playerStat.NowHealth <= 0)
            {
                Die();
            }
            
            // 버프 적용
            if (buffs == null) return;
            _playerStat.AddBuffs(buffs);
            foreach (var buff in buffs)
            {
                StartCoroutine(RemoveBuffAfterDuration(buff));
            }
        }

        private void Dodge()
        {
            // TODO : 회피
        }

        public void CallHealthChangedEvent()
        {
            var param = new PlayerHealthChangedParam(_playerStat.NowHealth, _playerStat.MaxHealth, playerCharacterId);
            EventManager.Instance.TriggerEvent(PandoraEventType.PlayerHealthChanged, param);
        }

        private IEnumerator RemoveBuffAfterDuration(Buff buff)
        {
            yield return new WaitForSeconds(buff.Duration);
            _playerStat.RemoveBuff(buff);
        }

        public void Die()
        {
            isDead = true;
            GetComponent<SpriteRenderer>().color = new Color(0.1f,0.1f,0.1f);
            var go = gameObject;
            go.layer = LayerMask.NameToLayer("DeadPlayer");
            go.tag = "Untagged";
            if(isOnControl)
            {
                isOnControl = false;
                ai.enabled = !isOnControl;
                var otherController =
                    PlayerManager.Instance.GetOtherPlayer(gameObject).GetComponent<PlayerController>();
                if(otherController.isDead)
                {
                    GameManager.Instance.GameOver();
                    return;
                }
                otherController.isOnControl = true;
                otherController.ai.enabled = !otherController.isOnControl;
            }
        }
        
        public void Rebirth()
        {
            isDead = false;
            GetComponent<SpriteRenderer>().color = Color.white;
            var go = gameObject;
            go.layer = LayerMask.NameToLayer("Player");
            go.tag = "Player";
        }

        #endregion

        #region 공격 관련

        // Input System에서 호출
        public void OnAttack(InputValue value)
        {
            if (!isOnControl) return;
            // press 여부 저장
            attackDir = value.Get<Vector2>();
            if(attackDir.magnitude > 0.5f)
            {
                isAttackKeyPressed = true;
            }
            else
            {
                isAttackKeyPressed = false;
            }
        }

        public bool CanAttack()
        {
            return attackCoolTime < 0;
        }

        public void Attack()
        {
            attackCoolTime = 1 / _playerStat.AttackSpeed;
            
            SetAttackAnimation();
        
            // 크리티컬 여부 판단
            var rand = Random.Range(0, 100);
            var damage = _playerStat.BaseDamage * _playerStat.AttackPower;
            if (rand < _playerStat.CriticalChance)
            {
                damage *= _playerStat.CriticalDamageTimes;
            }
        
            StartCoroutine(AttackCoroutine(damage, _playerStat.GetAttackBuffs()));
        }
        private void SetAttackAnimation()
        {
            anim.SetTrigger(Attack1);
            float angle = Vector2.SignedAngle(Vector2.right, attackDir);
            // 각도에 따라 4방향으로 공격 애니메이션을 재생한다.
            if (angle >= -45 && angle < 45)
            {
                anim.SetInteger(CachedAttackDir, 0);
            }
            else if (angle >= 45 && angle < 135)
            {
                anim.SetInteger(CachedAttackDir, 1);
            }
            else if (angle >= 135 || angle < -135)
            {
                anim.SetInteger(CachedAttackDir, 2);
            }
            else if (angle >= -135 && angle < -45)
            {
                anim.SetInteger(CachedAttackDir, 3);
            }
        }
    
        /// <summary>
        ///  공격 타입별로 하위 클래스에서 정의
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator AttackCoroutine(float damage, List<Buff> buffs)
        {
            yield return null;
        }
    
        /// <summary>
        /// 공격 타입별 사거리 변화 처리
        /// 첫 부분에 base.AttackRangeChanged()를 호출해야 함
        /// </summary>
        /// <param name="value">변경 후 사거리</param>
        public virtual void AttackRangeChanged(float value)
        {
            _playerStat.AttackRange = value;
        }

        #endregion

        #region 이동 관련

        public void OnMove(InputValue value)
        {
            if (!isOnControl) return;
            moveDir = value.Get<Vector2>();
        }

        private void SetMoveAnimation(Vector2 moveDirection)
        {
            // Vector2.right 와 moveDir 사이의 각도 계산
            float angle = Vector2.SignedAngle(Vector2.right, moveDirection);
            // 각도에 따라 8방향으로 애니메이션 설정
            if (angle >= -22.5f && angle < 22.5f)
            {
                anim.SetInteger(CachedMoveDir, 0);
            }
            else if (angle >= 22.5f && angle < 67.5f)
            {
                anim.SetInteger(CachedMoveDir, 1);
            }
            else if (angle >= 67.5f && angle < 112.5f)
            {
                anim.SetInteger(CachedMoveDir, 2);
            }
            else if (angle >= 112.5f && angle < 157.5f)
            {
                anim.SetInteger(CachedMoveDir, 3);
            }
            else if (angle >= 157.5f || angle < -157.5f)
            {
                anim.SetInteger(CachedMoveDir, 4);
            }
            else if (angle >= -157.5f && angle < -112.5f)
            {
                anim.SetInteger(CachedMoveDir, 5);
            }
            else if (angle >= -112.5f && angle < -67.5f)
            {
                anim.SetInteger(CachedMoveDir, 6);
            }
            else if (angle >= -67.5f && angle < -22.5f)
            {
                anim.SetInteger(CachedMoveDir, 7);
            }
        }
        

        #endregion
        
        public void OnTag(InputValue value)
        {
            var otherController = PlayerManager.Instance.GetOtherPlayer(gameObject).GetComponent<PlayerController>();
            if (otherController.isDead || isDead) return;
            isOnControl = !isOnControl;
            ai.enabled = !isOnControl;
        }
    }
}
