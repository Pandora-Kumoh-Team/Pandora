using System;
using System.Collections.Generic;
using Pandora.Scripts.Player.Controller;
using UnityEngine;

namespace Pandora.Scripts.NewDungeon.Rooms
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InRoomCollider : MonoBehaviour
    {
        private Room _room;
        private CompositeCollider2D _collider;
        private List<PlayerController> _players;
        
        private void Awake()
        {
            _room = GetComponentInParent<Room>();
            _collider = GetComponent<CompositeCollider2D>();
            _players = new List<PlayerController>();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var pc = other.GetComponent<PlayerController>();
                if (_players.Contains(pc)) return;
                _players.Add(pc);
                pc.GetComponent<PlayerAI>()._roomCollider = _collider;
                if (pc.isControlByPlayer)
                    _room.OnPlayerEnter(other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
            {
                var pc = other.GetComponent<PlayerController>();
                if (!_players.Contains(pc)) return;
                _players.Remove(pc);
                if (pc.isControlByPlayer)
                    _room.OnPlayerExit(other.gameObject);
                if (pc.GetComponent<PlayerAI>()._roomCollider == _collider)
                    pc.GetComponent<PlayerAI>()._roomCollider = null;
            }
        }
    }
}