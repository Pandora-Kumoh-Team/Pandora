using System;
using UnityEngine;

namespace Pandora.Scripts.Player.Controller
{
    public class PlayerManager : MonoBehaviour
    {
        // Singleton
        public static PlayerManager Instance { get; private set; }
        
        private GameObject _zeroPlayer;
        private GameObject _firstPlayer;
        
        private void Awake()
        {
            // Singleton
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            _zeroPlayer = transform.GetChild(0).gameObject;
            _firstPlayer = transform.GetChild(1).gameObject;
        }

        public GameObject GetOtherPlayer(GameObject o)
        {
            if (o == _zeroPlayer)
            {
                return _firstPlayer;
            }
            if (o == _firstPlayer)
            {
                return _zeroPlayer;
            }
            throw new Exception("PlayerManager: GetOtherPlayer: GameObject is not a player");
        }

        public GameObject[] GetPlayers()
        {
            return new[] {_zeroPlayer, _firstPlayer};
        }

        public GameObject GetPlayer(int playerNum)
        {
            if (playerNum == 0)
            {
                return _zeroPlayer;
            }
            if (playerNum == 1)
            {
                return _firstPlayer;
            }
            throw new Exception("PlayerManager: GetPlayer: playerNum is not 1 or 2");
        }
    }
}