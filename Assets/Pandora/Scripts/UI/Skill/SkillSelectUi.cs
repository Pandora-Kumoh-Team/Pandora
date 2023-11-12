using System;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.Player.Skill;
using Pandora.Scripts.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pandora.Scripts.UI
{
    public class SkillSelectUi : SkillInfoUi
    {
        public void SelectSkill()
        {
            if(Skill.type == Skill.SkillType.Passive)
            {
                PlayerManager.Instance.GetPlayer(PlayerNum).GetComponent<PlayerController>().AddPassiveSkill(Skill);
                transform.parent.parent.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
            else if (Skill.type == Skill.SkillType.Active)
            {
                var activeSkillEquip = GameManager.Instance.inGameCanvas.transform.
                    Find("SkillSelection").Find("ActiveSkillEquip").gameObject;
                activeSkillEquip.SetActive(true);
                activeSkillEquip.GetComponent<ActiveSkillEquipUi>().Init(PlayerNum, Skill);
                
                transform.parent.gameObject.SetActive(false);
            }
            else
                throw new NotImplementedException("This SkillType Selecting not implemented");
        }
    }
}