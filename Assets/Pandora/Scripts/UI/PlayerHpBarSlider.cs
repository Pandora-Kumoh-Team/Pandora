using System;
using Pandora.Scripts.Player;
using Pandora.Scripts.System;
using Pandora.Scripts.System.Event;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Pandora.Scripts.UI
{
    public class PlayerHpBarSlider : MonoBehaviour, IEventListener
    {
        public int playerId = -1;
        private Slider _slider;
        
        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }
        
        private void Start()
        {
            if(playerId == -1)
                Debug.LogError("PlayerId not set in PlayerHpBarSlider");
            EventManager.Instance.AddListener(PandoraEventType.PlayerHealthChanged, this);
        }
        
        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener(PandoraEventType.PlayerHealthChanged, this);
        }
        
        public void OnEvent(PandoraEventType pandoraEventType, Component sender, object param = null)
        {
            if (pandoraEventType != PandoraEventType.PlayerHealthChanged || param == null) return;
            var paramData = (PlayerHealthChangedParam) param;
            if (paramData.PlayerCharacterId != playerId) return;
            // 올림 처리
            _slider.value = Mathf.CeilToInt(paramData.CurrentHealth);
            _slider.maxValue = paramData.MaxHealth;
        }
    }
}