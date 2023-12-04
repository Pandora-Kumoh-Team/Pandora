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
                if (pc.onControl)
                    _room.OnPlayerEnter(other.gameObject);
                else
                    pc.GetComponent<PlayerAI>()._roomCollider = _collider;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.CompareTag("Player"))
            {
                var pc = other.GetComponent<PlayerController>();
                if (!_players.Contains(pc)) return;
                _players.Remove(pc);
                if (pc.onControl)
                    _room.OnPlayerExit(other.gameObject);
                else if (pc.GetComponent<PlayerAI>()._roomCollider == _collider)
                    pc.GetComponent<PlayerAI>()._roomCollider = null;
            }
        }
    }
}