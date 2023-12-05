using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using UnityEngine;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillRush : ActiveSkill
    {
        private PlayerController _playerController;
        private float _nowDuration;

        [Header("수치")]
        public float speed;

        private void Awake()
        {
            transform.localPosition = Vector3.zero;
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
            _playerController.isTrigger = true;
            _playerController.canControlMove = false;
            _playerController.playerCurrentStat.DodgeChance += 100;
            _nowDuration = duration;
        }

        public override void OnDuringSkill()
        {
            _playerController.rb.velocity = _playerController.lookDir * speed;
        }

        public override void OnEndSkill()
        {
            transform.GetComponent<CircleCollider2D>().enabled = false;
            _playerController.isTrigger = false;
            _playerController.canControlMove = true;
            _playerController.playerCurrentStat.DodgeChance -= 100;
        }

        private void OnTriggerEnter2D(Collider2D col) //몸 데미지
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                var hitParams = new HitParams();

                // 크리티컬 여부 판단
                var rand = Random.Range(0, 100);
                hitParams.damage = _playerController.playerCurrentStat.BaseDamage * _playerController.playerCurrentStat.AttackPower;
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