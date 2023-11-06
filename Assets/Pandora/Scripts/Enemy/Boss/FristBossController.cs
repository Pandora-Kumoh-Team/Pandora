using Pandora.Scripts;
using Pandora.Scripts.Effect;
using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.System;
using Pandora.Scripts.System.Event;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

namespace Pandora.Scripts.Enemy
{
    public class FirstBossController : MonoBehaviour, IHitAble
    {
        //Component
        private Rigidbody2D rb;
        private Animator anim;
        private PolygonCollider2D polygonCollider;
        private GameObject target;
        private bool isKnife;

        //Animator Hashes

        //Status
        public EnemyStatus _enemyStatus;
        private void Awake()
        {
            _enemyStatus = new EnemyStatus(this.gameObject.name);
        }

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            polygonCollider = GetComponent<PolygonCollider2D>();
            isKnife = false;
            StartCoroutine(GetBuffMode());
        }

        // Update is called once per frame
        void Update()
        {
            target = GameObject.FindGameObjectWithTag("Player"); //Player 계속 추적
            rb.velocity = Vector3.zero; //밀림 방지
        }
        public void Hit(float damage, List<Buff> buff)
        {
            anim.SetTrigger("Hit");
            transform.Find("BossHP").gameObject.SetActive(true);
            //damage effect
            var effectPosition = transform.position + new Vector3(1.5f, 1f, 0);
            var damageEffect = Instantiate(GameManager.Instance.damageEffect, effectPosition, Quaternion.identity, transform);
            damageEffect.GetComponent<FadeTextEffect>()
                .Init(damage.ToString(), Color.white, 1f, 0.5f, 0.05f, Vector3.up);//DamageEffect 생성

            //피해 계산
            _enemyStatus.NowHealth -= damage - (damage * _enemyStatus.DefencePower/100);
            CallHealthChangeEvetnt();

            if (_enemyStatus.NowHealth <= _enemyStatus.MaxHealth * 0.6 && isKnife == false) //체력이 60%일 때
            {
                StartCoroutine(KnifeThrow());
            }
            //hp 0에 도달 시
            if (_enemyStatus.NowHealth <= 0)
            {
                StartCoroutine(Death());
            }
        }
        private void OnDisable()
        {
            // 출력된 이펙트 제거
            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<FadeTextEffect>() != null)
                    Destroy(child.gameObject);
            }
        }
        public void Attack() //애니메이션 공격 끝날 시점에 타격 데미지 설정
        {
            target.GetComponent<PlayerController>().Hurt(_enemyStatus.BaseDamage+_enemyStatus.AttackPower, null); //TODO 데미지는 들어감 -> But 빨간색 캐릭터만 들어감.
        }
        private void CallHealthChangeEvetnt()
        {
            var param = new BossHealthChangedParam(_enemyStatus.NowHealth, _enemyStatus.MaxHealth);
            EventManager.Instance.TriggerEvent(PandoraEventType.BossHealthChanged, param);
        }
        IEnumerator Death() //TODO 죽을 때 멈춰서 죽어야함.
        {
            anim.SetTrigger("Death");
            yield return new WaitForSeconds(1.2f);
            Destroy(this.gameObject);
        }
        IEnumerator GetBuffMode()
        {
            float delay = 15f;
            while (true)
            {
                yield return new WaitForSeconds(delay);
                anim.SetTrigger("Defense");
                _enemyStatus.DefencePower += 2f; //방어 모드 활성화 시 계속해서 방어율 증가
                Debug.Log("Defense: "+_enemyStatus.DefencePower.ToString());
            }
        }
        IEnumerator KnifeThrow() //칼을 던지는 패턴
        {
            GameObject obj = transform.Find("KnifeGenerator").gameObject;
            float delay = 5f;
            isKnife = true;
            while (true)
            {
                yield return new WaitForSeconds(delay);
                //칼 생성 및 던지기
                obj.GetComponent<KnifeGenerator>().Fire("left");
                obj.GetComponent<KnifeGenerator>().Fire("right");
                obj.GetComponent<KnifeGenerator>().Fire("up");
                obj.GetComponent<KnifeGenerator>().Fire("down");
            }
        }
    }
}
