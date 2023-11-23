using Pandora.Scripts.Effect;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.System;
using Pandora.Scripts.System.Event;
using System.Collections;
using System.Collections.Generic;
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
        public void Hit(float damage, List<Buff> buff)
        {
            anim.SetTrigger("Hit");
            transform.Find("BossHP").gameObject.SetActive(true);
            //damage effect
            var effectPosition = transform.position + new Vector3(1.5f, 1f, 0);
            var damageEffect = Instantiate(GameManager.Instance.damageEffect, effectPosition, Quaternion.identity, transform);
            var reduceDamage = damage - (damage * _enemyStatus.DefencePower / 100);
            damageEffect.GetComponent<FadeTextEffect>()
                .Init(reduceDamage.ToString(), Color.white, 1f, 0.5f, 0.05f, Vector3.up);

            _enemyStatus.NowHealth -= reduceDamage;
            CallHealthChangeEvetnt();
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
            target.GetComponent<PlayerController>().Hurt(_enemyStatus.AttackPower, null, gameObject);
        }
        private void CallHealthChangeEvetnt()
        {
            var param = new BossHealthChangedParam(_enemyStatus.NowHealth, _enemyStatus.MaxHealth);
            EventManager.Instance.TriggerEvent(PandoraEventType.BossHealthChanged, param);
        }
    }
}
