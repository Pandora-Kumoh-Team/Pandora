using Pathfinding;
using UnityEngine;

namespace Pandora.Scripts.Player.Controller
{
    public class AttackTargetState : PlayerAIState
    {
        private GameObject _target;
        private float _minTargetDistance;
        private Seeker _seeker;
        private Path _path;
        private int _currentPathIndex;

        public override void Enter(PlayerAI player)
        {
            _target = player._target;
            _minTargetDistance = player._minTargetDistance;
            _seeker = player.GetComponent<Seeker>();
        }

        public override void Update(PlayerAI player)
        {
            if (_target == null)
            {
                player.ChangeState(new MoveToOtherPlayerState());
                return;
            }
            if (_target.activeSelf == false)
            {
                player.ChangeState(new IdleState());
                _target = null;
                return;
            }
            player._playerController.attackDir = (_target.transform.position - player.transform.position).normalized;
            if(player._playerController.CanAttack()) player._playerController.Attack();
            
            // 공격 사거리만큼 유지하며 접근 레이케스트로 측정하는 방식
            float distance;
            var hit = Physics2D.Raycast( player.transform.position, _target.transform.position - player.transform.position, 100f,
                LayerMask.GetMask("Enemy"));
            if(hit.collider != null)
                distance = hit.distance;
            else
                distance = Vector2.Distance( player.transform.position, _target.transform.position);
            // 접근
            if (distance > _minTargetDistance)
            {
                // 적과 나를 잇는 선분에서 적과 최대 사거리만큼 떨어진 지점을 구한다.
                var targetPos = _target.transform.position;
                var myPos = player.transform.position;
                var dir = (targetPos - myPos).normalized;
                var attackingPos = targetPos - dir * _minTargetDistance;
                
                // Seeker를 통해 이동한다.
                _seeker.StartPath(myPos, attackingPos, OnPathComplete);
                MoveToTarget(player);
            }
            // 너무 접근시 후퇴
            else if (distance < _minTargetDistance * 0.6f)
            {
                player._playerController.moveDir = (player.transform.position - _target.transform.position).normalized;
            }
            // 중간 값에선 정지
            else
            {
                player._playerController.moveDir = Vector2.zero;
            }
        }
        
        // 경로 존재시 이동
        private void MoveToTarget(PlayerAI player)
        {
            if (_path == null) return;
            if (_currentPathIndex >= _path.vectorPath.Count) return;
            var dir = (_path.vectorPath[_currentPathIndex] - player.transform.position).normalized;
            player._playerController.moveDir = dir;
            if (Vector2.Distance(player.transform.position, _path.vectorPath[_currentPathIndex]) < 0.1f)
            {
                _currentPathIndex++;
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