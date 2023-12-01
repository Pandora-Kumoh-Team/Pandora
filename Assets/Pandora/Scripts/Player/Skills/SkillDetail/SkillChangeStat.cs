using Pandora.Scripts.Player.Controller;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillChangeStat : PassiveSkill
    {
        public PlayerStat statToChange;

        public override void OnGetSkill()
        {
            var pc = ownerPlayer.GetComponent<PlayerController>();
            pc.playerCurrentStat.playerStat += statToChange;
            if(statToChange.maxHealth > 0)
                pc.playerCurrentStat.nowHealth += statToChange.maxHealth;
            else if (pc.playerCurrentStat.nowHealth > pc.playerCurrentStat.playerStat.maxHealth)
                pc.playerCurrentStat.nowHealth = pc.playerCurrentStat.playerStat.maxHealth;
            pc.CallHealthChangedEvent();
        }

        public override void OnLoseSkill()
        {
            var pc = ownerPlayer.GetComponent<PlayerController>();
            pc.playerCurrentStat.playerStat -= statToChange;
            if(statToChange.maxHealth < 0)
                pc.playerCurrentStat.nowHealth += statToChange.maxHealth;
            else if (pc.playerCurrentStat.nowHealth > pc.playerCurrentStat.playerStat.maxHealth)
                pc.playerCurrentStat.nowHealth = pc.playerCurrentStat.playerStat.maxHealth;
            pc.CallHealthChangedEvent();
        }
    }
}