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
            get => playerStat.maxHealth + _buffs.Sum(buff => buff.MaxHealthChange);
            set => playerStat.maxHealth = value;
        }

        public float NowHealth
        {
            get => nowHealth + _buffs.Sum(buff => buff.NowHealthChange);
            set => nowHealth = value > MaxHealth ? MaxHealth : value;
        }
        
        public float BaseDamage
        {
            get => playerStat.baseDamage + _buffs.Sum(buff => buff.BaseDamageChange);
            set => playerStat.baseDamage = value;
        }
        
        /// <summary>
        /// 공격력 기본데메지에 곱적용 됨 (1.0f = 100%)
        /// </summary>
        public float AttackPower
        {
            get => playerStat.attackPower + _buffs.Sum(buff => buff.AttackPowerChange);
            set => playerStat.attackPower = value;
        }

        /// <summary>
        /// 방어율 (0~1) ex)0.5 = 50%피해
        /// </summary>
        public float DefencePower
        {
            get => playerStat.defencePower + _buffs.Sum(buff => buff.DefencePowerChange);
            set => playerStat.defencePower = value;
        }

        public float Speed
        {
            get => playerStat.speed + _buffs.Sum(buff => buff.SpeedChange);
            set => playerStat.speed = value;
        }

        public float AttackRange
        {
            get => playerStat.attackRange + _buffs.Sum(buff => buff.AttackRangeChange);
            set => playerStat.attackRange = value;
        }

        public float AttackSpeed
        {
            get => playerStat.attackSpeed + _buffs.Sum(buff => buff.AttackSpeedChange);
            set => playerStat.attackSpeed = value;
        }

        public float CriticalChance
        {
            get => playerStat.criticalChance + _buffs.Sum(buff => buff.CriticalChanceChange);
            set => playerStat.criticalChance = value;
        }

        /// <summary>
        /// 치명타 데메지 (2.0f = 200%)
        /// </summary>
        public float CriticalDamageTimes
        {
            get => playerStat.criticalDamageTimes + _buffs.Sum(buff => buff.CriticalDamageChange);
            set => playerStat.criticalDamageTimes = value;
        }

        public float DodgeChance
        {
            get => playerStat.dodgeChance + _buffs.Sum(buff => buff.DodgeChanceChange);
            set => playerStat.dodgeChance = value;
        }
        
        public float NonControlHpRecovery
        {
            get => playerStat.nonControlHpRecovery;
            set => playerStat.nonControlHpRecovery = value;
        }

        [FormerlySerializedAs("_playerStat")] public PlayerStat playerStat;
        public float nowHealth;
        private List<Buff> _buffs = new();
        private List<Buff> _attackBuffs = new();

        public void Init()
        {
            nowHealth = playerStat.maxHealth;
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