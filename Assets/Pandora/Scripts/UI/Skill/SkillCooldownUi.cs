using System;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.Player.Skill;
using Pandora.Scripts.System.Event;
using UnityEngine;
using UnityEngine.UI;

namespace Pandora.Scripts.UI
{
    public class SkillCooldownUi : MonoBehaviour, IEventListener
    {
        public int playerNumber;
        public int skillIndex;
        
        private PlayerController _playerController;
        private GameObject _skillObject;
        private Image _cooldownImage;
        private Image skillIcon;

        public void Awake()
        {
            _cooldownImage = transform.Find("CooldownImage").GetComponent<Image>();
            skillIcon = transform.Find("SkillIcon").GetComponent<Image>();
        }

        private void Start()
        {
            _playerController = PlayerManager.Instance.GetPlayer(playerNumber).GetComponent<PlayerController>();
            _skillObject = _playerController.activeSkills[skillIndex];
            skillIcon.sprite = _skillObject.GetComponent<Skill>().icon;
            EventManager.Instance.AddListener(PandoraEventType.PlayerSkillChanged, this);
        }
        
        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener(PandoraEventType.PlayerSkillChanged, this);
        }

        public void Update()
        {
            _cooldownImage.fillAmount = _playerController.skillCoolTimes[skillIndex] /
                                        _playerController.activeSkills[skillIndex].GetComponent<Skill>().coolTime;
        }

        public void OnEvent(PandoraEventType pandoraEventType, Component sender, object param = null)
        {
            if(pandoraEventType != PandoraEventType.PlayerSkillChanged) return;
            var playerSkillChangedParam = (PlayerSkillChangedParam)param;
            if(playerSkillChangedParam.playerNumber != playerNumber) return;
            if(playerSkillChangedParam.skillIndex != skillIndex) return;
            skillIcon.sprite = playerSkillChangedParam.Skill.icon;
        }
    }
}