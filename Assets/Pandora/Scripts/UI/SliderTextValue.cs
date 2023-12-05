using Pandora.Scripts.System.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DarkPixelRPGUI.Scripts.UI
{
    public class SliderTextValue : MonoBehaviour, IEventListener
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text text;

        private void Start()
        {
            EventManager.Instance.AddListener(PandoraEventType.PlayerHealthChanged, this);
            if (slider)
            {
                slider.onValueChanged.AddListener(UpdateText);
            }
        }

        private void UpdateText(float value)
        {
            if (!text) return;
            text.text = $"{value:0}/{slider.maxValue:0}";
        }

        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener(PandoraEventType.PlayerHealthChanged, this);
            if (slider)
            {
                slider.onValueChanged.RemoveListener(UpdateText);
            }
        }

        public void OnEvent(PandoraEventType pandoraEventType, Component sender, object param = null)
        {
            if(pandoraEventType == PandoraEventType.PlayerHealthChanged)
            {
                UpdateText(slider.value);
            }
        }
    }
}