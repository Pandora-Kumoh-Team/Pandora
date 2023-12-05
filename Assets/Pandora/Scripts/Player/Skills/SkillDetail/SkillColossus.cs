using Cinemachine.Utility;
using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using System.Collections;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillColossus : ActiveSkill
    {
        private PlayerController _playerController;
        private float _nowDuration;
        private GameObject effect1;
        private GameObject effect2;
        private Vector2 currentPos;

        [Header("��ġ")]
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
            _nowDuration = duration;
            _playerController = ownerPlayer.GetComponent<PlayerController>();
            transform.localPosition = new Vector3(1, 0, 0);

            if (_playerController.moveDir.x >=0)
            {
                transform.localPosition = new Vector3(1.7f, 0, 0);
                effect1.GetComponent<SpriteRenderer>().flipX = false;
                effect1.transform.localPosition = new Vector3(-0.7f,0,0);
                effect2.transform.localPosition = new Vector3(-0.75f, 0, 0);
            }
            else
            {
                transform.localPosition = new Vector3(-1.7f, 0, 0);
                effect1.GetComponent<SpriteRenderer>().flipX = true;
                effect1.transform.localPosition = new Vector3(0.7f, 0, 0);
                effect2.transform.localPosition = new Vector3(0.75f, 0, 0);
            }
            currentPos = transform.position;

            StartCoroutine(ColDelayCoroutine());

            effect1.SetActive(true);
            StartCoroutine(Effect2Coroutine());
        }

        public override void OnDuringSkill()
        {
        }

        public override void OnEndSkill()
        {
            transform.GetComponent<BoxCollider2D>().enabled = false;
            effect1.SetActive(false);
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
            yield return new WaitForSeconds(0.3f);
            transform.GetComponent<BoxCollider2D>().enabled = true;
            yield return new WaitForSeconds(0.1f);
            transform.GetComponent<BoxCollider2D>().enabled = false;
        }

        private IEnumerator Effect2Coroutine()
        {
            effect2.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            effect2.SetActive(false);
        }

        private void OnDisable()
        {
            if (_nowDuration > 0)
                OnEndSkill();
        }
    }
}