using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using System.Collections;
using UnityEngine;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillShuriken : ActiveSkill
    {
        private PlayerController _playerController;
        private float _nowDuration;
        private GameObject effect;
        private float currentAngle;
        private Vector2 currentPos;
        private float timer;
        private Vector2 moveDir;

        [Header("데미지 (n%)")]
        public float damage;

        [Header("표창 회전 속도(n도)")]
        public float rotationSpeed;

        [Header("피해 주기(sec)")]
        public float delay;

        private void Awake()
        {
            effect = transform.Find("Effect").gameObject;
            currentAngle = 0;
            _nowDuration = 0;
        }

        private void Update()
        {
            if (_nowDuration > 0)
            {
                timer += Time.deltaTime;
                if (timer >= delay)
                {
                    StartCoroutine(ColDelayCoroutine());
                    timer = 0;
                }

                //표창 회전
                currentAngle += rotationSpeed;
                transform.rotation = Quaternion.Euler(0, 0, currentAngle);

                //표창 이동
                if(_nowDuration >= _nowDuration-1 && moveDir != Vector2.zero)
                {
                    currentPos += moveDir/60;
                    transform.position = currentPos;
                    effect.transform.localPosition = Vector3.zero;
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
            currentPos = ownerPlayer.transform.position;
            currentAngle = 0;
            _nowDuration = duration;
            _playerController = ownerPlayer.GetComponent<PlayerController>();
            transform.localPosition = Vector3.zero;
            moveDir = _playerController.moveDir;

            effect.transform.localPosition = new Vector3(0, 0, 0);
            effect.SetActive(true);
        }

        public override void OnDuringSkill() { }

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
