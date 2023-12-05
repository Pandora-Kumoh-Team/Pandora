using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.System.Event
{
    public struct PlayerAttackRangeChangedParam
    {
        /// <summary>
        /// changed value
        /// </summary>
        public float AttackRange;
        public int PlayerNumber;

        public PlayerAttackRangeChangedParam(float attackRange, int playerNumber)
        {
            this.AttackRange = attackRange;
            this.PlayerNumber = playerNumber;
        }
    }
}