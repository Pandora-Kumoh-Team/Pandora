using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using System.Collections;
using UnityEngine;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillIgnition : ActiveSkill
    {
        private PlayerController _playerController;
        private float _nowDuration;
        private GameObject effect;
        private Vector2 currentPos;

        [Header("데미지 n%")]
        public float damage;

        [Header("피해 주기(sec)")]
        public float delay;

        private float timer;

        private void Awake()
        {
            effect = transform.Find("Effect").gameObject;
            currentPos = transform.position;
        }

        private void Update()
        {
            if (_nowDuration > 0)
            {
                timer += Time.deltaTime;

                transform.position = currentPos;
                effect.transform.localPosition = new Vector3(0, 2.6f, 0);

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
            transform.localPosition = new Vector3(0, 0f, 0);
            currentPos = transform.position;

            effect.SetActive(true);
        }

        public override void OnDuringSkill() { }

        public override void OnEndSkill()
        {
            transform.GetComponent<BoxCollider2D>().enabled = false;
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
            transform.GetComponent<BoxCollider2D>().enabled = true;
            yield return new WaitForSeconds(0.1f);
            transform.GetComponent<BoxCollider2D>().enabled = false;
        }

        private void OnDisable()
        {
            if (_nowDuration > 0)
                OnEndSkill();
        }
    }

}
