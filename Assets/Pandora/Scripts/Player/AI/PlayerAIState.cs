using UnityEngine;

namespace Pandora.Scripts.Player.Controller
{
    public abstract class PlayerAIState
    {
        public bool IsInitialized = false;
        
        public abstract PlayerAIState Init(object data);
        public abstract void EnterState(PlayerAI player);
        public abstract void UpdateState(PlayerAI player);
        public abstract void ExitState(PlayerAI player);

        public virtual void CheckTransition(PlayerAI player)
        {
            if (player._playerController.isDead)
            {
                player.ChangeState(new DeadState().Init(null));
                return;
            }
            var otherPlayer = PlayerManager.Instance.GetOtherPlayer(player.gameObject);
            var distanceToOtherPlayer = Vector2.Distance(player.transform.position, otherPlayer.transform.position);
            if (distanceToOtherPlayer > player.maxOtherPlayerDistance && player._currentState is IdleState 
                || distanceToOtherPlayer > player.maxOtherPlayerDistance * 2 && player._currentState is not MoveToOtherPlayerState)
            {
                player.ChangeState(new MoveToOtherPlayerState().Init(null));
            }
            
        }
    }
}