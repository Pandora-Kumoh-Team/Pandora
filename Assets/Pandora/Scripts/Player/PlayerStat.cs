using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pandora.Scripts.Player
{
    public class PlayerStat
    {
        // 최대 체력
        public float MaxHealth
        {
            get => _maxHealth + _buffs.Sum(buff => buff.MaxHealthChange);
            set => _maxHealth = value;
        }

        public float NowHealth
        {
            get => _nowHealth + _buffs.Sum(buff => buff.NowHealthChange);
            set => _nowHealth = value;
        }
        
        public float BaseDamage
        {
            get => _baseDamage + _buffs.Sum(buff => buff.BaseDamageChange);
            set => _baseDamage = value;
        }
        
        /// <summary>
        /// 공격력 기본데메지에 곱적용 됨 (1.0f = 100%)
        /// </summary>
        public float AttackPower
        {
            get => _attackPower + _buffs.Sum(buff => buff.AttackPowerChange);
            set => _attackPower = value;
        }

        /// <summary>
        /// 방어율 (0~1) ex)0.5 = 50%피해
        /// </summary>
        public float DefencePower
        {
            get => _defencePower + _buffs.Sum(buff => buff.DefencePowerChange);
            set => _defencePower = value;
        }

        public float Speed
        {
            get => _speed + _buffs.Sum(buff => buff.SpeedChange);
            set => _speed = value;
        }

        public float AttackRange
        {
            get => _attackRange + _buffs.Sum(buff => buff.AttackRangeChange);
            set => _attackRange = value;
        }

        public float AttackSpeed
        {
            get => _attackSpeed + _buffs.Sum(buff => buff.AttackSpeedChange);
            set => _attackSpeed = value;
        }

        public float CriticalChance
        {
            get => _criticalChance + _buffs.Sum(buff => buff.CriticalChanceChange);
            set => _criticalChance = value;
        }

        /// <summary>
        /// 치명타 데메지 (2.0f = 200%)
        /// </summary>
        public float CriticalDamageTimes
        {
            get => _criticalDamageTimes + _buffs.Sum(buff => buff.CriticalDamageChange);
            set => _criticalDamageTimes = value;
        }

        public float DodgeChance
        {
            get => _dodgeChance + _buffs.Sum(buff => buff.DodgeChanceChange);
            set => _dodgeChance = value;
        }

        private float _maxHealth = 100;
        private float _nowHealth = 100;
        private float _baseDamage = 10;
        private float _attackPower = 1;
        private float _defencePower = 0;
        private float _speed = 3;
        private float _attackRange = 1;
        private float _attackSpeed = 1;
        private float _criticalChance = 0;
        private float _criticalDamageTimes = 2;
        private float _dodgeChance = 0;
        private List<Buff> _buffs = new List<Buff>();
        private List<Buff> _attackBuffs = new List<Buff>();
        
        public void AddBuffs(List<Buff> buffs)
        {
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