using System;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using static Pandora.Scripts.Player.PlayerCurrentStat;

namespace Pandora.Scripts.Player.Controller
{
    public class PermanentStatController : MonoBehaviour
    {
        // singleton
        public static PermanentStatController Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
                LoadPermanentStats();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // json file which save permanent stats
        private const string PermanentStatsFile = "permanentStats.json";
        
        // min and max values for permanent stats
        public PlayerStat p0MinStat;
        public PlayerStat p0MaxStat;
        public PlayerStat p1MinStat;
        public PlayerStat p1MaxStat;
        
        // permanent stats
        [HideInInspector]
        public PlayerStat p0PermanentStats;
        [HideInInspector]
        public PlayerStat p1PermanentStats;

        private struct WrapperStat
        {
            public PlayerStat P0PermanentStats;
            public PlayerStat P1PermanentStats;
            public int P0LeftPoints;
            public int P1LeftPoints;
            public int P0UsedPoints;
            public int P1UsedPoints;
        }

        // stat up points
        public int p0LeftPoints;
        public int p1LeftPoints;
        public int p0UsedPoints;
        public int p1UsedPoints;
        
        // save permanent stats
        public void SavePermanentStats()
        {
            var permanentStats = new WrapperStat
            {
                P0PermanentStats = p0PermanentStats,
                P1PermanentStats = p1PermanentStats,
                P0LeftPoints = p0LeftPoints,
                P1LeftPoints = p1LeftPoints,
                P0UsedPoints = p0UsedPoints,
                P1UsedPoints = p1UsedPoints
            };
            var json = JsonUtility.ToJson(permanentStats);
            File.WriteAllText(Application.persistentDataPath + "/" + PermanentStatsFile, json);
        }
        
        // load permanent stats
        public void LoadPermanentStats()
        {
            if (File.Exists(Application.persistentDataPath + "/" + PermanentStatsFile))
            {
                var json = File.ReadAllText(Application.persistentDataPath + "/" + PermanentStatsFile);
                var permanentStats = JsonUtility.FromJson<WrapperStat>(json);
                p0PermanentStats = permanentStats.P0PermanentStats;
                p1PermanentStats = permanentStats.P1PermanentStats;
                p0LeftPoints = permanentStats.P0LeftPoints;
                p1LeftPoints = permanentStats.P1LeftPoints;
                p0UsedPoints = permanentStats.P0UsedPoints;
                p1UsedPoints = permanentStats.P1UsedPoints;
            }
            else
            {
                p0PermanentStats = p0MinStat;
                p1PermanentStats = p1MinStat;
            }
        }
        
        public void MaxHpUp(int playerNum)
        {
            if (playerNum == 0)
            {
                if (p0LeftPoints == 0) return;
                if (p0PermanentStats.maxHealth >= p0MaxStat.maxHealth) return;
                var changeValue = (Instance.p0MaxStat.maxHealth - Instance.p0MinStat.maxHealth) / 5f;
                p0PermanentStats.maxHealth += changeValue;
                p0LeftPoints--;
                p0UsedPoints++;
            }
            else if (playerNum == 1)
            {
                if (p1LeftPoints == 0) return;
                if (p1PermanentStats.maxHealth >= p1MaxStat.maxHealth) return;
                var changeValue = (Instance.p1MaxStat.maxHealth - Instance.p1MinStat.maxHealth) / 5f;
                p1PermanentStats.maxHealth += changeValue;
                p1LeftPoints--;
                p1UsedPoints++;
            }
            SavePermanentStats();
        }

        public void BaseDamageUp(int playerNum)
        {
            if (playerNum == 0)
            {
                if (p0LeftPoints == 0) return;
                if (p0PermanentStats.baseDamage >= p0MaxStat.baseDamage) return;
                var changeValue = (Instance.p0MaxStat.baseDamage - Instance.p0MinStat.baseDamage) / 5f;
                p0PermanentStats.baseDamage += changeValue;
                p0LeftPoints--;
                p0UsedPoints++;
            }
            else if (playerNum == 1)
            {
                if (p1LeftPoints == 0) return;
                if (p1PermanentStats.baseDamage >= p1MaxStat.baseDamage) return;
                var changeValue = (Instance.p1MaxStat.baseDamage - Instance.p1MinStat.baseDamage) / 5f;
                p1PermanentStats.baseDamage += changeValue;
                p1LeftPoints--;
                p1UsedPoints++;
            }
            SavePermanentStats();
        }

        public void SpeedUp(int playerNum)
        {
            if (playerNum == 0)
            {
                if (p0LeftPoints == 0) return;
                if (p0PermanentStats.speed >= p0MaxStat.speed) return;
                var changeValue = (Instance.p0MaxStat.speed - Instance.p0MinStat.speed) / 5f;
                p0PermanentStats.speed += changeValue;
                p0LeftPoints--;
                p0UsedPoints++;
            }
            else if (playerNum == 1)
            {
                if (p1LeftPoints == 0) return;
                if (p1PermanentStats.speed >= p1MaxStat.speed) return;
                var changeValue = (Instance.p1MaxStat.speed - Instance.p1MinStat.speed) / 5f;
                p1PermanentStats.speed += changeValue;
                p1LeftPoints--;
                p1UsedPoints++;
            }
            SavePermanentStats();
        }

        public void AttackSpeedUp(int playerNum)
        {
            if (playerNum == 0)
            {
                if (p0LeftPoints == 0) return;
                if (p0PermanentStats.attackSpeed >= p0MaxStat.attackSpeed) return;
                var changeValue = (Instance.p0MaxStat.attackSpeed - Instance.p0MinStat.attackSpeed) / 5f;
                p0PermanentStats.attackSpeed += changeValue;
                p0LeftPoints--;
                p0UsedPoints++;
            }
            else if (playerNum == 1)
            {
                if (p1LeftPoints == 0) return;
                if (p1PermanentStats.attackSpeed >= p1MaxStat.attackSpeed) return;
                var changeValue = (Instance.p1MaxStat.attackSpeed - Instance.p1MinStat.attackSpeed) / 5f;
                p1PermanentStats.attackSpeed += changeValue;
                p1LeftPoints--;
                p1UsedPoints++;
            }
            SavePermanentStats();
        }
        
        // reset permanent stats
        public void ResetPermanentStats(int playerNum)
        {
            if (playerNum == 0)
            {
                p0PermanentStats = p0MinStat;
                p0LeftPoints = p0UsedPoints;
                p0UsedPoints = 0;
            }
            else if (playerNum == 1)
            {
                p1PermanentStats = p1MinStat;
                p1LeftPoints = p1UsedPoints;
                p1UsedPoints = 0;
            }
            SavePermanentStats();
        }
        
    }
}