using System.Collections.Generic;
using Pandora.Scripts.DebugConsole;
using UnityEngine;

namespace Pandora.Scripts.NewDungeon
{
    public class DungeonGenerator : MonoBehaviour
    {
        public DungeonGenerationData dungeonGenerationData;
        private List<Vector2Int> dungeonRoomPositions;
        private void Start()
        {
            dungeonRoomPositions = RoomPositionsGenerator.GenerateRoomPositions(dungeonGenerationData);
            SpawnRooms(dungeonRoomPositions);
        }

        private void SpawnRooms(IEnumerable<Vector2Int> roomPositions)
        {
            RoomController.Instance.EnqueueRoomToGeneration("Start", 0, 0);
            int roomIndex = 0;
            int shopIndex = Random.Range(1, 10);
            int bossIndex = dungeonRoomPositions.Count - 1;
            while (dungeonRoomPositions[bossIndex] == Vector2Int.zero)
            {
                bossIndex--;
                if (bossIndex >= -1) continue;
                RoomPositionsGenerator.stage = 0;
                RoomPositionsGenerator.RoomPositions.Clear();
                dungeonRoomPositions = RoomPositionsGenerator.GenerateRoomPositions(dungeonGenerationData);
                SpawnRooms(roomPositions);
                return;
            }
            foreach (var roomPosition in roomPositions)
            {
                if (roomPosition == dungeonRoomPositions[bossIndex] && roomPosition == Vector2.zero)
                {
                    Debug.LogWarning("Boss room is zero (" + roomPosition.x + ", " + roomPosition.y + ")");
                    if(DebugBossMapGenerateTest._instance != null)
                    {
                        DebugBossMapGenerateTest._instance.AddBossDenied("Boss room is zero (" + roomPosition.x + ", " + roomPosition.y + ")");
                    }
                }
                if(bossIndex == roomIndex)
                {
                    RoomController.Instance.EnqueueRoomToGeneration("End", roomPosition.x, roomPosition.y);
                }
                else if (shopIndex == roomIndex)
                {
                    RoomController.Instance.EnqueueRoomToGeneration("Shop", roomPosition.x, roomPosition.y);
                }
                else
                {
                    RoomController.Instance.EnqueueRoomToGeneration("Empty", roomPosition.x, roomPosition.y);
                }
                roomIndex++;
            }
        }
    }
}
