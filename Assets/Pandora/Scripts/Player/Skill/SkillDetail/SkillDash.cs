using Pandora.Scripts.Player.Controller;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillDash : ActiveSkill
    {
        private PlayerController _playerController;
        private float _nowDuration;

        private void Update()
        {
            if(_nowDuration > 0)
            {
                _nowDuration -= Time.deltaTime;
                if(_nowDuration <= 0)
                {
                    OnEndSkill();
                }
                OnDuringSkill();
            }
        }

        private void Start()
        {
            duration = 0.5f;
        }

        public override void OnUseSkill()
        {
            _playerController = ownerPlayer.GetComponent<PlayerController>();
            _playerController.canMoving = false;
            _nowDuration = duration;
        }

        public override void OnDuringSkill()
        {
            _playerController.rb.velocity = _playerController.moveDir * 10;
        }

        public override void OnEndSkill()
        {
            _playerController.canMoving = true;
        }
    }
}