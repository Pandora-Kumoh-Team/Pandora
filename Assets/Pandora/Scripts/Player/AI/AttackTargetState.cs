using System;
using System.Collections.Generic;
using System.Linq;
using Pandora.Scripts.NewDungeon.Rooms;
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
        private Collider2D _roomCollider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">GameObject : need Target, if null find enemy in 3 distance</param>
        /// <returns></returns>
        public override PlayerAIState Init(object data)
        {
            _target = (GameObject) data;
            IsInitialized = true;
            return this;
        }

        public override void EnterState(PlayerAI player)
        {
            _target = player._target;
            _seeker = player.GetComponent<Seeker>();
            _roomCollider = player._roomCollider;
        }

        public override void UpdateState(PlayerAI player)
        {
            // 타겟 사라졌을시
            if (_target == null || _target.activeSelf == false)
            {
                // 3f 이내의 타켓을 찾는다
                _target = FindTarget(player);
                if (_target == null)
                {
                    // 타겟이 없으면 Idle 상태로 전환
                    player.ChangeState(new IdleState().Init(null));
                    return;
                }
            }
            
            // 공격 사거리만큼 유지하며 접근 레이케스트로 측정하는 방식
            float distance;
            var hit = Physics2D.Raycast( player.transform.position, _target.transform.position - player.transform.position, 100f,
                LayerMask.GetMask("Enemy"));
            if (hit.collider != null)
                distance = hit.distance;
            else
                distance = _target.GetComponent<Collider2D>().Distance(player.GetComponent<Collider2D>()).distance;
            
            // 공격 사거리 이내면 공격
            var _minTargetDistance = player._playerController.playerCurrentStat.AttackRange;
            if (distance <= _minTargetDistance + 1f)
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
            // Debug.DrawLine(safePoint + Vector2.up * 0.5f, safePoint - Vector2.up * 0.5f, Color.blue);
            // Debug.DrawLine(safePoint + Vector2.left * 0.5f, safePoint - Vector2.left * 0.5f, Color.blue);
            // DrawCircle(player.transform.position, 5f, Color.green);
        }

        private GameObject FindTarget(PlayerAI player)
        {
            // 3f 이내의 타겟을 찾는다
            Collider2D[] results = new Collider2D[5];
            var size = Physics2D.OverlapCircleNonAlloc(player.transform.position, 3f, results, LayerMask.GetMask("Enemy"));
            if (size == 0) return null;
            var target = results[0].gameObject;
            var minDistance = Vector2.Distance(player.transform.position, target.transform.position);
            for (int i = 1; i < size; i++)
            {
                var distance = Vector2.Distance(player.transform.position, results[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = results[i].gameObject;
                }
            }
            
            return target;
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
            var dangerColliders =
                Physics2D.OverlapCircleAll(myPos, findDangerRange, LayerMask.GetMask("DangerRange", "Enemy"));
            var targetCollider = _target.GetComponent<Collider2D>();
            
            // 위험요소를 가까운 순으로 3개만 고른다.
            var dangerList = dangerColliders.ToList().OrderBy(x => Vector2.Distance(x.transform.position, myPos)).ToList();
            if (dangerList.Count > 3)
                dangerColliders = dangerList.GetRange(0, 3).ToArray();
            
            
            // 위험요소 위치에 십자가를 그린다.
            foreach (var danger in dangerColliders)
            {
                var dangerPos = danger.transform.position;
                // Debug.DrawLine(dangerPos + Vector3.up * 0.5f, dangerPos - Vector3.up * 0.2f, Color.red);
                // Debug.DrawLine(dangerPos + Vector3.left * 0.5f, dangerPos - Vector3.left * 0.2f, Color.red);
            }
            
            // 원 안에 위험요소가 없으면 현재 위치와 가장 가까운 원 안의 위치를 반환한다.
            if (dangerColliders.Length == 0)
            {
                var dir = (targetPos - myPos).normalized;
                return targetPos - dir * attackRange;
            }
            // 원 안에 위험요소가 있으면 모든 위험요소로 부터 가장 먼거리에 있는 원 안의 점을 반환한다.
            else
            {
                // 원 테두리의 방향의 점을 배열에 넣는다
                const int pointsCount = 32;
                const int pointsAtOneWay = 2;
                var points = new Vector3[pointsCount * pointsAtOneWay];
                for (var i = 0; i < pointsCount; i += pointsAtOneWay)
                {
                    var rad = i * Mathf.PI / pointsCount * 2;
                    for(var j = 0; j < pointsAtOneWay; j++)
                    {
                        points[i + j] = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * attackRange * (1 - (float)j / pointsAtOneWay) + targetPos;
                    }
                    // collider 크기를 고려하여 거리를 더 먼곳으로 설정한다.
                    var closestPoint = targetCollider.ClosestPoint(points[i]);
                    var distance = Vector2.Distance(closestPoint, targetPos);
                    var addVector = (points[i] - targetPos).normalized * distance;
                    for (int j = 0; j < pointsAtOneWay; j++)
                    {
                        points[i + j] += addVector;
                    }
                }
                
                // 전투중인 방 안에 없는 점을 제거한다
                var pointsList = points.ToList();
                if (_roomCollider != null)
                {
                    foreach (var point in pointsList)
                    {
                        // 디버그 십자가 그리기 만약 _roomCollider 안에 있으면 초록색 아니면 빨간색
                        // var color = _roomCollider.OverlapPoint(point) ? Color.green : Color.red;
                        // Debug.DrawLine(point + Vector3.up * 0.2f, point - Vector3.up * 0.2f, color);
                        // Debug.DrawLine(point + Vector3.left * 0.2f, point - Vector3.left * 0.2f, color);
                    }
                    pointsList.RemoveAll(x => !_roomCollider.OverlapPoint(x));
                    points = pointsList.ToArray();
                }
                if(points.Length == 0)
                {
                    return myPos;
                }
                // _roomCollider box 를 그린다
                var box = _roomCollider.bounds;
                Debug.DrawLine(new Vector3(box.min.x, box.min.y), new Vector3(box.min.x, box.max.y), Color.blue);
                Debug.DrawLine(new Vector3(box.min.x, box.max.y), new Vector3(box.max.x, box.max.y), Color.blue);
                Debug.DrawLine(new Vector3(box.max.x, box.max.y), new Vector3(box.max.x, box.min.y), Color.blue);
                Debug.DrawLine(new Vector3(box.max.x, box.min.y), new Vector3(box.min.x, box.min.y), Color.blue);
                
                // 각 점들의 각 위험요소와의 거리를 위험도로 반환하여 합을 구한다.
                var sumDangerAmount = new float[points.Length];
                for (var i = 0; i < points.Length; i++)
                {
                    var sum = 0f;
                    foreach (var danger in dangerColliders)
                    {
                        var closestPoint = danger.ClosestPoint(points[i]);
                        sum += 10 / Vector2.Distance(closestPoint, danger.transform.position);
                    }
                    // 현재 위치와 목표 위치와의 거리를 뺀다.
                    var distance = Vector2.Distance(points[i], player.transform.position);
                    sumDangerAmount[i] = sum;
                }
                // 점들을 위험도가 낮은 순으로 정렬한다.
                var sortedPoints = points.ToList().OrderBy(x => sumDangerAmount[Array.IndexOf(points, x)]).ToList();
                var nearestPoint = sortedPoints[0];
                var nearestPointDistance = Vector2.Distance(nearestPoint, player.transform.position);
                for (var i = 0; i < sortedPoints.Count / 4; i++)
                {
                    var distance = Vector2.Distance(sortedPoints[i], player.transform.position);
                    if (distance > nearestPointDistance)
                        break;
                    nearestPoint = sortedPoints[i];
                    nearestPointDistance = distance;
                }
                return nearestPoint;
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

        public override void ExitState(PlayerAI player)
        {
        }

        public override void CheckTransition(PlayerAI player)
        {
            base.CheckTransition(player);
        }
    }
}