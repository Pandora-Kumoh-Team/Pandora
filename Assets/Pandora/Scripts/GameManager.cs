using UnityEngine;

namespace Pandora.Scripts
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
        }
        
        // 숫차로 출력되는 데메지 이펙트 프리팹
        // Insistate 후 Init() 호출하여 사용
        public GameObject damageEffect;
    }
}