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

        public override void Enter(PlayerAI player)
        {
            _seeker = player.GetComponent<Seeker>();
            maxOtherPlayerDistance = player.maxOtherPlayerDistance;
        }

        public override void Update(PlayerAI player)
        {
            _target = PlayerManager.Instance.GetOtherPlayer(player.gameObject);
            if(_currentWaypoint == Vector2.zero)
                _currentWaypoint = _target.transform.position;
            var distance= Vector2.Distance(player.transform.position, _target.transform.position);
            var finalPathDistance = Vector2.Distance(_target.transform.position, _currentWaypoint);
            // if final path is null or target is too far, find new path
            if (_path == null || finalPathDistance > maxOtherPlayerDistance)
            {
                _seeker.StartPath(player.transform.position, _target.transform.position, OnPathComplete);
                _currentWaypoint = _target.transform.position;
            }
            else if (_currentPathIndex >= _path.vectorPath.Count)
            {
                player.ChangeState(new IdleState());
                player._playerController.moveDir = Vector2.zero;
                _path = null;
            }
            else if (distance < _minTargetDistance)
            {
                player.ChangeState(new IdleState());
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

        public override void Exit(PlayerAI player)
        {
            
        }

        public override void CheckTransition(PlayerAI player)
        {
            base.CheckTransition(player);
        }
    }
}