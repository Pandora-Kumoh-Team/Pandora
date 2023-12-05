using Pandora.Scripts;
using Pandora.Scripts.Effect;
using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.System;
using Pandora.Scripts.System.Event;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public List<GameObject> knifes;

        //Animator Hashes

        //Status
        public EnemyStatus _enemyStatus;
        private void Awake()
        {
            _enemyStatus = new EnemyStatus("1StageBoss");
            knifes = new List<GameObject>();
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
            target = GameObject.FindGameObjectWithTag("Player");
            rb.velocity = Vector3.zero; 
        }
        public void Hit(HitParams hitParams)
        {
            var damage = hitParams.damage;
            anim.SetTrigger("Hit");
            transform.Find("BossHP").gameObject.SetActive(true);
            // damage effect
            var reduceDamage = damage - (damage * _enemyStatus.DefencePower / 100);
            var relativePos = new Vector3(1.5f, 1f, 0);
            hitParams.damage = reduceDamage;
            DamageTextEffectManager.Instance.SpawnDamageTextEffect(relativePos, gameObject, hitParams);

            _enemyStatus.NowHealth -= reduceDamage;
            CallHealthChangeEvetnt();
            OnHitAnimationEnd();
            if (_enemyStatus.NowHealth <= _enemyStatus.MaxHealth * 0.6 && isKnife == false) 
            {
                StartCoroutine(KnifeThrow());
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
            foreach (var knife in knifes)
            {
                Destroy(knife);
            }
        }
        public void OnHitAnimationEnd()
        {
            anim.SetBool("isFollow", true);
        }
        public void Attack()
        {
            target.GetComponent<PlayerController>().Hurt(_enemyStatus.BaseDamage, null, gameObject);
        }
        private void CallHealthChangeEvetnt()
        {
            var param = new BossHealthChangedParam(_enemyStatus.NowHealth, _enemyStatus.MaxHealth);
            EventManager.Instance.TriggerEvent(PandoraEventType.BossHealthChanged, param);
        }
        IEnumerator Death() 
        {
            anim.SetTrigger("Death");
            yield return new WaitForSeconds(1.2f);
            Destroy(this.gameObject);
            //GameManager.Instance.GameClear();
        }
        IEnumerator GetBuffMode()
        {
            float delay = 15f;
            while (true)
            {
                yield return new WaitForSeconds(delay);
                anim.SetTrigger("Defense");
                _enemyStatus.DefencePower += 2f;
                Debug.Log("Defense: "+_enemyStatus.DefencePower.ToString());
            }
        }
        IEnumerator KnifeThrow() 
        {
            GameObject obj = transform.Find("KnifeGenerator").gameObject;
            float delay = 5f;
            isKnife = true;
            while (true)
            {
                yield return new WaitForSeconds(delay);
                obj.GetComponent<KnifeGenerator>().Fire("left");
                obj.GetComponent<KnifeGenerator>().Fire("right");
                obj.GetComponent<KnifeGenerator>().Fire("up");
                obj.GetComponent<KnifeGenerator>().Fire("down");
            }
        }
    }
}
