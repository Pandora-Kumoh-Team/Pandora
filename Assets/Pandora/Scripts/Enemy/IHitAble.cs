using System.Collections.Generic;

namespace Pandora.Scripts.Enemy
{
    public interface IHitAble
    {
        public void Hit(float damage, List<Buff> buff);
    }
}