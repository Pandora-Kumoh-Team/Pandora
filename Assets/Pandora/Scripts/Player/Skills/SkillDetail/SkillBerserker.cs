using Pandora.Scripts.Player.Controller;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillBerserker : PassiveSkill
    {
        public PlayerStat statToChange;
        private bool isActivate;

        private void Awake()
        {
            isActivate = false;
        }

        private void Update()
        {
            var pc = ownerPlayer.GetComponent<PlayerController>();
            if (pc.playerCurrentStat.nowHealth <= pc.playerCurrentStat.MaxHealth*3f/10f && !isActivate)
            {
                isActivate = true;
                pc.playerCurrentStat.playerStat += statToChange;
                pc.CallHealthChangedEvent();
            }
            else if(pc.playerCurrentStat.nowHealth > pc.playerCurrentStat.MaxHealth * 3f / 10f && isActivate)
            {
                isActivate = false;
                pc.playerCurrentStat.playerStat -= statToChange;
                pc.CallHealthChangedEvent();
            }
        }

        public override void OnGetSkill()
        {
        }

        public override void OnLoseSkill()
        {
        }
    }

}

