using System;
using Pandora.Scripts.Player;
using Pandora.Scripts.Player.Controller;
using UnityEngine;

namespace Pandora.Scripts.DebugConsole.Player
{
    public class PlayerHurtDebug : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.CompareTag("Player"))
            {
                col.GetComponent<PlayerController>().Hurt(10, null, gameObject);
            }
        }
    }
}