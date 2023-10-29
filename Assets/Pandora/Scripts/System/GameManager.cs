using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pandora.Scripts.System
{
    public class GameManager : MonoBehaviour
    {
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
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // 캔버스 찾기
            inGameCanvas = GameObject.Find("InGameCanvas").GetComponent<Canvas>();
        }
        
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}