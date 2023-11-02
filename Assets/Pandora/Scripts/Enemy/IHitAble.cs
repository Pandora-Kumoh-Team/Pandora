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
        /// <param name="damage">플레이어에게 받은 피해량</param>
        /// <param name="buff">플레이어에게 부여받은 디버프</param>
        public void Hit(float damage, List<Buff> buff);
    }
}