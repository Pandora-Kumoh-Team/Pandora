using System.Collections.Generic;
using UnityEngine;

namespace Pandora.Scripts.NewDungeon
{
    public class RoomPositionsGenerator : MonoBehaviour
    {
        public static List<Vector2Int> RoomPositions = new();
        public static int stage = 0;

        public static List<Vector2Int> GenerateRoomPositions(DungeonGenerationData dungeonData)
        {
            if (++stage == 3)
            {
                stage = 0;
                RoomPositions.Clear();
            }
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
            
            // Remove duplicates by bobble checking
            for(var i = 0; i < RoomPositions.Count; i++)
            {
                for(var j = i + 1; j < RoomPositions.Count; j++)
                {
                    if (RoomPositions[i] == RoomPositions[j])
                    {
                        RoomPositions.RemoveAt(j);
                        j--;
                    }
                }
            }

            return RoomPositions;
        }
    }
}