using System;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.Player.Skill;
using Pandora.Scripts.Player.Skill.SkillDetail;
using Pandora.Scripts.System;
using Pandora.Scripts.System.Event;
using Pandora.Scripts.UI;
using UnityEngine;

namespace Pandora.Scripts.DebugConsole.Player
{
    public class PlayerSkillDebug : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D col)
        {
            var playerId = col.gameObject.GetComponent<PlayerController>().playerCharacterId;
            GameManager.Instance.inGameCanvas.GetComponent<InGameCanvasManager>()
                .DisplaySkillSelection(Skill.SkillType.Active, playerId);
            Destroy(gameObject);
        }
    }
}