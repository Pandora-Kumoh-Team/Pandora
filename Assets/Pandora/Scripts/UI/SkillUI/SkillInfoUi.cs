using System;
using Pandora.Scripts.Player.Skill;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pandora.Scripts.UI.SkillUI
{
    public class SkillInfoUi : MonoBehaviour
    {
        protected GameObject InfoSkill;
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

        public void Init(GameObject skill, int playerNum)
        {
            InfoSkill = skill;
            PlayerNum = playerNum;
            var skillComponent = InfoSkill.GetComponent<Skill>();
            skillIcon.sprite = skillComponent.icon;
            skillName.text = skillComponent.name;
            skillName.color = SkillManager.SKillGradeToColor(skillComponent.grade);
            skillDescription.text = skillComponent.description;
        }
    }
}