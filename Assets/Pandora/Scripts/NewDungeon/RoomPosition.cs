using System.Collections.Generic;
using UnityEngine;

namespace Pandora.Scripts.NewDungeon
{
    public enum Direction
    {
        up = 0,
        Left = 1,
        Down = 2,
        Right = 3
    };

    public class RoomPosition
    {
        private Vector2Int Position { get; set; }
        
        private static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new()
        {
            {Direction.up, Vector2Int.up},
            {Direction.Left, Vector2Int.left},
            {Direction.Down, Vector2Int.down},
            {Direction.Right, Vector2Int.right}
        };
        
        public RoomPosition(Vector2Int startPos)
        {
            Position = startPos;
        }

        public Vector2Int GetRandomMovedPosition()
        {
            var toMove = (Direction) Random.Range(0, directionMovementMap.Count);
            Position += directionMovementMap[toMove];
            return Position;
        }
    }
}
