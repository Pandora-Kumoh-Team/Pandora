using Pandora.Scripts.Player.Controller;
using System.Collections;
using UnityEngine;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillCryoFreeze : ActiveSkill
    {
        private PlayerController _playerController;
        private float _nowDuration;
        GameObject effect;

        private void Awake()
        {
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
            transform.localPosition = Vector3.zero;
            effect.transform.localPosition = new Vector3(-0.1f, 0.05f, 0);
            effect.SetActive(true);
            ownerPlayer.GetComponent<SpriteRenderer>().color = new Color(0,140,140,255);

            _playerController = ownerPlayer.GetComponent<PlayerController>();
            _playerController.canControlMove = false;
            _playerController.playerCurrentStat.DodgeChance += 100;

            _nowDuration = duration;
        }

        public override void OnDuringSkill()
        {
            // Do nothing
        }

        public override void OnEndSkill()
        {
            effect.SetActive(false);
            ownerPlayer.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
            _playerController.canControlMove = true;
            _playerController.playerCurrentStat.DodgeChance -= 100;
        }

        private void OnDisable()
        {
            if (_nowDuration > 0)
                OnEndSkill();
        }
    }
}
