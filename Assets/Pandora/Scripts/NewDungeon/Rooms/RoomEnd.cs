using System;
using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.System;
using Pandora.Scripts.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pandora.Scripts.NewDungeon.Rooms
{
    public class RoomEnd : Room
    {
        private bool _isSpawned;
        private GameObject _boss;

        private void Update()
        {
            if (_isSpawned && _boss == null && !isClear)
            {
                isClear = true;
                var playerNum = Random.Range(0, 2);
                GameManager.Instance.GetActiveSkill(playerNum);
            }
        }

        public override void OnPlayerEnter(GameObject playerObject)
        {
            base.OnPlayerEnter(playerObject);

            if (_isSpawned) return;
            
            if(!isClear)
            {
                _isSpawned = true;
                // move other player to this room
                PlayerManager.Instance.GetOtherPlayer(playerObject).transform.position = playerObject.transform.position;
            
                // close all doors
                foreach(Door door in doors)
                {
                    door.CloseDoor();
                }
                
                // spawn enemies
                var enemyPrefab = StageController.Instance.currentStageInfo.boss;
                _boss = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}