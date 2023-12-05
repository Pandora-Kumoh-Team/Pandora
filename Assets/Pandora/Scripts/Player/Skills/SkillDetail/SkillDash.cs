using Pandora.Scripts.Player.Controller;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillDash : ActiveSkill
    {
        private PlayerController _playerController;
        private float _nowDuration;

        [Header("수치")]
        public float speed;

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