using Pandora.Scripts.Player.Controller;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillChangeStatTemporary : ActiveSkill
    {
        [Header("양수값 입력시 증가, 음수값 입력시 감소")]
        [Header("스킬 발동 종료시 회수되는 효과")]
        public PlayerCurrentStat.PlayerStat temporaryStat;

        [Header("양수값 입력시 증가, 음수값 입력시 감소")]
        [Header("스킬 발동 종료시 회수되지 않는 효과")]
        public PlayerCurrentStat.PlayerStat costStat;
        
        public override void OnUseSkill()
        {
            var pc = ownerPlayer.GetComponent<PlayerController>();
            pc.playerCurrentStat.PlusStat(temporaryStat);
            pc.playerCurrentStat.PlusStat(costStat);
            pc.CallHealthChangedEvent();
        }

        public override void OnDuringSkill()
        {
            // Do nothing
        }

        public override void OnEndSkill()
        {
            var pc = ownerPlayer.GetComponent<PlayerController>();
            pc.playerCurrentStat.SubStat(temporaryStat);
            pc.CallHealthChangedEvent();
        }
    }
}