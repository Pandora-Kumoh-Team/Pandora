using System.Collections;
using System.Collections.Generic;
using Pandora.Scripts.DebugConsole;
using Pandora.Scripts.Enemy;
using Pandora.Scripts.NewDungeon.Rooms;
using Pandora.Scripts.Player;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.Player.Skill;
using Pandora.Scripts.System;
using Pandora.Scripts.System.Event;
using Pandora.Scripts.UI.SkillUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.UI
{
    public class InGameCanvasManager : MonoBehaviour, IEventListener
    {
        public GameObject mob;
        public List<GameObject> displayedUIElementStack;
        public List<GameObject> cantPopUpByPauseElement;

        private void Awake()
        {
            transform.Find("LoadingScreen").gameObject.SetActive(true);
            StartCoroutine(LateAwake());
        }

        private IEnumerator LateAwake()
        {
            yield return null;
            EventManager.Instance.AddListener(PandoraEventType.MapGenerateComplete, this);
        }

        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener(PandoraEventType.MapGenerateComplete, this);
        }

        public void OnEvent(PandoraEventType pandoraEventType, Component sender, object param = null)
        {
            if (pandoraEventType == PandoraEventType.MapGenerateComplete)
            {
                transform.Find("LoadingScreen").gameObject.SetActive(false);
            }
        }
        
        public void OnPause()
        {
            var pausePanel = transform.Find("PauseMenu").gameObject;
            displayedUIElementStack ??= new List<GameObject>();
            if (displayedUIElementStack.Count == 0)
                PushUIElement(pausePanel);
            else if (!cantPopUpByPauseElement.Contains(displayedUIElementStack[^1]))
                PopUIElement();
        }

        public void OnPassiveSkillList()
        {
            var skillPanel = transform.Find("SkillsList").gameObject;
            if(displayedUIElementStack.Contains(skillPanel))
                RemoveUIElement(skillPanel);
            else
                PushUIElement(skillPanel);
        }
        
        /// <summary>
        /// 스킬 보상 선택 패널을 엽니다.
        /// </summary>
        public void DisplaySkillSelection(Skill.SkillType skillType, int playerNum)
        {
            var skillSelection = transform.Find("SkillSelection").gameObject;
            PushUIElement(skillSelection);
            cantPopUpByPauseElement.Add(skillSelection);

            var skillList = SkillManager.Instance.GetRandomSkills(playerNum, skillType, 3);
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
            PushUIElement(closeConfirm);
        }
        
        public void CancelSkillSelection()
        {
            var closeConfirm = transform.Find("SkillSelection").Find("CloseConfirm").gameObject;
            RemoveUIElement(closeConfirm);
            var skillSelection = transform.Find("SkillSelection").gameObject;
            RemoveUIElement(skillSelection);
        }
        
        public void ReStart()
        {
            RemoveAllUIElement();
            SceneManager.LoadScene("MainMenu");
        }
        
        public void ExitGame()
        {
            GameManager.ExitGame();
        }

        // TODO : 중간 시연용 이후 삭제해야함
        public void GoToBoss()
        {
            var go = FindObjectOfType<DebugGoToBossPos>();
            var ps = PlayerManager.Instance.GetPlayers();
            foreach (var p in ps)
            {
                p.transform.position = go.transform.position;
            }
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
        // TODO : 최종 시연용
        public void PlayerStronger()
        {
            var players = PlayerManager.Instance.GetPlayers();
            for (var i = 0; i < players.Length; i++)
            {
                var player = players[i];
                var playerStatus = player.GetComponent<PlayerController>().playerCurrentStat;
                var pcs = playerStatus.playerStat;
                pcs.maxHealth += 10;
                pcs.baseDamage += 1;
                pcs.attackPower *= 0.1f;
                pcs.defencePower *= 0.1f;
                pcs.speed += 1;
                pcs.attackRange *= 1.1f;
                pcs.attackSpeed += 0.1f;
                pcs.criticalChance *= 0.1f;
                pcs.criticalDamageTimes += 0.1f;
                pcs.dodgeChance *= 1.1f;
                pcs.nonControlHpRecovery += 1;
                playerStatus.NowHealth = pcs.maxHealth;
                var param = new PlayerHealthChangedParam(pcs.maxHealth, pcs.maxHealth, i);
                EventManager.Instance.TriggerEvent(PandoraEventType.PlayerHealthChanged, param);
            }
        }
        
        public void PushUIElement(GameObject uiElement)
        {
            if (displayedUIElementStack == null)
            {
                displayedUIElementStack = new List<GameObject>();
            }
            displayedUIElementStack.Add(uiElement);
            uiElement.SetActive(true);
            Time.timeScale = 0;
        }
        
        public GameObject PopUIElement()
        {
            if (displayedUIElementStack == null || displayedUIElementStack.Count == 0)
            {
                return null;
            }
            var uiElement = displayedUIElementStack[displayedUIElementStack.Count - 1];
            displayedUIElementStack.RemoveAt(displayedUIElementStack.Count - 1);
            uiElement.SetActive(false);
            if (displayedUIElementStack.Count == 0)
            {
                Time.timeScale = 1;
            }
            return uiElement;
        }
        
        public void RemoveUIElement(GameObject uiElement)
        {
            if (displayedUIElementStack == null || displayedUIElementStack.Count == 0)
            {
                return;
            }
            displayedUIElementStack.Remove(uiElement);
            uiElement.SetActive(false);
            if (displayedUIElementStack.Count == 0)
            {
                Time.timeScale = 1;
            }
        }
        
        public void RemoveAllUIElement()
        {
            if (displayedUIElementStack == null || displayedUIElementStack.Count == 0)
            {
                return;
            }
            foreach (var uiElement in displayedUIElementStack)
            {
                uiElement.SetActive(false);
            }
            displayedUIElementStack.Clear();
            Time.timeScale = 1;
        }
    }
}