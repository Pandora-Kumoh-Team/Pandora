using Pandora.Scripts.Player.Controller;
using System.Collections;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillChangeStatTemporary : ActiveSkill
    {
        private float _nowDuration;

        [Header("양수값 입력시 증가, 음수값 입력시 감소")]
        [Header("스킬 발동 종료시 회수되는 효과")]
        public PlayerStat temporaryStat;

        [Header("양수값 입력시 증가, 음수값 입력시 감소")]
        [Header("스킬 발동 종료시 회수되지 않는 효과")]
        public PlayerStat costStat;

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
            var pc = ownerPlayer.GetComponent<PlayerController>();
            pc.playerCurrentStat.playerStat += temporaryStat;
            pc.playerCurrentStat.playerStat += costStat;
            _nowDuration = duration;
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
            GameObject Effect = transform.Find("Effect").gameObject;
            switch (id)
            {
                case 3: //희생
                    Effect.transform.localPosition = new Vector3(0.1f, 0.3f, 0);
                    Effect.SetActive(true);
                    break;
                case 6: //무적
                    Effect.transform.localPosition = new Vector3(0,0.7f,0);
                    Effect.SetActive(true);
                    break;
                case 9: //집중
                case 10: //각성
                    Effect.transform.localPosition = new Vector3(0, 0, 0);
                    Effect.SetActive(true);
                    break;
            }
        }
        public void OffEffect()
        {
            switch (id)
            {
                case 3: //희생
                case 6: //무적
                case 9: //집중
                case 10://각성
                    GameObject Effect = transform.Find("Effect").gameObject;
                    Effect.SetActive(false);
                    break;
            }
        }
    }
}