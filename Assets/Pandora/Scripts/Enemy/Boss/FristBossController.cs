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
            _enemyStatus = new EnemyStatus("1StageBoss");
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
            target = GameObject.FindGameObjectWithTag("Player"); //Player ��� ����
            rb.velocity = Vector3.zero; //�и� ����
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
                .Init(reduceDamage.ToString(), Color.white, 1f, 0.5f, 0.05f, Vector3.up);//DamageEffect ����

            //���� ���
            _enemyStatus.NowHealth -= reduceDamage;
            CallHealthChangeEvetnt();

            if (_enemyStatus.NowHealth <= _enemyStatus.MaxHealth * 0.6 && isKnife == false) //ü���� 60%�� ��
            {
                StartCoroutine(KnifeThrow());
            }
            //hp 0�� ���� ��
            if (_enemyStatus.NowHealth <= 0)
            {
                StartCoroutine(Death());
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
            Debug.Log("�⺻ ����");
            target.GetComponent<PlayerController>().Hurt(_enemyStatus.AttackPower, null, gameObject); //TODO �������� �� -> But ������ ĳ���͸� ��.
        }
        private void CallHealthChangeEvetnt()
        {
            var param = new BossHealthChangedParam(_enemyStatus.NowHealth, _enemyStatus.MaxHealth);
            EventManager.Instance.TriggerEvent(PandoraEventType.BossHealthChanged, param);
        }
        IEnumerator Death() //TODO ���� �� ���缭 �׾����.
        {
            anim.SetTrigger("Death");
            yield return new WaitForSeconds(1.2f);
            Destroy(this.gameObject);
            GameManager.Instance.GameClear();
        }
        IEnumerator GetBuffMode()
        {
            float delay = 15f;
            while (true)
            {
                yield return new WaitForSeconds(delay);
                anim.SetTrigger("Defense");
                _enemyStatus.DefencePower += 2f; //��� ��� Ȱ��ȭ �� ����ؼ� ����� ����
                Debug.Log("Defense: "+_enemyStatus.DefencePower.ToString());
            }
        }
        IEnumerator KnifeThrow() //Į�� ������ ����
        {
            GameObject obj = transform.Find("KnifeGenerator").gameObject;
            float delay = 5f;
            isKnife = true;
            while (true)
            {
                yield return new WaitForSeconds(delay);
                //Į ���� �� ������
                obj.GetComponent<KnifeGenerator>().Fire("left");
                obj.GetComponent<KnifeGenerator>().Fire("right");
                obj.GetComponent<KnifeGenerator>().Fire("up");
                obj.GetComponent<KnifeGenerator>().Fire("down");
            }
        }
    }
}
