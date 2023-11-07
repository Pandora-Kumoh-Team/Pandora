using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
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