using System;
using System.Collections.Generic;
using Cinemachine;
using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pandora.Scripts.NewDungeon.Rooms
{
    public class RoomEmpty : Room
    {
        private bool _isSpawned;
        private List<GameObject> _enemies = new List<GameObject>();

        private void Update()
        {
            if (_isSpawned && !isClear)
            {
                foreach (var enemy in _enemies)
                {
                    if(enemy.activeSelf == true)
                        return;
                }
                OnClearRoom();
            }
        }

        public override void OnPlayerEnter(GameObject playerObject)
        {
            base.OnPlayerEnter(playerObject);
        
            if (_isSpawned) return;
            _isSpawned = true;
            
            if(!isClear)
            {
                // move other player to this room
                PlayerManager.Instance.GetOtherPlayer(playerObject).transform.position = playerObject.transform.position;
            
                // close all doors
                CloseAllDoors();
                
                // spawn enemies
                GameObject enemyPrefab;
                var leftDifficulty = StageController.Instance.currentStageInfo.difficulty;
                while(true)
                {
                    enemyPrefab = StageController.Instance.GetRandomMob(leftDifficulty);
                    if (enemyPrefab == null) break;
                    var spawnPoint = GetRandomSpawnPoint();
                    var go = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
                    _enemies.Add(go);
                    leftDifficulty -= enemyPrefab.GetComponent<EnemyController>().difficulty;
                }
            }
        }

        private Vector3 GetRandomSpawnPoint()
        {
            // get 4 corners of the room
            var isLeft = Random.Range(0, 2) == 0 ? -0.8f : 0.8f;
            isLeft += Random.Range(-0.1f, 0.1f);
            var isUp = Random.Range(0, 2) == 0 ? -0.8f : 0.8f;
            isUp += Random.Range(-0.1f, 0.1f);
            return transform.position + new Vector3(isLeft * Width / 2, isUp * Height / 2, 0);
        }
    }
}