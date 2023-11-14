using System.Collections.Generic;
using Pandora.Scripts.Player;
using Pandora.Scripts.Player.Controller;
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
            player.playerCurrentStat.MaxHealth = _maxHealth;
            player.playerCurrentStat.NowHealth = _nowHealth;
            player.playerCurrentStat.BaseDamage = _baseDamage;
            player.playerCurrentStat.AttackPower = _attackPower;
            player.playerCurrentStat.DefencePower = _defencePower;
            player.playerCurrentStat.Speed = _speed;
            player.playerCurrentStat.AttackRange = _attackRange;
            player.AttackRangeChanged(_attackRange);
            player.playerCurrentStat.AttackSpeed = _attackSpeed;
            player.playerCurrentStat.CriticalChance = _criticalChance;
            player.playerCurrentStat.CriticalDamageTimes = _criticalDamageTimes;
            player.playerCurrentStat.DodgeChance = _dodgeChance;
        }
    }
}