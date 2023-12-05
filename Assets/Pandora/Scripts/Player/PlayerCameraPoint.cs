using System;
using Pandora.Scripts.Player.Controller;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pandora.Scripts.Player
{
    public class PlayerCameraPoint : MonoBehaviour
    {
        private Transform player1;
        private Transform player2;
        
        private PlayerController playerController1;

        public float controlFocusRate = 0.8f;
        
        private void Start()
        {
            player1 = PlayerManager.Instance.transform.Find("PlayerCharacterMelee");
            player2 = PlayerManager.Instance.transform.Find("PlayerCharacterRanged");
            playerController1 = player1.GetComponent<PlayerController>();
        }

        private void Update()
        {
            // set camera position to the middle of the two players
            if(playerController1.isControlByPlayer)
                transform.position = (player1.position * controlFocusRate + player2.position * (1 - controlFocusRate));
            else
                transform.position = (player2.position * controlFocusRate + player1.position * (1 - controlFocusRate));
        }
    }
}