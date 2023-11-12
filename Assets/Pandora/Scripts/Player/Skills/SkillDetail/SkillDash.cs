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

        public override void OnUseSkill()
        {
            _playerController = ownerPlayer.GetComponent<PlayerController>();
            _playerController.canControllMove = false;
            _playerController._playerStat.DodgeChance += 100;
            _nowDuration = duration;
        }

        public override void OnDuringSkill()
        {
            _playerController.rb.velocity = _playerController.lookDir * speed;
            Debug.Log(_playerController.rb.velocity);
        }

        public override void OnEndSkill()
        {
            _playerController.canControllMove = true;
            _playerController._playerStat.DodgeChance -= 100;
        }
    }
}