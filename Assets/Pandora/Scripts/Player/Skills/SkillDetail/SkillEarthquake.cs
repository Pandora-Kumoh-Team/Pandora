using Cinemachine;
using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillEarthquake : ActiveSkill
    {
        private PlayerController _playerController;
        private float _nowDuration;
        private GameObject effect;
        private CinemachineImpulseSource _impulseSource;

        [Header("수치")]
        public float damage;

        private void Awake()
        {
            transform.localPosition = Vector3.zero;
            effect = transform.Find("Effect").gameObject;
            _impulseSource =GetComponent<CinemachineImpulseSource>();
        }

        private void Update()
        {
            if (_nowDuration > 0)
            {
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

            transform.GetComponent<CircleCollider2D>().enabled = true;
            _impulseSource.GenerateImpulse();
            _nowDuration = duration;

            effect.transform.localPosition = new Vector3(0, 0.7f, 0);
            effect.SetActive(true);
        }

        public override void OnDuringSkill()
        {
        }

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

                // 크리티컬 여부 판단
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

        private void OnDisable()
        {
            if (_nowDuration > 0)
                OnEndSkill();
        }
    }
}