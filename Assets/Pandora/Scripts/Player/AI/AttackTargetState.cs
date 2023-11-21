using System;
using Pathfinding;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.Player.Controller
{
    public class AttackTargetState : PlayerAIState
    {
        private GameObject _target;
        private Seeker _seeker;
        private Path _path;
        private int _currentPathIndex;
        private Vector2 nowWaypoint;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">GameObject : need Target</param>
        /// <returns></returns>
        public override PlayerAIState Init(object data)
        {
            _target = (GameObject) data;
            IsInitialized = true;
            return this;
        }

        public override void Enter(PlayerAI player)
        {
            _target = player._target;
            _seeker = player.GetComponent<Seeker>();
        }

        public override void Update(PlayerAI player)
        {
            // 타겟 사라졌을시
            if (_target == null)
            {
                player.ChangeState(new MoveToOtherPlayerState().Init(null));
                return;
            }
            if (_target.activeSelf == false)
            {
                player.ChangeState(new IdleState().Init(null));
                _target = null;
                return;
            }
            
            // 공격 사거리만큼 유지하며 접근 레이케스트로 측정하는 방식
            float distance;
            var hit = Physics2D.Raycast( player.transform.position, _target.transform.position - player.transform.position, 100f,
                LayerMask.GetMask("Enemy"));
            if(hit.collider != null)
                distance = hit.distance;
            else
                distance = Vector2.Distance( player.transform.position, _target.transform.position);
            
            // 공격 사거리 이내면 공격
            var _minTargetDistance = player._playerController.playerCurrentStat.AttackRange;
            if (distance <= _minTargetDistance)
            {
                player._playerController.attackDir =
                    (_target.transform.position - player.transform.position).normalized;
                if (player._playerController.CanAttack()) player._playerController.Attack();
            }
            
            // 접근 이동
            if (distance > _minTargetDistance)
            {
                // 적과 나를 잇는 선분에서 적과 최대 사거리만큼 떨어진 지점을 구한다.
                var targetPos = _target.transform.position;
                var myPos = player.transform.position;
                var dir = (targetPos - myPos).normalized;
                var attackingPos = targetPos - dir * _minTargetDistance;
                
                // Seeker를 통해 이동한다.
                if(_path == null || Vector2.Distance(nowWaypoint, targetPos) > 5f)
                {
                    _seeker.StartPath(myPos, attackingPos, OnPathComplete);
                    nowWaypoint = attackingPos;
                }
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
                // 경로 이동 가능 검사
                var hit = Physics2D.Raycast(player.transform.position, dir, 0.5f, LayerMask.GetMask("Enemy", "Wall"));
                if (hit.collider != null)
                {
                    _path = null;
                    return;
                }
                _currentPathIndex++;
                if(_currentPathIndex >= _path.vectorPath.Count)
                    _path = null;
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