using Pandora.Scripts.System.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pandora.Scripts.UI
{
    public class BossHPBarSlider : MonoBehaviour, IEventListener
    {
        private Slider _slider;
        // Start is called before the first frame update
        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        void Start()//시작하면
        {
            EventManager.Instance.AddListener(PandoraEventType.BossHealthChanged, this);
        }
        private void OnDestroy()//제거되었을 때
        {
            EventManager.Instance.RemoveListener(PandoraEventType.BossHealthChanged, this);
        }

        public void OnEvent(PandoraEventType pandoraEventType, Component sender, object param = null)
        {
            if (pandoraEventType != PandoraEventType.BossHealthChanged || param == null) return;
            var paramData = (BossHealthChangedParam)param;
            _slider.value = paramData.CurrentHealth;
            _slider.maxValue = paramData.MaxHealth;
        }
    }
}
