using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Controller
{
    public class MoveToPointState : PlayerAIState
    {
        private Vector2 _movePoint;

        public override void Enter(PlayerAI player)
        {
            throw new NotImplementedException();
        }

        public override void Update(PlayerAI player)
        {
            // 이동 지점 도달시 다시 Idle 상태로
            if (Vector2.Distance(player.transform.position, _movePoint) < 0.1f)
            {
                player.ChangeState(new IdleState());
            }
            else
            {
                player._playerController.moveDir = (_movePoint - (Vector2)player.transform.position).normalized;
            }
        }

        public override void Exit(PlayerAI player)
        {
            throw new NotImplementedException();
        }

        public override void CheckTransition(PlayerAI player)
        {
            base.CheckTransition(player);
        }
    }
}