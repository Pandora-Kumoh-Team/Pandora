using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pandora.Scripts
{
    public class AttackRange : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log("collided with " + col.gameObject.name);
        }
    }
}