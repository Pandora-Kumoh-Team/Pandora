using System;
using System.Collections;
using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.Player.Skill;
using Pandora.Scripts.System;
using Pandora.Scripts.System.Event;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Pandora.Scripts.NewDungeon
{
    public class StageController : MonoBehaviour, IEventListener
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
        public StageInfo currentStageInfo;

        private void Awake()
        {
            // Singleton
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            currentStageInfo = stages[currentStage];
            // add scene load event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private IEnumerator LateAwake()
        {
            yield return null;
            EventManager.Instance.AddListener(PandoraEventType.MapGenerateComplete, this);
        }
        
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            EventManager.Instance.RemoveListener(PandoraEventType.MapGenerateComplete, this);
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
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
        }

        public void OnEvent(PandoraEventType pandoraEventType, Component sender, object param = null)
        {
            if(pandoraEventType == PandoraEventType.MapGenerateComplete)
            {
                PlayerManager.Instance.ActivePlayers();
            }
        }
    }
}