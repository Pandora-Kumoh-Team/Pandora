using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using UnityEngine;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillFreezing : ActiveSkill
    {
        private PlayerController _playerController;
        private float _nowDuration;
        private GameObject effect;

        [Header("둔화속도 (3*기본속도/n)")]
        public float speedDebuff;

        private void Awake()
        {
            transform.localPosition = Vector3.zero;
            effect = transform.Find("Effect").gameObject;
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
                col.GetComponent<EnemyController>()._enemyStatus.Speed *= 3f/speedDebuff;
                col.GetComponent<SpriteRenderer>().color = new Color( 0f, 1f, 1f);
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                col.GetComponent<EnemyController>()._enemyStatus.Speed *= speedDebuff/3f;
                col.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        private void OnDisable()
        {
            if (_nowDuration > 0)
                OnEndSkill();
        }
    }
}