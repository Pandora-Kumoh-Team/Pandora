using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using System.Collections;
using UnityEngine;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillPoison : ActiveSkill
    {
        private PlayerController _playerController;
        private float _nowDuration;
        private GameObject effect;
        private float timer;

        [Header("피해량 n%")]
        public float damage;

        [Header("피해 주기")]
        public float delay;


        private void Awake()
        {
            transform.localPosition = Vector3.zero;
            effect = transform.Find("Effect").gameObject;
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (_nowDuration > 0)
            {
                if (timer >= delay)
                {
                    StartCoroutine(ColDelayCoroutine());
                    timer = 0;
                }

                _nowDuration -= Time.deltaTime;
                if (_nowDuration <= 0)
                {
                    OnEndSkill();
                }
                OnDuringSkill();
            }
        }

        public override void OnUseSkill()
        {
            _playerController = ownerPlayer.GetComponent<PlayerController>();
            _nowDuration = duration;

            effect.transform.localPosition = new Vector3(0, 0, 0);
            effect.SetActive(true);
        }

        public override void OnDuringSkill() {}

        public override void OnEndSkill()
        {
            transform.GetComponent<CircleCollider2D>().enabled = false;
            effect.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                var hitParams = new HitParams();

                var rand = Random.Range(0, 100);
                hitParams.damage = _playerController.playerCurrentStat.BaseDamage * _playerController.playerCurrentStat.AttackPower * (damage * 0.01f);
                if (rand < _playerController.playerCurrentStat.CriticalChance)
                {
                    hitParams.damage *= _playerController.playerCurrentStat.CriticalDamageTimes;
                    hitParams.isCritical = true;
                }
                col.GetComponent<IHitAble>().Hit(hitParams);
            }
        }

        private IEnumerator ColDelayCoroutine()
        {
            transform.GetComponent<CircleCollider2D>().enabled = true;
            yield return new WaitForSeconds(0.1f);
            transform.GetComponent<CircleCollider2D>().enabled = false;
        }

        private void OnDisable()
        {
            if (_nowDuration > 0)
                OnEndSkill();
        }
    }
}