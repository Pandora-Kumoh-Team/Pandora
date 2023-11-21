using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Controller
{
    public class IdleState : PlayerAIState
    {
        private bool isIdleWait;
        private float idleWaitTimer;
        private float idleWalkTimer;
        private Vector2 _moveDirection;
        public float idleWalkSpeed;

        public override void Enter(PlayerAI player)
        {
            player._playerController.moveDir = Vector2.zero;
        }

        public override void Update(PlayerAI player)
        {
            // idle wait
            if (isIdleWait)
            {
                if(idleWaitTimer > 0)
                    idleWaitTimer -= Time.deltaTime;
                else
                {
                    isIdleWait = false;
                    idleWalkTimer = Random.Range(0.5f, 1f);
                    var random = Random.Range(0, 4);
                    _moveDirection = random switch
                    {
                        0 => Vector2.up,
                        1 => Vector2.down,
                        2 => Vector2.left,
                        3 => Vector2.right,
                        _ => _moveDirection
                    };
                    _moveDirection *= idleWalkSpeed;
                }
            }
            // idle walk
            else
            {
                if (idleWalkTimer > 0)
                {
                    player._playerController.moveDir = _moveDirection;
                    idleWalkTimer -= Time.deltaTime;
                }
                else
                {
                    player._playerController.moveDir = Vector2.zero;
                    idleWaitTimer = Random.Range(1f, 2f);
                    isIdleWait = true;
                }
            }
        }

        public override void Exit(PlayerAI player)
        {
            player._playerController.moveDir = Vector2.zero;
        }

        public override void CheckTransition(PlayerAI player)
        {
            base.CheckTransition(player);
            var otherPlayer = PlayerManager.Instance.GetOtherPlayer(player.gameObject);
            var distanceToOtherPlayer = Vector2.Distance(player.transform.position, otherPlayer.transform.position);
            if (distanceToOtherPlayer > player.maxOtherPlayerDistance)
            {
                player.ChangeState(new MoveToOtherPlayerState());
            }
        }
    }
}