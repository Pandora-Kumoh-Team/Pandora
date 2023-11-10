using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.Player.Skill.SkillDetail;
using UnityEngine;

namespace Pandora.Scripts.DebugConsole.Player
{
    public class PlayerSkillDebug : MonoBehaviour
    {
        public void SetSkill()
        {
            Debug.Log("SetSkill");
            var go = new GameObject();
            go.AddComponent<SkillDash>();
            PlayerManager.Instance.GetPlayers()[0].GetComponent<PlayerController>()
                .SetSkill1(go.GetComponent<SkillDash>());
        }
    }
}