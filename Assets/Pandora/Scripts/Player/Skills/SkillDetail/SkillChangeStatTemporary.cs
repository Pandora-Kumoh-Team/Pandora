using Pandora.Scripts.Player.Controller;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillChangeStatTemporary : ActiveSkill
    {
        [Header("양수값 입력시 증가, 음수값 입력시 감소")]
        [Header("스킬 발동 종료시 회수되는 효과")]
        public float maxHealth;
        public float nowHealth;
        public float baseDamage;
        public float attackPower;
        public float defencePower;
        public float speed;
        public float attackRange;
        public float attackSpeed;
        public float criticalChance;
        public float criticalDamageTimes;
        public float dodgeChance;
        public float nonControlHpRecovery;

        [Header("스킬 발동 종료시 회수되지 않는 효과")]
        public float maxHealthCost;
        public float nowHealthCost;
        public float baseDamageCost;
        public float attackPowerCost;
        public float defencePowerCost;
        public float speedCost;
        public float attackRangeCost;
        public float attackSpeedCost;
        public float criticalChanceCost;
        public float criticalDamageTimesCost;
        public float dodgeChanceCost;
        public float nonControlHpRecoveryCost;
        
        public override void OnUseSkill()
        {
            var pc = ownerPlayer.GetComponent<PlayerController>();
            pc._playerStat.MaxHealth += maxHealth;
            pc._playerStat.NowHealth += nowHealth;
            pc._playerStat.BaseDamage += baseDamage;
            pc._playerStat.AttackPower += attackPower;
            pc._playerStat.DefencePower += defencePower;
            pc._playerStat.Speed += speed;
            pc._playerStat.AttackRange += attackRange;
            pc._playerStat.AttackSpeed += attackSpeed;
            pc._playerStat.CriticalChance += criticalChance;
            pc._playerStat.CriticalDamageTimes += criticalDamageTimes;
            pc._playerStat.DodgeChance += dodgeChance;
            pc._playerStat.NonControlHpRecovery += nonControlHpRecovery;
            
            pc._playerStat.MaxHealth += maxHealthCost;
            pc._playerStat.NowHealth += nowHealthCost;
            pc._playerStat.BaseDamage += baseDamageCost;
            pc._playerStat.AttackPower += attackPowerCost;
            pc._playerStat.DefencePower += defencePowerCost;
            pc._playerStat.Speed += speedCost;
            pc._playerStat.AttackRange += attackRangeCost;
            pc._playerStat.AttackSpeed += attackSpeedCost;
            pc._playerStat.CriticalChance += criticalChanceCost;
            pc._playerStat.CriticalDamageTimes += criticalDamageTimesCost;
            pc._playerStat.DodgeChance += dodgeChanceCost;
            pc._playerStat.NonControlHpRecovery += nonControlHpRecoveryCost;
            pc.CallHealthChangedEvent();
        }

        public override void OnDuringSkill()
        {
            // Do nothing
        }

        public override void OnEndSkill()
        {
            var pc = ownerPlayer.GetComponent<PlayerController>();
            pc._playerStat.MaxHealth -= maxHealth;
            pc._playerStat.NowHealth -= nowHealth;
            pc._playerStat.BaseDamage -= baseDamage;
            pc._playerStat.AttackPower -= attackPower;
            pc._playerStat.DefencePower -= defencePower;
            pc._playerStat.Speed -= speed;
            pc._playerStat.AttackRange -= attackRange;
            pc._playerStat.AttackSpeed -= attackSpeed;
            pc._playerStat.CriticalChance -= criticalChance;
            pc._playerStat.CriticalDamageTimes -= criticalDamageTimes;
            pc._playerStat.DodgeChance -= dodgeChance;
            pc._playerStat.NonControlHpRecovery -= nonControlHpRecovery;
        }
    }
}