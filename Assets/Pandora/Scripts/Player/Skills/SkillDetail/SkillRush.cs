using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillRush : ActiveSkill
    {
        private PlayerController _playerController;
        private float _nowDuration;
        private GameObject effect1;
        private GameObject effect2;
        private Vector2 currentPos;

        [Header("수치")]
        public float speed;

        private void Awake()
        {
            transform.localPosition = Vector3.zero;
            effect1 = transform.Find("Effect1").gameObject;
            effect2 = transform.Find("Effect2").gameObject;
        }

        private void Update()
        {
            if( effect2.active)
            {
                effect2.transform.position = currentPos;
            }
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
            _playerController.canControlMove = false;
            transform.GetComponent<CircleCollider2D>().enabled = true;
            _playerController.playerCurrentStat.DodgeChance += 100;
            _nowDuration = duration;

            if(_playerController.lookDir.x <0 && _playerController.lookDir.y == 0 )
            {
                effect1.transform.localPosition = new Vector3(1,0,0);
                effect1.transform.rotation = Quaternion.Euler(0,0,0);
                effect2.transform.localPosition = new Vector3(-1,0,0);
            }
            else if(_playerController.lookDir.x < 0 && _playerController.lookDir.y > 0)
            {
                effect1.transform.localPosition = new Vector3(0.5f,-0.5f, 0);
                effect1.transform.rotation = Quaternion.Euler(0, 0, -45f);
                effect2.transform.localPosition = new Vector3(-0.6f, 0.6f, 0);
            }
            else if (_playerController.lookDir.x == 0 && _playerController.lookDir.y > 0)
            {
                effect1.transform.localPosition = new Vector3(0, -1, 0);
                effect1.transform.rotation = Quaternion.Euler(0, 0, -90f);
                effect2.transform.localPosition = new Vector3(0, 1f, 0);
            }
            else if (_playerController.lookDir.x>0 && _playerController.lookDir.y > 0)
            {
                effect1.transform.localPosition = new Vector3(-0.6f,-0.6f, 0);
                effect1.transform.rotation = Quaternion.Euler(0, 0, -135f);
                effect2.transform.localPosition = new Vector3(0.5f, 0.5f, 0);
            }
            else if (_playerController.lookDir.x > 0 && _playerController.lookDir.y == 0)
            {
                effect1.transform.localPosition = new Vector3(-1f, 0, 0);
                effect1.transform.rotation = Quaternion.Euler(0, 0, -180f);
                effect2.transform.localPosition = new Vector3(0.5f, 0, 0);
            }

            else if (_playerController.lookDir.x > 0 && _playerController.lookDir.y < 0)
            {
                effect1.transform.localPosition = new Vector3(-0.7f, 0.9f, 0);
                effect1.transform.rotation = Quaternion.Euler(0, 0, -225f);
                effect2.transform.localPosition = new Vector3(0.5f, -0.5f, 0);
            }
            else if (_playerController.lookDir.x == 0 && _playerController.lookDir.y < 0)
            {
                effect1.transform.localPosition = new Vector3(0, 1.2f, 0);
                effect1.transform.rotation = Quaternion.Euler(0, 0, -270f);
                effect2.transform.localPosition = new Vector3(0, -0.5f, 0);
            }
            else if (_playerController.lookDir.x < 0 && _playerController.lookDir.y < 0)
            {
                effect1.transform.localPosition = new Vector3(0.7f, 0.7f, 0);
                effect1.transform.rotation = Quaternion.Euler(0, 0, -315f);
                effect2.transform.localPosition = new Vector3(-0.5f, -0.5f, 0);
            }
            StartCoroutine(Effect1Coroutine());
        }

        public override void OnDuringSkill()
        {
            _playerController.rb.velocity = _playerController.lookDir * speed;
        }

        public override void OnEndSkill()
        {
            _playerController.canControlMove = true;
            _playerController.playerCurrentStat.DodgeChance -= 100;
            transform.GetComponent<CircleCollider2D>().enabled = false;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (_nowDuration == 0 && col.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
            var hitParams = new HitParams();

            // 크리티컬 여부 판단
            var rand = Random.Range(0, 100);
            hitParams.damage = _playerController.playerCurrentStat.BaseDamage * _playerController.playerCurrentStat.AttackPower;
            if (rand < _playerController.playerCurrentStat.CriticalChance)
            {
                hitParams.damage *= _playerController.playerCurrentStat.CriticalDamageTimes;
                hitParams.isCritical = true;
            }
            col.gameObject.GetComponent<IHitAble>().Hit(hitParams);
            StartCoroutine(Effect2Coroutine());
        }

        private IEnumerator Effect2Coroutine()
        {
            effect2.SetActive(true);
            currentPos = effect2.transform.position;
            yield return new WaitForSeconds(0.667f);
            effect2.SetActive(false);
        }

        private IEnumerator Effect1Coroutine()
        {
            effect1.SetActive(true);
            yield return new WaitForSeconds(0.583f);
            effect1.SetActive(false);
        }

        private void OnDisable()
        {
            if (_nowDuration > 0)
                OnEndSkill();
        }
    }
}