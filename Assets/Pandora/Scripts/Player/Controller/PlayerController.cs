using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pandora.Scripts.Effect;
using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Skill;
using Pandora.Scripts.System;
using Pandora.Scripts.System.Event;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using NotImplementedException = System.NotImplementedException;
using Random = UnityEngine.Random;

namespace Pandora.Scripts.Player.Controller
{
    public class PlayerController : MonoBehaviour, IEventListener
    {
        // Components
        [HideInInspector]
        public Rigidbody2D rb;
        private Animator anim;
        private PlayerAI ai;
        private AudioSource audioSource;
    
        /// <summary>
        /// 플레이어 캐릭터 고유번호, UI와 연동
        /// </summary>
        [FormerlySerializedAs("playerCharacterId")] public int playerNumber = -1;
        
        // Stat
        [HideInInspector]
        public PlayerCurrentStat playerCurrentStat;
        public PlayerStat playerBasicStat;
    
        // Variables
        // 이동 관련
        [HideInInspector]
        public Vector2 lookDir;
        [HideInInspector]
        public Vector2 moveDir;
        [HideInInspector]
        public bool canControlMove;
        public AudioClip[] footstepAudioSource;
        public float footstepDistance = 0.35f;
        public float footstepVolume = 0.5f;
        private float footstepMovedDistance;

        // 공격 관련
        [HideInInspector]
        public Vector2 attackDir;
        private float attackCoolTime;
        private bool isAttackKeyPressed;
        public AudioClip[] attackSounds;
        public float attackSoundVolume = 0.5f;
    
        // 태그 관련
        [HideInInspector]
        public bool isControlByPlayer;
        [FormerlySerializedAs("onControlInit")] public bool isStartWithPlayerControl = true;
        private GameObject angelRing;
        
        [HideInInspector]
        public bool isDead;
        
        // 스킬 관련
        public GameObject[] activeSkills;
        [HideInInspector]
        public List<GameObject> passiveSkills;
        [HideInInspector]
        public float[] skillCoolTimes;
        [HideInInspector]
        public Transform activeSkillContainer;
        [HideInInspector]
        public Transform passiveSkillContainer;
        [HideInInspector]
        public bool isTrigger;

        private static readonly int CachedMoveDir = Animator.StringToHash("WalkDir");
        private static readonly int Attack1 = Animator.StringToHash("Attack");
        private static readonly int CachedAttackDir = Animator.StringToHash("AttackDir");

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            ai = GetComponent<PlayerAI>();
            playerCurrentStat = new PlayerCurrentStat();
            canControlMove = true;
            skillCoolTimes = new float[3];
            activeSkillContainer = transform.Find("Skills").Find("ActiveSkills");
            passiveSkillContainer = transform.Find("Skills").Find("PassiveSkills");
            angelRing = transform.Find("Angel").gameObject;
            audioSource = GetComponent<AudioSource>();
            isTrigger = false;
            
            StartCoroutine(LateAwake());
        }

        private IEnumerator LateAwake()
        {
            yield return null;
            EventManager.Instance.AddListener(PandoraEventType.MapGenerateComplete, this);
        }

        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener(PandoraEventType.MapGenerateComplete, this);
        }

        public virtual void Start()
        {
            isControlByPlayer = isStartWithPlayerControl;
            ai.enabled = !isStartWithPlayerControl;
            anim.SetInteger(CachedMoveDir, -1);
            playerCurrentStat.playerStat = playerNumber == 0
                ? PermanentStatController.Instance.p0PermanentStats
                : PermanentStatController.Instance.p1PermanentStats;
            playerCurrentStat.Init();

            foreach (var activeSkill in activeSkills)
            {
                activeSkill.GetComponent<Skill.Skill>().ownerPlayer = gameObject;
            }
            
            if(playerNumber == -1)
            {
                Debug.LogError("Player Character Id is not set");
            }
            // 한 프레임 뒤에 실행
            StartCoroutine(Init());
        }
        
        private IEnumerator Init()
        {
            yield return null;
            // TODO : Start() 실행 순서에 따른 버그 해결을 위해 StartTaskQueue를 만들어야 할 것 같음
            // Start() 실행 문제 때문에 생기는 버그는 아래와 같음
            // 1. 플레이어가 생성되고 변경된 스텟이 HPUI에 반영되지 않음
            CallHealthChangedEvent();
        }

        private void Update()
        {
            // 이동
            if(canControlMove && moveDir.magnitude > 0.5f)
            {
                rb.velocity = moveDir * playerCurrentStat.Speed;
                SetMoveAnimation(moveDir);
            }
            else if(canControlMove && moveDir.magnitude <= 0.5f && isControlByPlayer)
            {
                rb.velocity = Vector2.zero;
                anim.SetInteger(CachedMoveDir, -1);
            }
            else if(canControlMove && moveDir.magnitude <= 0.5f && !isControlByPlayer)
            {
                rb.velocity = moveDir * playerCurrentStat.Speed;
                SetMoveAnimation(moveDir);
            }
            else
            {
                anim.SetInteger(CachedMoveDir, -1);
            }
            // 이동 발자국 소리
            if (canControlMove && moveDir.magnitude > 0.5f)
            {
                footstepMovedDistance += moveDir.magnitude * Time.deltaTime;
                if (footstepMovedDistance >= footstepDistance)
                {
                    footstepMovedDistance = 0f;
                    audioSource.PlayOneShot(footstepAudioSource[Random.Range(0, footstepAudioSource.Length)],
                        footstepVolume);
                }
            }
            
            // 스킬 쿨다운
            for (int i = 0; i < skillCoolTimes.Length; i++)
            {
                if (skillCoolTimes[i] >= 0)
                {
                    skillCoolTimes[i] -= Time.deltaTime;
                }
            }

            // 공격
            if(!CanAttack())
            {
                attackCoolTime -= Time.deltaTime;
            }
            if(isControlByPlayer && isAttackKeyPressed && CanAttack())
            {
                Attack();
            }

            if (isTrigger)
                transform.GetComponent<CircleCollider2D>().isTrigger = true;
            else
                transform.GetComponent<CircleCollider2D>().isTrigger = false;
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
            if (rand < playerCurrentStat.DodgeChance)
            {
                Dodge();
                return;
            }
            
            // 피격 피해 적용
            playerCurrentStat.NowHealth -= damage * (1f - playerCurrentStat.DefencePower);
            CallHealthChangedEvent();
            
            // AI 공격 대상 변경
            if (!isControlByPlayer)
            {
                ai._target = attacker;
                ai.ChangeState(new AttackTargetState().Init(attacker));
            }
            
            // 이펙트 출력
            var coll = GetComponent<CircleCollider2D>();
            var position = transform.position + new Vector3(0, coll.radius * 2, 0);
            var damageEffect = Instantiate(GameManager.Instance.damageEffect, position, Quaternion.identity, transform);
            damageEffect.GetComponent<FadeTextEffect>()
                .Init(damage.ToString(), Color.red, 1f, 0.5f, 0.05f, Vector3.up);
            var bloodEffect = Instantiate(GameManager.Instance.bloodParticle, position, Quaternion.identity);
            // 공격자의 반대 방향으로 피격 이펙트 회전
            var z = transform.position.x > attacker.transform.position.x ? -45 : 45;
            bloodEffect.transform.rotation = Quaternion.Euler(0, 0, z);
            Destroy(bloodEffect, 1f);
            
            if (playerCurrentStat.NowHealth <= 0)
            {
                Die();
            }
            
            // 버프 적용
            if (buffs == null) return;
            playerCurrentStat.AddBuffs(buffs);
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
            var param = new PlayerHealthChangedParam(playerCurrentStat.NowHealth, playerCurrentStat.MaxHealth, playerNumber);
            EventManager.Instance.TriggerEvent(PandoraEventType.PlayerHealthChanged, param);
        }

        public void Die()
        {
            isDead = true;
            moveDir = Vector2.zero;
            attackDir = Vector2.zero;
            GetComponent<SpriteRenderer>().color = new Color(0.1f,0.1f,0.1f);
            var go = gameObject;
            go.layer = LayerMask.NameToLayer("DeadPlayer");
            go.tag = "Untagged";
            if(isControlByPlayer)
            {
                isControlByPlayer = false;
                ai.enabled = !isControlByPlayer;
                var otherController =
                    PlayerManager.Instance.GetOtherPlayer(gameObject).GetComponent<PlayerController>();
                if(otherController.isDead)
                {
                    GameManager.Instance.GameOver();
                    return;
                }
                otherController.isControlByPlayer = true;
                otherController.ai.enabled = !otherController.isControlByPlayer;
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
            if (!isControlByPlayer) return;
            // press 여부 저장
            var keyDir = value.Get<Vector2>();
            if(keyDir.magnitude > 0.5f)
            {
                attackDir = keyDir;
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
            attackCoolTime = 1 / playerCurrentStat.AttackSpeed;
            
            SetAttackAnimation();

            var hitParams = new HitParams();
            hitParams.attacker = gameObject;
            
            // 크리티컬 여부 판단
            var rand = Random.Range(0, 100);
            hitParams.damage = playerCurrentStat.BaseDamage * playerCurrentStat.AttackPower;
            if (rand < playerCurrentStat.CriticalChance)
            {
                hitParams.damage *= playerCurrentStat.CriticalDamageTimes;
                hitParams.isCritical = true;
            }
            
            // 사운드 출력
            var randSound = Random.Range(0, attackSounds.Length);
            audioSource.PlayOneShot(attackSounds[randSound], attackSoundVolume);
        
            StartCoroutine(AttackCoroutine(hitParams));
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
        protected virtual IEnumerator AttackCoroutine(HitParams hitParams)
        {
            yield return null;
        }
    
        /// <summary>
        /// 공격 타입별 사거리 변화 처리
        /// 첫 부분에 base.AttackRangeChanged()를 호출해야 함
        /// </summary>
        /// <param name="value">변경 후 사거리 (ex) 1.5시 기본값보다 사거리 50프로 증가)</param>
        public virtual void AttackRangeChanged(float value)
        {
            playerCurrentStat.AttackRange = value;
        }

        #endregion

        #region 이동 관련

        public void OnMove(InputValue value)
        {
            if (!isControlByPlayer || !canControlMove) return;
            moveDir = value.Get<Vector2>();
            if(moveDir.magnitude > 0.5f)
                lookDir = moveDir;
        }

        private void SetMoveAnimation(Vector2 moveDirection)
        {
            if (moveDirection == Vector2.zero)
            {
                anim.SetInteger(CachedMoveDir, -1);
                return;
            }
            
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
        
        #region 스킬 관련
        
        public void AddPassiveSkill(GameObject skill)
        {
            SkillManager.Instance.RemoveSkillAtList(playerNumber, Skill.Skill.SkillType.Passive, skill);
            var skillObject = Instantiate(skill, passiveSkillContainer, true);
            var skillComponent = skillObject.GetComponent<Skill.Skill>();
            skillComponent.ownerPlayer = gameObject;
            passiveSkills.Add(skill);
            ((PassiveSkill)skillComponent).OnGetSkill();
        }
        
        public void RemovePassiveSkill(GameObject skill)
        {
            var skillComponent = skill.GetComponent<Skill.Skill>();
            ((PassiveSkill)skillComponent).OnLoseSkill();
        }

        public void SetActiveSkill(GameObject skill, int skillIndex)
        {
            SkillManager.Instance.RemoveSkillAtList(playerNumber, Skill.Skill.SkillType.Active, skill);
            var eventParam =
                new PlayerSkillChangedParam(skill.GetComponent<Skill.Skill>(), playerNumber, skillIndex);
            EventManager.Instance.TriggerEvent(PandoraEventType.PlayerSkillChanged, eventParam);
            Destroy(activeSkills[skillIndex]);
            var skillObject = Instantiate(skill, activeSkillContainer, true);
            activeSkills[skillIndex] = skillObject;
            var skillComponent = skillObject.GetComponent<Skill.Skill>();
            skillComponent.ownerPlayer = gameObject;
        }

        private void OnSkill(InputValue value, int skillIndex)
        {
            if (!isControlByPlayer) return;
            if (activeSkills == null) return;
            if (skillCoolTimes[skillIndex] < 0)
            {
                var skillComponent = activeSkills[skillIndex].GetComponent<Skill.Skill>();
                skillCoolTimes[skillIndex] = skillComponent.coolTime;
                ((ActiveSkill)skillComponent).Use();
                // anim.SetTrigger(activeSkills[skillIndex].name - "(Clone)");
            }
        }
        public void OnSkill1(InputValue value)
        {
            OnSkill(value, 0);
        }
        public void OnSkill2(InputValue value)
        {
            OnSkill(value, 1);
        }
        public void OnSkill3(InputValue value)
        {
            OnSkill(value, 2);
        }

        public GameObject[] GetActiveSkills()
        {
            return activeSkills;
        }
        
        public GameObject[] GetPassiveSkills()
        {
            return passiveSkills.ToArray();
        }

        #endregion

        public void OnTag(InputValue value)
        {
            var otherController = PlayerManager.Instance.GetOtherPlayer(gameObject).GetComponent<PlayerController>();
            if (otherController.isDead || isDead) return;
            if(!isControlByPlayer)
            {
                attackDir = Vector2.zero;
                isAttackKeyPressed = false;
            }
            isControlByPlayer = !isControlByPlayer;
            ai.enabled = !isControlByPlayer;
            angelRing.SetActive(isControlByPlayer);
        }
        
        private IEnumerator RemoveBuffAfterDuration(Buff buff)
        {
            yield return new WaitForSeconds(buff.Duration);
            playerCurrentStat.RemoveBuff(buff);
        }

        public void OnEvent(PandoraEventType pandoraEventType, Component sender, object param = null)
        {
            if (pandoraEventType == PandoraEventType.MapGenerateComplete)
            {
                attackDir = Vector2.zero;
                moveDir = Vector2.zero;
                var healthParam = new PlayerHealthChangedParam(playerCurrentStat.NowHealth, playerCurrentStat.MaxHealth,
                    playerNumber);
                EventManager.Instance.TriggerEvent(PandoraEventType.PlayerHealthChanged, healthParam);
            }
        }
    }
}
