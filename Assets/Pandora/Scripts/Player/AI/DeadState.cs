using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Controller
{
    public class DeadState : PlayerAIState
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">null : need nothing</param>
        /// <returns></returns>
        public override PlayerAIState Init(object data)
        {
            IsInitialized = true;
            return this;
        }

        public override void EnterState(PlayerAI player)
        {
            player._playerController.canControlMove = false;
        }

        public override void UpdateState(PlayerAI player)
        {
            player._playerController.playerCurrentStat.NowHealth +=
                player._playerController.playerCurrentStat.NonControlHpRecovery * Time.deltaTime;
            player._playerController.CallHealthChangedEvent();
            if (player._playerController.playerCurrentStat.NowHealth >
                player._playerController.playerCurrentStat.MaxHealth * 0.3f)
            {
                player._playerController.Rebirth();
                player.ChangeState(new IdleState().Init(null));
            }
        }

        public override void ExitState(PlayerAI player)
        {
            player._playerController.canControlMove = true;
        }

        public override void CheckTransition(PlayerAI player)
        {
            base.CheckTransition(player);
        }
    }
}