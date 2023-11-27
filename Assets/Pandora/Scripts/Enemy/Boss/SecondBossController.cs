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
        private float delay = 300f; //����ȭ
        private float AttackPatternDelay = 15f; //�ٸ� ���� ���� ������
        private bool isCharging = false;
        public float chargeSpeed = 3f; // ���� �ӵ�
        public float chargeDuration = 1f; // ���� ���� �ð�
        public float cooldownTime = 5f; // ���� ��Ÿ��
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
            target = GameObject.FindGameObjectWithTag("Player"); //�÷��̾� ã��
            rb.velocity = Vector3.zero; //�и� ����
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
        public void AnotherAttack() //�⺻ ���ݿ� n��ŭ�� �������� ����
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
        IEnumerator WIDE_AREA()//����ȭ
        {
            yield return new WaitForSeconds(delay);
            _enemyStatus.AttackPower += 10f;
            _enemyStatus.Speed += 2f;
        }
        IEnumerator StartChargingWithCooldown()
        {
            if (isCooldown)
            {
                yield break; // �̹� ���� ���̸� ����������
            }
            isCooldown = true;
            while (true)
            {
                // ���� ��Ÿ�� ��ٸ���
                yield return new WaitForSeconds(cooldownTime);

                // ���� ����
                StartCharge();

                // ������ ���� ������ ��ٸ���
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
                // ���� �������� �̵�
                transform.Translate((target.transform.position - gameObject.transform.position).normalized * chargeSpeed * Time.deltaTime);                
                yield return null; // ���� �����ӱ��� ���
            }

            // ���� ����
            isCharging = false; 
            anim.SetBool("isFollow", true);
        }
    }
}
