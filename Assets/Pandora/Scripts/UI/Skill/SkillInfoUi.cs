using System;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.Player.Skill;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pandora.Scripts.UI
{
    public class SkillInfoUi : MonoBehaviour
    {
        protected Skill Skill;
        protected int PlayerNum;
        public Image skillIcon;
        public TextMeshProUGUI skillName;
        public TextMeshProUGUI skillDescription;

        protected void Awake()
        {
            skillIcon = transform.Find("SkillIcon").GetComponent<Image>();
            skillName = transform.Find("SkillName").GetComponent<TextMeshProUGUI>();
            skillDescription = transform.Find("SkillDescription").GetComponent<TextMeshProUGUI>();
        }

        public void Init(Skill skill, int playerNum)
        {
            Skill = skill;
            PlayerNum = playerNum;
            skillIcon.sprite = skill.icon;
            skillName.text = skill.name;
            skillDescription.text = skill.description;
        }
    }
}