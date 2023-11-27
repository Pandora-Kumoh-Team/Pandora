using Pandora.Scripts.Player.Controller;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillChangeStatTemporary : ActiveSkill
    {
        [Header("양수값 입력시 증가, 음수값 입력시 감소")]
        [Header("스킬 발동 종료시 회수되는 효과")]
        public PlayerStat temporaryStat;

        [Header("양수값 입력시 증가, 음수값 입력시 감소")]
        [Header("스킬 발동 종료시 회수되지 않는 효과")]
        public PlayerStat costStat;
        
        public override void OnUseSkill()
        {
            transform.localPosition = Vector3.zero;
            var pc = ownerPlayer.GetComponent<PlayerController>();
            pc.playerCurrentStat.playerStat += temporaryStat;
            pc.playerCurrentStat.playerStat += costStat;
            pc.CallHealthChangedEvent();
            OnEffect();
        }

        public override void OnDuringSkill()
        {
            // Do nothing
        }

        public override void OnEndSkill()
        {
            OffEffect();
            var pc = ownerPlayer.GetComponent<PlayerController>();
            pc.playerCurrentStat.playerStat -= temporaryStat;
            pc.CallHealthChangedEvent();
        }


        public void OnEffect()
        {
            switch (id)
            {
                case 6: //무적
                    GameObject Effect = transform.Find("Effect").gameObject;
                    Effect.transform.localPosition = new Vector3(0,0.7f,0);
                    Effect.SetActive(true);
                    break;
            }
        }
        public void OffEffect()
        {
            switch (id)
            {
                case 6: //무적
                    GameObject Effect = transform.Find("Effect").gameObject;
                    Effect.SetActive(false);
                    break;
            }
        }
    }
}