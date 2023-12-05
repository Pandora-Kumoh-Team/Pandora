using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Controller
{
    public class IdleState : PlayerAIState
    {
        private bool isIdleWait = true;
        private float idleWaitTimer = 2f;
        private float idleWalkTimer = 1f;
        private Vector2 _moveDirection;
        public float idleWalkSpeed = 0.25f;

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
            player._playerController.moveDir = Vector2.zero;
        }

        public override void UpdateState(PlayerAI player)
        {
            // idle wait
            if (isIdleWait)
            {
                if(idleWaitTimer > 0)
                {
                    idleWaitTimer -= Time.deltaTime;
                }
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
                    player._playerController.rb.velocity = _moveDirection;
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

        public override void ExitState(PlayerAI player)
        {
            player._playerController.moveDir = Vector2.zero;
        }

        public override void CheckTransition(PlayerAI player)
        {
            base.CheckTransition(player);
            
        }
    }
}