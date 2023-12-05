using System;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.Player.Skill;
using Pandora.Scripts.System.Event;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pandora.Scripts.UI.SkillUI
{
    public partial class SkillCooldownUi : MonoBehaviour, IEventListener, IPointerEnterHandler, IPointerExitHandler
    {
        public int playerNumber;
        public int skillIndex;
        
        private PlayerController _playerController;
        private GameObject _skillObject;
        private Skill _skill;
        private Image _cooldownImage;
        private Image skillIcon;
        
        [SerializeField] private SkillTooltipWindow skillTooltipWindow;
        [SerializeField] private RectTransform rectTransform;


        public void Awake()
        {
            _cooldownImage = transform.Find("CooldownImage").GetComponent<Image>();
            skillIcon = transform.Find("SkillIcon").GetComponent<Image>();
        }

        private void Start()
        {
            _playerController = PlayerManager.Instance.GetPlayer(playerNumber).GetComponent<PlayerController>();
            _skillObject = _playerController.activeSkills[skillIndex];
            _skill = _skillObject.GetComponent<Skill>();
            skillIcon.sprite = _skill.icon;
            EventManager.Instance.AddListener(PandoraEventType.PlayerSkillChanged, this);
        }
        
        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener(PandoraEventType.PlayerSkillChanged, this);
        }

        public void Update()
        {
            // index out check
            try
            {
                _cooldownImage.fillAmount = _playerController.skillCoolTimes[skillIndex] /
                                            _playerController.activeSkills[skillIndex].GetComponent<Skill>().coolTime;
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public void OnEvent(PandoraEventType pandoraEventType, Component sender, object param = null)
        {
            if(pandoraEventType != PandoraEventType.PlayerSkillChanged) return;
            var playerSkillChangedParam = (PlayerSkillChangedParam)param;
            if(playerSkillChangedParam.playerNumber != playerNumber) return;
            if(playerSkillChangedParam.skillIndex != skillIndex) return;
            _skill = playerSkillChangedParam.Skill;
            skillIcon.sprite = _skill.icon;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            skillTooltipWindow.ShowTooltip(_skill.name, _skill.description,
                SkillManager.SKillGradeToColor(_skill.grade), rectTransform.position.x);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            skillTooltipWindow.HideTooltip();
        }
    }
}