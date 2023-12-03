using System.Collections.Generic;
using UnityEngine;

namespace Pandora.Scripts.Enemy
{
    public struct HitParams
    {
        public float damage;
        public List<Buff> buff;
        public bool isCritical;
        public GameObject attacker;

        public HitParams(float damage, List<Buff> buff, bool isCritical = false, GameObject attacker = null)
        {
            this.damage = damage;
            this.buff = buff;
            this.isCritical = isCritical;
            this.attacker = attacker;
        }
    }
}