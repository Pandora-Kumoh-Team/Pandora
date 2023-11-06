using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pandora.Scripts.UI
{
    public class InGameCanvasManager : MonoBehaviour
    {
        public void OnPause()
        {
            var pausePanel = transform.Find("PauseMenu").gameObject;
            var isMenuActive = pausePanel.activeSelf;
            pausePanel.SetActive(!isMenuActive);
            Time.timeScale = isMenuActive ? 1 : 0;
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
    }
}