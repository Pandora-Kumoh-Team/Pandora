using System;
using Pandora.Scripts.NewDungeon;
using Pandora.Scripts.Player.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pandora.Scripts.UI
{
    public class MainMenuController : MonoBehaviour
    {
        GameObject _nowUIElement;

        private void Awake()
        {
            var stageController = FindObjectOfType<StageController>();
            if (stageController != null)
            {
                Destroy(stageController.gameObject);
            }
            var playerManager = FindObjectOfType<PlayerManager>();
            if (playerManager != null)
            {
                Destroy(playerManager.gameObject);
            }

            var roomgenerater = FindObjectOfType<RoomPositionsGenerator>();
            if (roomgenerater != null)
            {
                RoomPositionsGenerator.stage = 0;
                RoomPositionsGenerator.RoomPositions.Clear();
            }
        }

        public void StageStart()
        {
            // TODO : 플레이어 영구 스텟 반영하여 destroy로 생성
            // 현재는 stage1 씬에 플레이어 오브젝트가 추가되어 있음
            SceneManager.LoadScene("NewStage1");
        }

        public void Credit()
        {
            // TODO
            // SceneManager.LoadScene("Credit");
        }
        
        public void GameExit()
        {
            Debug.Log("Game Exit");
            Application.Quit();
        }

        public void OnPause()
        {
            transform.Find("MainMenuWindow").gameObject.SetActive(true);
            _nowUIElement.SetActive(false);
        }
        
        public void ResisterUIElement(GameObject uiElement)
        {
            _nowUIElement = uiElement;
        }
    }
}