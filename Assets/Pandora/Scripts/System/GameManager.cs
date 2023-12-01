using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Skill;
using Pandora.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.System
{
    public class GameManager : MonoBehaviour
    {
        public PoolManager pool;

        // Singleton class
        public static GameManager Instance { get; private set; }
        
        // Singleton Awake
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("GameManager already exists!");
                Destroy(this);
            }
            
            // 씬 로드시 할 행동
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        public Canvas inGameCanvas;
        
        // 숫차로 출력되는 데메지 이펙트 프리팹
        // Insistate 후 Init() 호출하여 사용
        public GameObject damageEffect;
        public GameObject bloodParticle;
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(scene.name == "MainMenu")
            {
                // TODO
            }
            else
            {
                inGameCanvas = GameObject.Find("InGameCanvas").GetComponent<Canvas>();
                if (FindObjectOfType<DamageTextEffectManager>() == null)
                {
                    var damageTextEffectManager = new GameObject("DamageTextEffectManager");
                    damageTextEffectManager.AddComponent<DamageTextEffectManager>();
                    DamageTextEffectManager.Instance.damageEffectPrefab = damageEffect;
                }
            }
        }
        
        public static void ExitGame()
        {
            // 게임 종료 전처리가 필요하다면 여기서 처리
            Application.Quit();
        }

        public void GameOver()
        {
            Time.timeScale = 0;
            inGameCanvas.transform.Find("GameOverPanel").gameObject.SetActive(true);
        }

        public void GameClear()
        {
            Time.timeScale = 0;
            inGameCanvas.transform.Find("GameClearPanel").gameObject.SetActive(true);
        }

        public void GetPassiveSkill(int playerNum)
        {
            inGameCanvas.GetComponent<InGameCanvasManager>().DisplaySkillSelection(Skill.SkillType.Passive, playerNum);
        }

        public void GetActiveSkill(int playerNum)
        {
            inGameCanvas.GetComponent<InGameCanvasManager>().DisplaySkillSelection(Skill.SkillType.Active, playerNum);
        }
    }
}