using Pathfinding;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Controller
{
    public class MoveToPointState : PlayerAIState
    {
        private Vector2 _movePoint;
        private Seeker _seeker;
        private Path _path;
        private int _currentPathIndex;

        /// <summary>
        /// need to be called before using this state
        /// </summary>
        /// <param name="data">Vector2 : need target point</param>
        /// <returns></returns>
        public override PlayerAIState Init(object data)
        {
            _movePoint = (Vector2) data;
            IsInitialized = true;
            return this;
        }

        public override void EnterState(PlayerAI player)
        {
            _seeker = player.GetComponent<Seeker>();
            _seeker.pathCallback += OnPathComplete;
            _seeker.StartPath(player.transform.position, _movePoint);
        }

        private void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                _path = p;
                _currentPathIndex = 0;
            }
        }

        public override void UpdateState(PlayerAI player)
        {
            if (_path == null)
            {
                return;
            }

            if (_currentPathIndex >= _path.vectorPath.Count)
            {
                player.ChangeState(new IdleState().Init(null));
                return;
            }
            player._playerController.moveDir =
                ((Vector2)_path.vectorPath[_currentPathIndex] - (Vector2)player.transform.position).normalized;
            var distance = Vector2.Distance(player.transform.position, _path.vectorPath[_currentPathIndex]);
            if (distance < 0.1f)
            {
                _currentPathIndex++;
            }
        }

        public override void ExitState(PlayerAI player)
        {
            _path = null;
            _seeker.pathCallback -= OnPathComplete;
            IsInitialized = false;
        }

        public override void CheckTransition(PlayerAI player)
        {
            base.CheckTransition(player);
        }
    }
}