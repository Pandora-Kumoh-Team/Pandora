using System.Collections.Generic;
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
            foreach (var roomPosition in roomPositions)
            {
                if(roomPosition == dungeonRoomPositions[^1] && roomPosition != Vector2Int.zero)
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
