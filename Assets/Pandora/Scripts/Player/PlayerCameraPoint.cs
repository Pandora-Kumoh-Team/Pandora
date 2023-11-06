using System;
using Pandora.Scripts.Player.Controller;
using UnityEngine;

namespace Pandora.Scripts.Player
{
    public class PlayerCameraPoint : MonoBehaviour
    {
        private void Update()
        {
            var player1 = PlayerManager.Instance.transform.Find("PlayerCharacterMelee");
            var player2 = PlayerManager.Instance.transform.Find("PlayerCharacterRanged");
            
            // set camera position to the middle of the two players
            transform.position = (player1.position + player2.position) / 2;
        }
    }
}