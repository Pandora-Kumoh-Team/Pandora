using System;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.Player.Skill;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pandora.Scripts.UI
{
    public class SkillReplaceUi : SkillInfoUi
    {
        public int skillIndex;
        public GameObject changeSkill;
        private string _key;
        
        public void SelectSkill()
        {
            PlayerManager.Instance.GetPlayer(PlayerNum).GetComponent<PlayerController>()
                .SetActiveSkill(changeSkill, skillIndex);
            Time.timeScale = 1;
        }

        public void Init(GameObject infoSkill, int playerNum, GameObject changeSkill)
        {
            base.Init(infoSkill, playerNum);
            this.changeSkill = changeSkill;
        }
    }
}