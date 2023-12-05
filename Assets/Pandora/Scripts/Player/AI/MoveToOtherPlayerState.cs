using Pathfinding;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Controller
{
    public class MoveToOtherPlayerState : PlayerAIState
    {
        private GameObject _target;
        private Vector2 _currentWaypoint;
        private Path _path;
        public float maxOtherPlayerDistance;
        private Seeker _seeker;
        private int _currentPathIndex;
        private float _minTargetDistance;
        private bool findingPath;

        /// <summary>
        /// needs to be called before the state is used
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
            _seeker = player.GetComponent<Seeker>();
            maxOtherPlayerDistance = player.maxOtherPlayerDistance;
        }

        public override void UpdateState(PlayerAI player)
        {
            _target = PlayerManager.Instance.GetOtherPlayer(player.gameObject);
            if(_currentWaypoint == Vector2.zero)
                _currentWaypoint = _target.transform.position;
            var distance= Vector2.Distance(player.transform.position, _target.transform.position);
            // 거리가 멀면 바로 텔레포트
            if (distance > maxOtherPlayerDistance * 2.5f)
            {
                player.transform.position = _target.transform.position;
                _currentWaypoint = _target.transform.position;
                return;
            }
            var finalPathDistance = Vector2.Distance(_target.transform.position, _currentWaypoint);
            // if final path is null or target is too far, find new path
            if (_path == null || finalPathDistance > maxOtherPlayerDistance)
            {
                _seeker.StartPath(player.transform.position, _target.transform.position, OnPathComplete);
                _currentWaypoint = _target.transform.position;
            }
            else if (_currentPathIndex >= _path.vectorPath.Count)
            {
                player.ChangeState(new IdleState().Init(null));
                player._playerController.moveDir = Vector2.zero;
                _path = null;
            }
            else if (distance < _minTargetDistance)
            {
                player.ChangeState(new IdleState().Init(null));
                player._playerController.moveDir = Vector2.zero;
                _path = null;
            }
            else
            {
                player._playerController.moveDir = (_path.vectorPath[_currentPathIndex] - player.transform.position).normalized;
                if (Vector2.Distance(player.transform.position, _path.vectorPath[_currentPathIndex]) < 0.1f)
                {
                    _currentPathIndex++;
                }
            }
        }
        
        private void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                _path = p;
                _currentPathIndex = 0;
            }
        }

        public override void ExitState(PlayerAI player)
        {
            
        }

        public override void CheckTransition(PlayerAI player)
        {
            base.CheckTransition(player);
        }
    }
}