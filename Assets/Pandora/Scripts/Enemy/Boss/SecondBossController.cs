using Pandora.Scripts.Effect;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.System;
using Pandora.Scripts.System.Event;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Pandora.Scripts.Enemy
{
    public class SecondBossController : MonoBehaviour, IHitAble
    {
        //Component
        private Rigidbody2D rb;
        private Animator anim;
        private PolygonCollider2D polygonCollider;
        private GameObject target;
        private float delay = 300f; //광폭화
        private float AttackPatternDelay = 15f; //다른 공격 패턴 딜레이
        private bool isCharging = false;
        public float chargeSpeed = 3f; // 돌진 속도
        public float chargeDuration = 1f; // 돌진 지속 시간
        public float cooldownTime = 5f; // 돌진 쿨타임
        private bool isCooldown = false;

        //Status
        public EnemyStatus _enemyStatus;

        private void Awake()
        {
            _enemyStatus = new EnemyStatus("2StageBoss");
        }
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            polygonCollider = GetComponent<PolygonCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            target = GameObject.FindGameObjectWithTag("Player"); //플레이어 찾기
            rb.velocity = Vector3.zero; //밀림 방지
        }
        public void Hit(HitParams hitParams)
        {
            var damage = hitParams.damage;
            anim.SetTrigger("Hit");
            transform.Find("BossHP").gameObject.SetActive(true);
            //damage effect
            var reduceDamage = damage - (damage * _enemyStatus.DefencePower / 100);
            var relativePos = new Vector3(1.5f, 1f, 0);
            hitParams.damage = reduceDamage;
            DamageTextEffectManager.Instance.SpawnDamageTextEffect(relativePos, gameObject, hitParams);

            _enemyStatus.NowHealth -= reduceDamage;
            CallHealthChangeEvetnt();
            if (_enemyStatus.NowHealth <= _enemyStatus.MaxHealth * 0.6 && isCharging == false)
            {
                StartCoroutine(StartChargingWithCooldown());
            }
            if (_enemyStatus.NowHealth <= 0)
            {
                StartCoroutine(Death());
            }
        }
        private void OnDisable()
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<FadeTextEffect>() != null)
                    Destroy(child.gameObject);
            }
        }
        public void Attack()
        {
            target.GetComponent<PlayerController>().Hurt(_enemyStatus.BaseDamage, null, gameObject);
        }
        public void AnotherAttack() //기본 공격에 n만큼만 강해지게 때림
        {
            target.GetComponent<PlayerController>().Hurt(_enemyStatus.BaseDamage + 3f, null, gameObject);
        }
        private void CallHealthChangeEvetnt()
        {
            var param = new BossHealthChangedParam(_enemyStatus.NowHealth, _enemyStatus.MaxHealth);
            EventManager.Instance.TriggerEvent(PandoraEventType.BossHealthChanged, param);
        }
        IEnumerator Death()
        {
            anim.SetTrigger("Death");
            yield return new WaitForSeconds(1.5f);
            Destroy(gameObject);
        }
        IEnumerator WIDE_AREA()//광폭화
        {
            yield return new WaitForSeconds(delay);
            _enemyStatus.AttackPower += 10f;
            _enemyStatus.Speed += 2f;
        }
        IEnumerator StartChargingWithCooldown()
        {
            if (isCooldown)
            {
                yield break; // 이미 실행 중이면 빠져나가기
            }
            isCooldown = true;
            while (true)
            {
                // 일정 쿨타임 기다리기
                yield return new WaitForSeconds(cooldownTime);

                // 돌진 시작
                StartCharge();

                // 돌진이 끝날 때까지 기다리기
                yield return new WaitForSeconds(chargeDuration);
            }

        }

        void StartCharge()
        {
            isCharging = true;
            StartCoroutine(Charge());
        }

        IEnumerator Charge()
        {
            float startTime = Time.time;

            while (Time.time - startTime < chargeDuration)
            {
                anim.SetBool("isFollow", false);
                anim.SetTrigger("Rush");
                // 돌진 방향으로 이동
                transform.Translate((target.transform.position - gameObject.transform.position).normalized * chargeSpeed * Time.deltaTime);                
                yield return null; // 다음 프레임까지 대기
            }

            // 돌진 종료
            isCharging = false; 
            anim.SetBool("isFollow", true);
        }
    }
}
