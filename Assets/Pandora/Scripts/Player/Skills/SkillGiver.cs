using System;
using Pandora.Scripts.NewDungeon;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.System;
using Pandora.Scripts.UI;
using UnityEngine;
using static Pandora.Scripts.Player.Skill.Skill;

namespace Pandora.Scripts.Player.Skills
{
    public class SkillGiver : OnEnableNearPlayerCheckObject
    {
        private ParticleSystem _particleSystem;
        
        public bool isActiveSkill;
        public int playerId;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            DistanceToPlayer = 2f;
        }

        protected override void OnEnableNearPlayer()
        {
            base.OnEnableNearPlayer();
            _particleSystem.Stop();
        }

        protected override void OnPlayerGetDistance()
        {
            base.OnPlayerGetDistance();
            _particleSystem.Play();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Player")) return;
            if (!col.gameObject.GetComponent<PlayerController>().isControlByPlayer) return;
            if (IsPlayerStillNearAfterEnable) return;
        
            var skillType = isActiveSkill? SkillType.Active : SkillType.Passive;
            GameManager.Instance.inGameCanvas.GetComponent<InGameCanvasManager>()
                .DisplaySkillSelection(skillType, playerId);
            Destroy(transform.parent.gameObject);
            
        }
        
    }
}