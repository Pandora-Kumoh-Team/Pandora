using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.System;
using UnityEngine;

namespace Pandora.Scripts.UI.SkillUI
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
            var ingameCanvas = GameManager.Instance.inGameCanvas.GetComponent<InGameCanvasManager>();
            ingameCanvas.RemoveUIElement(transform.parent.gameObject);
            ingameCanvas.RemoveUIElement(transform.parent.parent.gameObject);
            ingameCanvas.cantPopUpByPauseElement.Remove(transform.parent.parent.gameObject);
        }

        public void Init(GameObject infoSkill, int playerNum, GameObject changeSkill)
        {
            base.Init(infoSkill, playerNum);
            this.changeSkill = changeSkill;
        }
    }
}