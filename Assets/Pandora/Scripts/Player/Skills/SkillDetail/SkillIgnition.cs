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
        private bool isActivate;
        private Vector2 currentPos;

        [Header("데미지 n%")]
        public float damage;

        [Header("피해 주기(sec)")]
        public float delay;

        private float timer;

        private void Awake()
        {
            effect = transform.Find("Effect").gameObject;
            isActivate = false;
            currentPos = transform.position;
        }

        private void Start()
        {
            StartCoroutine(SkillDelay());
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (isActivate)
            {
                transform.position = currentPos;
                effect.transform.localPosition = new Vector3(0,2.6f,0);

                if(timer >= delay)
                {
                    transform.GetComponent<BoxCollider2D>().enabled = true;
                    timer = 0;
                }
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
            _nowDuration = duration;
            transform.localPosition = new Vector3(0, 0f, 0);
            isActivate = true;
            currentPos = transform.position;

            transform.GetComponent<BoxCollider2D>().enabled = true;
            effect.SetActive(true);
        }

        public override void OnDuringSkill() { }

        public override void OnEndSkill()
        {
            isActivate = false;
            transform.GetComponent<BoxCollider2D>().enabled = false;
            effect.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log("피격 판정");
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
            while (true)
            {
                transform.GetComponent<BoxCollider2D>().enabled = false;
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

}
