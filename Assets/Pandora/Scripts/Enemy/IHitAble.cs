using System.Collections.Generic;

namespace Pandora.Scripts.Enemy
{
    /// <summary>
    /// 플레이어에게 피격당할 수 있는 클래스들이 구현해야 하는 클래스
    /// </summary>
    public interface IHitAble
    {
        /// <summary>
        /// 플레이어에게 피격당했을 때, 호출되는 클래스
        /// </summary>
        /// <param name="hitParams"></param>
        public void Hit(HitParams hitParams);
    }
}