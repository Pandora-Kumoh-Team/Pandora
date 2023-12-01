using Cinemachine;
using Pandora.Scripts.Player.Controller;
using UnityEngine;

namespace Pandora.Scripts.NewDungeon.Rooms
{
    public class RoomEmpty : Room
    {
        public override void OnPlayerEnter(GameObject playerObject)
        {
            base.OnPlayerEnter(playerObject);
        
            if(!isClear)
            {
                // move other player to this room
                PlayerManager.Instance.GetOtherPlayer(playerObject).transform.position = playerObject.transform.position;
            
                // close all doors
                foreach(Door door in doors)
                {
                    door.CloseDoor();
                }
            }
        }
    }
}