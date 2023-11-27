using System.Collections.Generic;

namespace Pandora.Scripts.Enemy
{
    public struct HitParams
    {
        public float damage;
        public List<Buff> buff;
        public bool isCritical;

        public HitParams(float damage, List<Buff> buff, bool isCritical = false)
        {
            this.damage = damage;
            this.buff = buff;
            this.isCritical = isCritical;
        }
    }
}