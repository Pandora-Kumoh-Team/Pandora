using Pandora.Scripts.Player.Controller;
using UnityEngine;

namespace Pandora.Scripts.NewDungeon.Rooms
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class RoomEnterCollider : MonoBehaviour
    {
        private Room _room;
        private BoxCollider2D _collider;
        
        private void Awake()
        {
            _room = GetComponentInParent<Room>();
            _collider = GetComponent<BoxCollider2D>();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var pc = other.GetComponent<PlayerController>();
                if (!pc.onControl) return;
                _room.OnPlayerEnter(other.gameObject);
            }
        }
    }
}