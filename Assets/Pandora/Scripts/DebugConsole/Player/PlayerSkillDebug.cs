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
        public bool isActiveSkill;
        [Header("보상 시스템에 따른 랜덤 스킬 획득하기")]
        public bool getRandomSKill = true;
        [Header("인스펙터에 설정한 스킬 획득하기")]
        public bool getInspectorSkill = false;
        public GameObject setSkillPrefab;
        
        private bool IsPlayerNear()
        {
            var players = PlayerManager.Instance.GetPlayers();
            foreach (var player in players)
            {
                if(Vector2.Distance(transform.position, player.transform.position) < 1.5f)
                    return true;
            }
            return false;
        }
        
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (!col.gameObject.CompareTag("Player")) return;
            if (!col.gameObject.GetComponent<PlayerController>().isControlByPlayer) return;
            
            if(getRandomSKill)
            {
                var playerId = col.gameObject.GetComponent<PlayerController>().playerNumber;
                var skillType = isActiveSkill? Skill.SkillType.Active : Skill.SkillType.Passive;
                GameManager.Instance.inGameCanvas.GetComponent<InGameCanvasManager>()
                    .DisplaySkillSelection(skillType, playerId);
                Destroy(gameObject);
            }
            else if(getInspectorSkill)
            {
                if(isActiveSkill)
                    col.gameObject.GetComponent<PlayerController>().SetActiveSkill(setSkillPrefab, 0);
                else
                    col.gameObject.GetComponent<PlayerController>().AddPassiveSkill(setSkillPrefab);
            }
        }
    }
}