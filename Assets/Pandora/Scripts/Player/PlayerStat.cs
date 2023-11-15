using System;

namespace Pandora.Scripts.Player
{
    [Serializable]
    public struct PlayerStat
    {
        public float maxHealth;
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
        
        // override operator +
        public static PlayerStat operator +(PlayerStat a, PlayerStat b)
        {
            PlayerStat result = new PlayerStat();
            result.maxHealth = a.maxHealth + b.maxHealth;
            result.baseDamage = a.baseDamage + b.baseDamage;
            result.attackPower = a.attackPower + b.attackPower;
            result.defencePower = a.defencePower + b.defencePower;
            result.speed = a.speed + b.speed;
            result.attackRange = a.attackRange + b.attackRange;
            result.attackSpeed = a.attackSpeed + b.attackSpeed;
            result.criticalChance = a.criticalChance + b.criticalChance;
            result.criticalDamageTimes = a.criticalDamageTimes + b.criticalDamageTimes;
            result.dodgeChance = a.dodgeChance + b.dodgeChance;
            result.nonControlHpRecovery = a.nonControlHpRecovery + b.nonControlHpRecovery;
            return result;
        }
        
        // override operator -
        public static PlayerStat operator -(PlayerStat a, PlayerStat b)
        {
            PlayerStat result = new PlayerStat();
            result.maxHealth = a.maxHealth - b.maxHealth;
            result.baseDamage = a.baseDamage - b.baseDamage;
            result.attackPower = a.attackPower - b.attackPower;
            result.defencePower = a.defencePower - b.defencePower;
            result.speed = a.speed - b.speed;
            result.attackRange = a.attackRange - b.attackRange;
            result.attackSpeed = a.attackSpeed - b.attackSpeed;
            result.criticalChance = a.criticalChance - b.criticalChance;
            result.criticalDamageTimes = a.criticalDamageTimes - b.criticalDamageTimes;
            result.dodgeChance = a.dodgeChance - b.dodgeChance;
            result.nonControlHpRecovery = a.nonControlHpRecovery - b.nonControlHpRecovery;
            return result;
        }
    }
}