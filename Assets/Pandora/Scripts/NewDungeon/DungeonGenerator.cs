using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> dungeonRooms;

    private void Start()
    {
        dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData);
        SpawnRooms(dungeonRooms);
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms)
    {
        RoomController.instance.LoadRoom("Start", 0, 0);
        int shopIndex = UnityEngine.Random.Range(3, 10);
        int roomIndex = 0;
        foreach (Vector2Int roomLocation in rooms)
        {
            if (roomIndex == shopIndex)
            {
                RoomController.instance.LoadRoom("Shop", roomLocation.x, roomLocation.y);
            }
            else if (roomLocation == dungeonRooms[dungeonRooms.Count - 1] && !(roomLocation == Vector2Int.zero))
            {
                RoomController.instance.LoadRoom("End", roomLocation.x, roomLocation.y);
            }
            else
            {
                RoomController.instance.LoadRoom("Empty", roomLocation.x, roomLocation.y);
            }
            roomIndex++;
        }
    }
}
