using System.Collections.Generic;
using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.Player.Skill;
using Pandora.Scripts.System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pandora.Scripts.UI
{
    public class InGameCanvasManager : MonoBehaviour
    {
        public GameObject mob;
        public void OnPause()
        {
            var pausePanel = transform.Find("PauseMenu").gameObject;
            var isMenuActive = pausePanel.activeSelf;
            pausePanel.SetActive(!isMenuActive);
            Time.timeScale = isMenuActive ? 1 : 0;
        }
        
        public void OnPause(bool isPaused)
        {
            var pausePanel = transform.Find("PauseMenu").gameObject;
            pausePanel.SetActive(isPaused);
            Time.timeScale = isPaused ? 0 : 1;
        }

        public void OnPassiveSkillList()
        {
            var skillPanel = transform.Find("SkillsList").gameObject;
            var isSkillPanelActive = skillPanel.activeSelf;
            skillPanel.SetActive(!isSkillPanelActive);
        }
        
        /// <summary>
        /// 스킬 보상 선택 패널을 엽니다.
        /// </summary>
        public void DisplaySkillSelection(Skill.SkillType skillType, int playerNum)
        {
            var skillSelection = transform.Find("SkillSelection").gameObject;
            skillSelection.SetActive(true);
            Time.timeScale = 0;

            var skillList = skillType == Skill.SkillType.Active
                ? SkillManager.Instance.GetRandomActiveSkills(playerNum, 3)
                : SkillManager.Instance.GetRandomPassiveSkills(playerNum, 3);
            var skillObjectList = new List<GameObject>
            {
                skillSelection.transform.Find("SkillWindow").Find("Skill1").gameObject,
                skillSelection.transform.Find("SkillWindow").Find("Skill2").gameObject,
                skillSelection.transform.Find("SkillWindow").Find("Skill3").gameObject
            };
            for (var i = 0; i < 3; i++)
            {
                var skillObject = skillObjectList[i];
                if(i < skillList.Count)
                {
                    var skill = skillList[i];
                    skillObject.SetActive(true);
                    skillObject.GetComponent<SkillSelectUi>().Init(skill, playerNum);
                }
                else
                {
                    skillObject.SetActive(false);
                }
            }
        }
        
        public void CloseConfirm()
        {
            var closeConfirm = transform.Find("SkillSelection").Find("CloseConfirm").gameObject;
            closeConfirm.SetActive(true);
        }
        
        public void CancelSkillSelection()
        {
            var closeConfirm = transform.Find("SkillSelection").Find("CloseConfirm").gameObject;
            closeConfirm.SetActive(false);
            var skillSelection = transform.Find("SkillSelection").gameObject;
            skillSelection.SetActive(false);
            Time.timeScale = 1;
        }
        
        public void ReStart()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }
        
        public void ExitGame()
        {
            GameManager.ExitGame();
        }

        // TODO : 중간 시연용 이후 삭제해야함
        public void GoToBoss()
        {
            var players = PlayerManager.Instance.GetPlayers();
            var bossPos = GameObject.FindObjectOfType<FirstBossController>().gameObject.transform.position;
            players[0].transform.position = bossPos;
            players[1].transform.position = bossPos;
            players[0].GetComponent<PlayerController>()._playerStat.AttackPower *= 6;
            players[1].GetComponent<PlayerController>()._playerStat.AttackPower *= 1;
        }
        // TODO : 중간 시연용 이후 삭제해야함
        public void SummonManyMob()
        {
            // 몹 10마리 플레이어 근처 원형으로 소환
            var players = PlayerManager.Instance.GetPlayers();
            var playerPos = players[0].transform.position;
            for (int i = 0; i < 10; i++)
            {
                var pos = playerPos + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
                Instantiate(mob, pos, Quaternion.identity);
            }
        }
    }
}