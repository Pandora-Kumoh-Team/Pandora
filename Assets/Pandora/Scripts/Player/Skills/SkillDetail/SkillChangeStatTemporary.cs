using Pandora.Scripts.Player.Controller;
using System.Collections;
using Pandora.Scripts.System.Event;
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
            if(pc.playerNumber == 0 && id == 13) // 스킬 무기의 달인 근접일시 공격 사거리 효과 감소
            {
                temporaryStat.attackRange /= 3;
                costStat.attackRange /= 3;
            }
            if (id == 3) // 스킬 희생 체력 소모
                pc.playerCurrentStat.nowHealth -= 10;
            pc.playerCurrentStat.playerStat += temporaryStat;
            pc.playerCurrentStat.playerStat += costStat;
            _nowDuration = duration;
            pc.CallHealthChangedEvent();
            if(temporaryStat.attackRange + costStat.attackRange != 0)
            {
                pc.AttackRangeChanged(pc.playerCurrentStat.playerStat.attackRange);
            }
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
            if(temporaryStat.attackRange != 0)
            {
                pc.AttackRangeChanged(pc.playerCurrentStat.playerStat.attackRange);
            }
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
                case 13: //무기의 달인
                    effect.transform.localPosition = new Vector3(0, 0, 0);
                    break;
            }
            effect.SetActive(true);
        }
        public void OffEffect()
        {
            effect.SetActive(false);
        }

        private void OnDisable()
        {
            if(_nowDuration >0)
                OnEndSkill();
        }
    }
}