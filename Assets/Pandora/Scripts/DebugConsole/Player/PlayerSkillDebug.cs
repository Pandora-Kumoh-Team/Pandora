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
        
        private void OnCollisionEnter2D(Collision2D col)
        {
            if(getRandomSKill)
            {
                var playerId = col.gameObject.GetComponent<PlayerController>().playerCharacterId;
                GameManager.Instance.inGameCanvas.GetComponent<InGameCanvasManager>()
                    .DisplaySkillSelection(Skill.SkillType.Active, playerId);
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