using System;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pandora.Scripts.NewDungeon.Rooms
{
    public class NextFloor : OnEnableNearPlayerCheckObject
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Player")) return;
            if (!col.gameObject.GetComponent<PlayerController>().isControlByPlayer) return;
            if (IsPlayerStillNearAfterEnable) return;
            StageController.Instance.currentStage++;
                
            DontDestroyOnLoad(PlayerManager.Instance);
            var players = PlayerManager.Instance.GetPlayers();
            foreach (var player in players)
            {
                player.transform.position = new Vector3(0, 0, 0);
            }
            // reload this scene again
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}