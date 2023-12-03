using System.Collections.Generic;
using UnityEngine;

namespace Pandora.Scripts.NewDungeon
{
    public class RoomPositionsGenerator : MonoBehaviour
    {
        private static readonly List<Vector2Int> RoomPositions = new();

        public static List<Vector2Int> GenerateRoomPositions(DungeonGenerationData dungeonData)
        {
            var roomPositions = new List<RoomPosition>();

            for(var i = 0; i < dungeonData.numberOfCrawlers; i++)
            {
                roomPositions.Add(new RoomPosition(Vector2Int.zero));
            }

            var iterations = Random.Range(dungeonData.iterationMin, dungeonData.iterationMax);

            for(var i = 0; i < iterations; i++)
            {
                foreach(var roomPosition in roomPositions)
                {
                    var newPos = roomPosition.GetRandomMovedPosition();
                    RoomPositions.Add(newPos);
                }
            }

            return RoomPositions;
        }
    }
}