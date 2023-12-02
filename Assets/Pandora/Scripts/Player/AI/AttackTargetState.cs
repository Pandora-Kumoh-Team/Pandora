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
            // 적과 나를 잇는 선분에서 적과 최대 사거리만큼 떨어진 지점을 구한다.
            var targetPos = _target.transform.position;
            var myPos = player.transform.position;
            var attackingPos = GetSafePosition(targetPos, player);
            
            // Seeker를 통해 이동한다.
            if(Vector2.Distance(attackingPos, myPos) > 1f)
            {
                if (_path == null || Vector2.Distance(nowWaypoint, targetPos) > 5f)
                {
                    _seeker.StartPath(myPos, attackingPos, OnPathComplete);
                    nowWaypoint = attackingPos;
                }
            }
            else
            {
                player._playerController.moveDir = Vector2.zero;
            }
            MoveToTarget(player);
            
            
            var safePoint = GetSafePosition(_target.transform.position, player);
            Debug.DrawLine(safePoint + Vector2.up * 0.5f, safePoint - Vector2.up * 0.5f, Color.red);
            Debug.DrawLine(safePoint + Vector2.left * 0.5f, safePoint - Vector2.left * 0.5f, Color.red);
            DrawCircle(player.transform.position, 5f, Color.green);
        }
        
        // Debug Draw Circle
        private void DrawCircle(Vector2 center, float radius, Color color)
        {
            var theta = 0f;
            var x = radius * Mathf.Cos(theta);
            var y = radius * Mathf.Sin(theta);
            var pos = center + new Vector2(x, y);
            var newPos = Vector2.zero;
            for (var i = 0; i < 30; i++)
            {
                theta += (2 * Mathf.PI * 10) / 360;
                x = radius * Mathf.Cos(theta);
                y = radius * Mathf.Sin(theta);
                newPos = center + new Vector2(x, y);
                Debug.DrawLine(pos, newPos, color);
                pos = newPos;
            }
        }
        
        private Vector2 GetSafePosition(Vector3 targetPos, PlayerAI player)
        {
            var attackRange = player._playerController.playerCurrentStat.AttackRange;
            var myPos = player.transform.position;
            const float findDangerRange = 5f;
            // 근처에 있는 위험요소를 감지한다.
            var dangerRange =
                Physics2D.OverlapCircleAll(myPos, findDangerRange, LayerMask.GetMask("DangerRange", "Enemy"));
            
            // 원 안에 위험요소가 없으면 현재 위치와 가장 가까운 원 안의 위치를 반환한다.
            if (dangerRange.Length == 0)
            {
                var dir = (targetPos - myPos).normalized;
                return targetPos - dir * attackRange;
            }
            // 원 안에 위험요소가 있으면 모든 위험요소로 부터 가장 먼거리에 있는 원 안의 점을 반환한다.
            else
            {
                // 원 테두리의 방향의 점을 배열에 넣는다
                const int pointsCount = 32;
                var points = new Vector3[pointsCount];
                for (var i = 0; i < pointsCount; i++)
                {
                    var rad = i * Mathf.PI / pointsCount * 2;
                    points[i] = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * attackRange + targetPos;
                }
                // 각 점들의 각 위험요소와의 거리의 합을 구한다.
                var sumDistances = new float[pointsCount];
                for (var i = 0; i < pointsCount; i++)
                {
                    var sum = 0f;
                    foreach (var danger in dangerRange)
                    {
                        sum += Vector2.Distance(points[i], danger.transform.position);
                    }
                    // 현재 위치와 목표 위치와의 거리를 뺀다.
                    sumDistances[i] = sum - Vector2.Distance(points[i], player.transform.position);
                }
                // 가장 거리가 먼 점을 반환한다.
                var maxIndex = 0;
                for (var i = 1; i < pointsCount; i++)
                {
                    if (sumDistances[maxIndex] < sumDistances[i]) maxIndex = i;
                }
                return points[maxIndex];
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