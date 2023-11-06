using System;
using UnityEngine;

namespace Pandora.Scripts.Player.Controller
{
    public class PlayerManager : MonoBehaviour
    {
        // Singleton
        public static PlayerManager Instance { get; private set; }
        
        private GameObject _firstPlayer;
        private GameObject _secondPlayer;
        
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

            _firstPlayer = transform.GetChild(0).gameObject;
            _secondPlayer = transform.GetChild(1).gameObject;
        }

        public GameObject GetOtherPlayer(GameObject o)
        {
            if (o == _firstPlayer)
            {
                return _secondPlayer;
            }
            if (o == _secondPlayer)
            {
                return _firstPlayer;
            }
            throw new Exception("PlayerManager: GetOtherPlayer: GameObject is not a player");
        }
    }
}