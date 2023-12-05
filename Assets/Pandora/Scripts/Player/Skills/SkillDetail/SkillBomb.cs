using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using System.Collections;
using UnityEngine;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillBomb : ActiveSkill
    {
        private PlayerController _playerController;
        private float _nowDuration;
        private GameObject effect1;
        private GameObject effect2;

        private Vector2 currentPos;
        private float timer;

        [Header("µ¥¹ÌÁö (n%)")]
        public float damage;

        private void Awake()
        {
            effect1 = transform.Find("Effect1").gameObject;
            effect2 = transform.Find("Effect2").gameObject;
        }

        private void Update()
        {
            if (_nowDuration > 0)
            {
                transform.position = currentPos;
                effect1.GetComponent<SpriteRenderer>().color = new Color(1f , 1f/(duration-_nowDuration) , 1f/(duration - _nowDuration));

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
            currentPos = ownerPlayer.transform.position;
            transform.localPosition = Vector3.zero;

            effect1.transform.localPosition = new Vector3(0, 0, 0);
            effect2.transform.localPosition = new Vector3(0, 0, 0);
            StartCoroutine(BombCoroutine());
        }

        public override void OnDuringSkill() { }

        public override void OnEndSkill()
        {
            effect2.SetActive(false);
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

        private IEnumerator BombCoroutine()
        {
            effect1.SetActive(true);
            yield return new WaitForSeconds(3f);
            effect1.SetActive(false);
            effect2.SetActive(true);
            transform.GetComponent<CircleCollider2D>().enabled = true;
            yield return new WaitForSeconds(0.2f);
            transform.GetComponent<CircleCollider2D>().enabled = false;
        }

        private void OnDisable()
        {
            if (_nowDuration > 0)
                OnEndSkill();
        }
    }
}
