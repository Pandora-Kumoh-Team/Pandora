using System;
using System.Collections.Generic;
using System.Linq;
using Pandora.Scripts.Player.Skill;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pandora.Scripts.Player
{
    [Serializable]
    public class PlayerCurrentStat
    {
        // 최대 체력
        public float MaxHealth
        {
            get => _playerStat.maxHealth + _buffs.Sum(buff => buff.MaxHealthChange);
            set => _playerStat.maxHealth = value;
        }

        public float NowHealth
        {
            get => _playerStat.nowHealth + _buffs.Sum(buff => buff.NowHealthChange);
            set => _playerStat.nowHealth = value > MaxHealth ? MaxHealth : value;
        }
        
        public float BaseDamage
        {
            get => _playerStat.baseDamage + _buffs.Sum(buff => buff.BaseDamageChange);
            set => _playerStat.baseDamage = value;
        }
        
        /// <summary>
        /// 공격력 기본데메지에 곱적용 됨 (1.0f = 100%)
        /// </summary>
        public float AttackPower
        {
            get => _playerStat.attackPower + _buffs.Sum(buff => buff.AttackPowerChange);
            set => _playerStat.attackPower = value;
        }

        /// <summary>
        /// 방어율 (0~1) ex)0.5 = 50%피해
        /// </summary>
        public float DefencePower
        {
            get => _playerStat.defencePower + _buffs.Sum(buff => buff.DefencePowerChange);
            set => _playerStat.defencePower = value;
        }

        public float Speed
        {
            get => _playerStat.speed + _buffs.Sum(buff => buff.SpeedChange);
            set => _playerStat.speed = value;
        }

        public float AttackRange
        {
            get => _playerStat.attackRange + _buffs.Sum(buff => buff.AttackRangeChange);
            set => _playerStat.attackRange = value;
        }

        public float AttackSpeed
        {
            get => _playerStat.attackSpeed + _buffs.Sum(buff => buff.AttackSpeedChange);
            set => _playerStat.attackSpeed = value;
        }

        public float CriticalChance
        {
            get => _playerStat.criticalChance + _buffs.Sum(buff => buff.CriticalChanceChange);
            set => _playerStat.criticalChance = value;
        }

        /// <summary>
        /// 치명타 데메지 (2.0f = 200%)
        /// </summary>
        public float CriticalDamageTimes
        {
            get => _playerStat.criticalDamageTimes + _buffs.Sum(buff => buff.CriticalDamageChange);
            set => _playerStat.criticalDamageTimes = value;
        }

        public float DodgeChance
        {
            get => _playerStat.dodgeChance + _buffs.Sum(buff => buff.DodgeChanceChange);
            set => _playerStat.dodgeChance = value;
        }
        
        public float NonControlHpRecovery
        {
            get => _playerStat.nonControlHpRecovery;
            set => _playerStat.nonControlHpRecovery = value;
        }

        [Serializable]
        public struct PlayerStat
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
        }
        
        private PlayerStat _playerStat;
        private List<Buff> _buffs = new List<Buff>();
        private List<Buff> _attackBuffs = new List<Buff>();

        public void SetStat(PlayerStat stat)
        {
            _playerStat = stat;
        }
        
        public void PlusStat(PlayerStat stat)
        {
            _playerStat.maxHealth += stat.maxHealth;
            _playerStat.nowHealth += stat.nowHealth;
            _playerStat.baseDamage += stat.baseDamage;
            _playerStat.attackPower += stat.attackPower;
            _playerStat.defencePower += stat.defencePower;
            _playerStat.speed += stat.speed;
            _playerStat.attackRange += stat.attackRange;
            _playerStat.attackSpeed += stat.attackSpeed;
            _playerStat.criticalChance += stat.criticalChance;
            _playerStat.criticalDamageTimes += stat.criticalDamageTimes;
            _playerStat.dodgeChance += stat.dodgeChance;
            _playerStat.nonControlHpRecovery += stat.nonControlHpRecovery;
        }

        public void SubStat(PlayerStat stat)
        {
            _playerStat.maxHealth -= stat.maxHealth;
            _playerStat.nowHealth -= stat.nowHealth;
            _playerStat.baseDamage -= stat.baseDamage;
            _playerStat.attackPower -= stat.attackPower;
            _playerStat.defencePower -= stat.defencePower;
            _playerStat.speed -= stat.speed;
            _playerStat.attackRange -= stat.attackRange;
            _playerStat.attackSpeed -= stat.attackSpeed;
            _playerStat.criticalChance -= stat.criticalChance;
            _playerStat.criticalDamageTimes -= stat.criticalDamageTimes;
            _playerStat.dodgeChance -= stat.dodgeChance;
            _playerStat.nonControlHpRecovery -= stat.nonControlHpRecovery;
        }
        
        public void AddBuff(Buff buff)
        {
            _buffs.Add(buff);
        }
        
        public void AddBuffs(List<Buff> buffs)
        {
            if(buffs == null) return;
            _buffs.AddRange(buffs);
        }

        public void RemoveBuff(Buff buff)
        {
            _buffs.Remove(buff);
        }
        
        public void AddAttackBuffs(List<Buff> buffs)
        {
            _attackBuffs.AddRange(buffs);
        }
        
        public void RemoveAttackBuff(Buff buff)
        {
            _attackBuffs.Remove(buff);
        }
        
        public List<Buff> GetAttackBuffs()
        {
            return _attackBuffs;
        }
    }
}