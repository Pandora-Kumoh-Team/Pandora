using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Controller
{
    public class DeadState : PlayerAIState
    {
        public override void Enter(PlayerAI player)
        {
            player._playerController.canControllMove = false;
        }

        public override void Update(PlayerAI player)
        {
            player._playerController.playerCurrentStat.NowHealth +=
                player._playerController.playerCurrentStat.NonControlHpRecovery * Time.deltaTime;
            player._playerController.CallHealthChangedEvent();
            if (player._playerController.playerCurrentStat.NowHealth >
                player._playerController.playerCurrentStat.MaxHealth * 0.3f)
            {
                player._playerController.Rebirth();
                player.ChangeState(new IdleState());
            }
        }

        public override void Exit(PlayerAI player)
        {
            player._playerController.canControllMove = true;
        }

        public override void CheckTransition(PlayerAI player)
        {
            base.CheckTransition(player);
        }
    }
}