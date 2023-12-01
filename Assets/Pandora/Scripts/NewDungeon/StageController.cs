using System;
using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Skill;
using Pandora.Scripts.System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pandora.Scripts.NewDungeon
{
    public class StageController : MonoBehaviour
    {
        [Serializable]
        public struct StageInfo
        {
            public int stageNumber;
            public int difficulty;
            public GameObject[] ableEnemies;
            public GameObject boss;
        }
        
        
        // Singleton
        public static StageController Instance { get; private set; }
        
        public StageInfo[] stages;
        public int currentStage;
        [HideInInspector] public StageInfo currentStageInfo;

        private void Awake()
        {
            // Singleton
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            currentStageInfo = stages[currentStage];
        }
        
        public GameObject GetRandomMob(int leftDifficulty)
        {
            var possibleMobs = Array.FindAll(currentStageInfo.ableEnemies, x => x.GetComponent<EnemyController>().difficulty <= leftDifficulty);
            if (possibleMobs.Length == 0)
            {
                return null;
            }
            return possibleMobs[UnityEngine.Random.Range(0, possibleMobs.Length)];
        }

        public void OnRoomClear()
        {
            var randomPlayer = Random.Range(0, 2);
            GameManager.Instance.GetPassiveSkill(randomPlayer);
        }
    }
}