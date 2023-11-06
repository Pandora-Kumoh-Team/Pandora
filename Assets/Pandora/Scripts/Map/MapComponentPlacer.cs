using System;
using System.Collections.Generic;
using Pandora.Scripts.Enemy;
using Pandora.Scripts.Player.Controller;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pandora.Scripts.Map
{
    public class MapComponentPlacer : MonoBehaviour
    {
        public DivideSpace divideSpace;
        public GameObject bossPrefab;
        
        public List<GameObject> mobPrefabs;
        private void Start()
        {
            if(divideSpace == null)
                divideSpace = FindObjectOfType<DivideSpace>();
            if(bossPrefab == null)
                Debug.LogError("스테이지 보스 프리팹이 없습니다.");
        }

        public void InitMapComponents()
        {
            var playerStartRoom = divideSpace.spaceList[0];
            PlayerManager.Instance.transform.position =
                new Vector2(playerStartRoom.Center().x, playerStartRoom.Center().y);
            
            // enemy parent
            var enemyParent = new GameObject("Enemies");
            
            var bossRoom = divideSpace.spaceList[^1];
            var boosRomCenter = new Vector2(bossRoom.Center().x, bossRoom.Center().y);
            Instantiate(bossPrefab, boosRomCenter, Quaternion.identity, enemyParent.transform);
            
            // TODO : 맵 난이도에 따른 잡몹 배치 변화
            
            foreach (var space in divideSpace.spaceList)
            {
                if (space == playerStartRoom || space == bossRoom)
                    continue;
                var mobCount = Random.Range(1, 4);
                for (int i = 0; i < mobCount; i++)
                {
                    var mobIndex = Random.Range(0, mobPrefabs.Count);
                    var mobPosition = new Vector2(space.Center().x, space.Center().y);
                    Instantiate(mobPrefabs[mobIndex], mobPosition, Quaternion.identity, enemyParent.transform);
                }
            }
            
        }
    }
}