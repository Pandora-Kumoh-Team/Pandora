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
                OnClearRoom();
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
                CloseAllDoors();
                
                // spawn enemies
                var enemyPrefab = StageController.Instance.currentStageInfo.boss;
                _boss = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            }
        }

        public override void OnClearRoom()
        {
            base.OnClearRoom();
            // 마지막 스테이지 일시
            if(StageController.Instance.currentStage == 2)
            {
                GameManager.Instance.GameClear();
                return;
            }
            // 액티브 스킬 보상
            transform.Find("SkillGiver").gameObject.SetActive(true);
            
            transform.Find("NextFloor").gameObject.SetActive(true);
        }
    }
}