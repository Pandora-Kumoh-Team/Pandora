using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Pandora.Scripts.NewDungeon.Rooms
{
    public class RoomStart : Room
    {
        public override void OnPlayerEnter(GameObject playerObject)
        {   
            base.OnPlayerEnter(playerObject);
            
            isClear = true;
            // open doors
        }

        protected override void OnMapGenerateComplete()
        {
            base.OnMapGenerateComplete();
            OpenAllDoors();
        }
    }
}