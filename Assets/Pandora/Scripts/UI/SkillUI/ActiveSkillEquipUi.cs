using Pandora.Scripts.Player.Controller;
using UnityEngine;

namespace Pandora.Scripts.UI.SkillUI
{
    public class ActiveSkillEquipUi : MonoBehaviour
    {
        private static readonly int IsMelee = Animator.StringToHash("IsMelee");

        public void Init(int playerNum, GameObject changeSkill)
        {
            var player = PlayerManager.Instance.GetPlayer(playerNum);
            var playerController = player.GetComponent<PlayerController>();
            var skillList = playerController.GetActiveSkills();
            transform.Find("Skill1").GetComponent<SkillReplaceUi>().Init(skillList[0], playerNum, changeSkill);
            transform.Find("Skill2").GetComponent<SkillReplaceUi>().Init(skillList[1], playerNum, changeSkill);
            transform.Find("Skill3").GetComponent<SkillReplaceUi>().Init(skillList[2], playerNum, changeSkill);

            var playerPreview = transform.Find("PlayerPreview");
            var previewAnimator = playerPreview.Find("Image").GetComponent<Animator>();
            previewAnimator.SetBool(IsMelee, player.GetComponent<MeleePlayerController>() != null);
        }
    }
}