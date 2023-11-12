using Pandora.Scripts.Player.Controller;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Skill.SkillDetail
{
    public class SkillChangeStat : PassiveSkill
    {
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

        public override void OnGetSkill()
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
        }

        public override void OnLoseSkill()
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