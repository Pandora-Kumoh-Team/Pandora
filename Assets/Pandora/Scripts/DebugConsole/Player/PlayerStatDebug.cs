using System.Collections.Generic;
using Pandora.Scripts.Player;
using UnityEngine;

namespace Pandora.Scripts.DebugConsole.Player
{
    public class PlayerStatDebug : MonoBehaviour
    {
        public float _maxHealth = 100;
        public float _nowHealth = 100;
        public float _baseDamage = 10;
        public float _attackPower = 1;
        public float _defencePower = 0;
        public float _speed = 3;
        public float _attackRange = 1;
        public float _attackSpeed = 1;
        public float _criticalChance = 0;
        public float _criticalDamageTimes = 2;
        public float _dodgeChance = 0;
        public List<Buff> _buffs = new List<Buff>();
        public List<Buff> _attackBuffs = new List<Buff>();
        
        private PlayerController player;
        
        private void Start()
        {
            //FInd Player
            player = GameObject.Find("PlayerCharacterRanged").GetComponent<PlayerController>();
        }

        private void Update()
        {
            // Update Player Stats
            player._playerStat.MaxHealth = _maxHealth;
            player._playerStat.NowHealth = _nowHealth;
            player._playerStat.BaseDamage = _baseDamage;
            player._playerStat.AttackPower = _attackPower;
            player._playerStat.DefencePower = _defencePower;
            player._playerStat.Speed = _speed;
            player._playerStat.AttackRange = _attackRange;
            player.AttackRangeChanged(_attackRange);
            player._playerStat.AttackSpeed = _attackSpeed;
            player._playerStat.CriticalChance = _criticalChance;
            player._playerStat.CriticalDamageTimes = _criticalDamageTimes;
            player._playerStat.DodgeChance = _dodgeChance;
        }
    }
}