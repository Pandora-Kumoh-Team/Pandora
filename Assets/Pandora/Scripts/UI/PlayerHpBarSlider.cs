using System;
using Pandora.Scripts.System;
using UnityEngine;
using UnityEngine.UI;

namespace Pandora.Scripts.UI
{
    public class PlayerHpBarSlider : MonoBehaviour, IEventListener
    {
        private Slider _slider;
        
        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }
        
        private void Start()
        {
            EventManager.Instance.AddListener(PandoraEventType.PlayerHealthChanged, this);
        }
        
        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener(PandoraEventType.PlayerHealthChanged, this);
        }
        
        public void OnEvent(PandoraEventType pandoraEventType, Component sender, object param = null)
        {
            if (pandoraEventType == PandoraEventType.PlayerHealthChanged)
            {
                if (param != null) _slider.value = (float)param;
            }
        }
    }
}