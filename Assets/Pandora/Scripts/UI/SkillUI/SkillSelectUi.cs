using System;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.Player.Skill;
using Pandora.Scripts.System;

namespace Pandora.Scripts.UI.SkillUI
{
    public class SkillSelectUi : SkillInfoUi
    {
        public void SelectSkill()
        {
            var skillComponent = InfoSkill.GetComponent<Skill>();
            var ingameCanvas = GameManager.Instance.inGameCanvas.GetComponent<InGameCanvasManager>();
            if(skillComponent.type == Skill.SkillType.Passive)
            {
                PlayerManager.Instance.GetPlayer(PlayerNum).GetComponent<PlayerController>().AddPassiveSkill(InfoSkill);
                ingameCanvas.RemoveUIElement(transform.parent.parent.gameObject);
                ingameCanvas.cantPopUpByPauseElement.Remove(transform.parent.parent.gameObject);
            }
            else if (skillComponent.type == Skill.SkillType.Active)
            {
                var activeSkillEquip = GameManager.Instance.inGameCanvas.transform.
                    Find("SkillSelection").Find("ActiveSkillEquip").gameObject;
                ingameCanvas.PushUIElement(activeSkillEquip);
                activeSkillEquip.GetComponent<ActiveSkillEquipUi>().Init(PlayerNum, InfoSkill);
            }
            else
                throw new NotImplementedException("This SkillType Selecting not implemented");
        }
    }
}