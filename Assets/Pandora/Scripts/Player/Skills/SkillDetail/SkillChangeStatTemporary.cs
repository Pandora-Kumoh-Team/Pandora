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

        GameObject effect;

        private void Awake()
        {
            effect = transform.Find("Effect").gameObject;
        }

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
            switch (id)
            {
                case 3: //희생
                    effect.transform.localPosition = new Vector3(0.1f, 0.3f, 0);
                    break;
                case 6: //무적
                    effect.transform.localPosition = new Vector3(0,0.7f,0);
                    break;
                case 2: //경질화
                case 4: //속사
                case 9: //집중
                case 10: //각성
                case 12: //신속
                    effect.transform.localPosition = new Vector3(0, 0, 0);
                    break;
            }
            effect.SetActive(true);
        }
        public void OffEffect()
        {
            effect.SetActive(false);
        }
    }
}