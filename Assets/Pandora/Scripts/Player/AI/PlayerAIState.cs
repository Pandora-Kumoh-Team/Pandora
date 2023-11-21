namespace Pandora.Scripts.Player.Controller
{
    public abstract class PlayerAIState
    {
        public abstract void Enter(PlayerAI player);
        public abstract void Update(PlayerAI player);
        public abstract void Exit(PlayerAI player);

        public virtual void CheckTransition(PlayerAI player)
        {
            if (player._playerController.isDead)
            {
                player.ChangeState(new DeadState());
            }
        }
    }
}