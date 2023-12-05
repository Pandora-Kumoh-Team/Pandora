using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pandora.Scripts.System.Event
{
    /// <summary>
    /// 이벤트 리스트
    /// </summary>
    public enum PandoraEventType
    {
        PlayerHealthChanged,
        PlayerAttackEnemy,
        PlayerSkillChanged,
        
        BossHealthChanged,
        
        MapGenerateComplete,
    }
    
    /// <summary>
    /// 이벤트 관리자로 Singleton으로 구현되어있다.
    /// EventType enum을 통해 이벤트를 구분하고, 이벤트를 받을 함수를 등록하고, 이벤트를 발생시킬 수 있다.
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance { get { return _instance; } }
        private static EventManager _instance;
        
        private Dictionary<PandoraEventType, List<IEventListener>> _eventListeners =
            new Dictionary<PandoraEventType, List<IEventListener>>();

        private void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(gameObject);
            }
            else {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                // 씬 전환시 파괴된 오브젝트 참조 제거
                SceneManager.sceneLoaded += RemoveRedundancies;
            }
        }
        
        public void AddListener(PandoraEventType pandoraEventType, IEventListener listener)
        {
            if (!_eventListeners.ContainsKey(pandoraEventType))
            {
                _eventListeners.Add(pandoraEventType, new List<IEventListener>());
            }
            _eventListeners[pandoraEventType].Add(listener);
        }
        
        public void RemoveListener(PandoraEventType pandoraEventType, IEventListener listener)
        {
            if (!_eventListeners.ContainsKey(pandoraEventType))
            {
                return;
            }
            _eventListeners[pandoraEventType].Remove(listener);
        }
        
        public void TriggerEvent(PandoraEventType pandoraEventType, object data = null)
        {
            if (!_eventListeners.ContainsKey(pandoraEventType))
            {
                return;
            }
            foreach (var listener in _eventListeners[pandoraEventType])
            {
                listener.OnEvent(pandoraEventType,this, data);
            }
        }
        
        /// <summary>
        /// 파괴된 오브젝트의 참조를 제거한다.
        /// </summary>
        private void RemoveRedundancies(Scene arg0, LoadSceneMode loadSceneMode)
        {
            var tmpListeners = new Dictionary<PandoraEventType, List<IEventListener>>();

            foreach (var listener in _eventListeners)
            {
                for (var i = listener.Value.Count - 1; i >= 0; i--)
                {
                    if (listener.Value[i] == null)
                    {
                        listener.Value.RemoveAt(i);
                    }
                }
                if (listener.Value.Count > 0)
                {
                    tmpListeners.Add(listener.Key, listener.Value);
                }
            }
            _eventListeners = tmpListeners;
        }
    }
}