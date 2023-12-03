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

        [Header("���ط� n%")]
        public float damage;

        [Header("���� �ֱ�")]
        public float delay;


        private void Awake()
        {
            transform.localPosition = Vector3.zero;
            effect = transform.Find("Effect").gameObject;
        }

        private void Start()
        {
            StartCoroutine(SkillDelay());
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (_nowDuration > 0)
            {
                _nowDuration -= Time.deltaTime;
                if (_nowDuration <= 0)
                {
                    OnEndSkill();
                }
                OnDuringSkill();
            }

            if(timer >= delay)
            {
                transform.GetComponent<CircleCollider2D>().enabled = true;
                timer = 0;
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
                col.GetComponent<EnemyController>().Hit(hitParams);
            }
        }

        private IEnumerator SkillDelay()
        {
            while(true)
            {
                transform.GetComponent<CircleCollider2D>().enabled = false;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}