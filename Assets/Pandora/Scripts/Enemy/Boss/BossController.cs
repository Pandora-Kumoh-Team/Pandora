using Pandora.Scripts;
using Pandora.Scripts.Effect;
using Pandora.Scripts.Enemy;
using Pandora.Scripts.System;
using Pandora.Scripts.System.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Pandora.Scripts.Enemy
{
    public class BossController : MonoBehaviour, IHitAble
    {
        //Component
        private Rigidbody2D rb;
        private Animator anim;
        private PolygonCollider2D polygonCollider;

        //Animator Hashes

        //Status
        public EnemyStatus _enemyStatus;
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            polygonCollider = GetComponent<PolygonCollider2D>();

            _enemyStatus = new EnemyStatus(this.gameObject.name);
        }

        // Update is called once per frame
        void Update()
        {
            rb.velocity = Vector3.zero; //�и� ����
        }
        public void Hit(float damage, List<Buff> buff)
        {
            anim.SetTrigger("Hit");

            //damage effect
            var effectPosition = transform.position + new Vector3(1.5f, 1f, 0);
            var damageEffect = Instantiate(GameManager.Instance.damageEffect, effectPosition, Quaternion.identity, transform);
            damageEffect.GetComponent<FadeTextEffect>()
                .Init(damage.ToString(), Color.white, 1f, 0.5f, 0.05f, Vector3.up);//DamageEffect ����

            //���� ���
            _enemyStatus.NowHealth -= damage;
            CallHealthChangeEvetnt();

            //hp 0�� ���� ��
            if (_enemyStatus.NowHealth <= 0)
            {
                anim.SetTrigger("Death"); //�״� ��� -> �״� ��� ������ ����
                Destroy(this.gameObject);
            }
        }
        private void OnDisable()
        {
            // ��µ� ����Ʈ ����
            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<FadeTextEffect>() != null)
                    Destroy(child.gameObject);
            }
        }
        public void Attack() //�ִϸ��̼� ���� ���� ������ Ÿ�� ������ ����
        {
            Debug.Log("Player Attack");
        }
        private void CallHealthChangeEvetnt()
        {
            var param = new BossHealthChangedParam(_enemyStatus.NowHealth, _enemyStatus.MaxHealth);
            EventManager.Instance.TriggerEvent(PandoraEventType.BossHealthChanged, param);
        }
    }
}
