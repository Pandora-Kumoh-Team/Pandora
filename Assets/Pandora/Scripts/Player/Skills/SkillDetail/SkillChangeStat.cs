using Pandora.Scripts.Player.Controller;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillChangeStat : PassiveSkill
    {
        public PlayerCurrentStat.PlayerStat statToChange;

        public override void OnGetSkill()
        {
            var pc = ownerPlayer.GetComponent<PlayerController>();
            pc.playerCurrentStat.PlusStat(statToChange);
            pc.CallHealthChangedEvent();
        }

        public override void OnLoseSkill()
        {
            var pc = ownerPlayer.GetComponent<PlayerController>();
            pc.playerCurrentStat.SubStat(statToChange);
            pc.CallHealthChangedEvent();
        }
    }
}