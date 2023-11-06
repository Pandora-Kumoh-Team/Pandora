using System;
using System.Collections.Generic;
using Pandora.Scripts.System.Event;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Pandora.Scripts.Player.Controller
{
    public class PlayerAI : MonoBehaviour, IEventListener
    {
        private PlayerController _playerController;
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;

        private AIState _currentState;
        public GameObject _target;
        private Vector2 _movePoint;
        private float _minTargetDistance;
        private bool isTargetPlayer;
        
        private float idleWaitTime;
        
        public float maxOtherPlayerDistance = 10f;

        private enum AIState
        {
            Idle,
            MoveToTarget,
            MoveToOtherPlayer,
            MoveToPoint,
            Dead
        }
        
        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
            EventManager.Instance.AddListener(PandoraEventType.PlayerAttackEnemy, this);
            _currentState = AIState.Idle;
            _movePoint = transform.position;
        }
        
        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener(PandoraEventType.PlayerAttackEnemy, this);
        }

        private void Update()
        {
            // 조작 플레이어와 멀리 떨어지면 조작 플레이어에게 이동
            var otherPlayer = PlayerManager.Instance.GetOtherPlayer(gameObject);
            var distanceToOtherPlayer = Vector2.Distance(transform.position, otherPlayer.transform.position);
            if (distanceToOtherPlayer > maxOtherPlayerDistance)
            {
                _currentState = AIState.MoveToOtherPlayer;
                _movePoint = otherPlayer.transform.position;
            }
            switch (_currentState)
            {
                case AIState.Idle:
                    Idle();
                    break;
                case AIState.MoveToTarget:
                    MoveToTarget();
                    break;
                case AIState.MoveToOtherPlayer:
                    MoveToOtherPlayer();
                    break;  
                case AIState.MoveToPoint:
                    MoveToPoint();
                    break;
                case AIState.Dead:
                    Dead();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _playerController._playerStat.NowHealth += _playerController._playerStat.NonControlHpRecovery * Time.deltaTime;
            _playerController.CallHealthChangedEvent();
        }

        protected void Idle()
        {
            // 상하좌우중 랜덤으로 이동 포인트 설정
            if(((Vector2)transform.position - _movePoint).magnitude >= 0.05f)
            {
                _playerController.moveDir = (_movePoint - (Vector2)transform.position).normalized;
                idleWaitTime = Random.Range(0, 3f);
            }
            else
            {
                _playerController.moveDir = Vector2.zero;
                if (idleWaitTime > 0)
                    idleWaitTime -= Time.deltaTime;
                else
                {
                    var random = Random.Range(0, 4);
                    _movePoint = random switch
                    {
                        0 => new Vector2(transform.position.x + 0.3f, transform.position.y),
                        1 => new Vector2(transform.position.x - 0.3f, transform.position.y),
                        2 => new Vector2(transform.position.x, transform.position.y + 0.3f),
                        3 => new Vector2(transform.position.x, transform.position.y - 0.3f),
                        _ => _movePoint
                    };
                }
            }
        }

        protected void MoveToTarget()
        {
            if (_target == null)
            {
                _currentState = AIState.MoveToOtherPlayer;
                return;
            }
            
            _playerController.attackDir = (_target.transform.position - transform.position).normalized;
            if(_playerController.CanAttack()) _playerController.Attack();
            
            // 공격 사거리만큼 유지하며 접근
            //var distance = Vector2.Distance(transform.position, _target.transform.position);
            // 레이케스트로 측정하는 방식
            float distance;
            if(!isTargetPlayer)
            {
                var hit = Physics2D.Raycast(transform.position, _target.transform.position - transform.position, 100f,
                    LayerMask.GetMask("Enemy"));
                distance = hit.distance;
            }
            else
            {
                distance = Vector2.Distance(transform.position, _target.transform.position);
            }
            if (distance > _minTargetDistance)
            {
                _playerController.moveDir = (_target.transform.position - transform.position).normalized;
            }
            // 너무 접근시 후퇴
            else if (distance < _minTargetDistance * 0.3f)
            {
                _playerController.moveDir = (transform.position - _target.transform.position).normalized;
            }
            else
            {
                _playerController.moveDir = Vector2.zero;
            }
        }
        
        private void MoveToOtherPlayer()
        {
            if (_target == null)
            {
                _target = PlayerManager.Instance.GetOtherPlayer(gameObject);
                isTargetPlayer = true;
                _minTargetDistance = 3f;
            }
            
            var distance= Vector2.Distance(transform.position, _target.transform.position);
            if (distance > _minTargetDistance)
            {
                _playerController.moveDir = (_target.transform.position - transform.position).normalized;
            }
            else
            {
                _currentState = AIState.Idle;
                _playerController.moveDir = Vector2.zero;
            }
        }

        protected void MoveToPoint()
        {
            // 이동 지점 도달시 다시 Idle 상태로
            if (Vector2.Distance(transform.position, _movePoint) < 0.1f)
            {
                _currentState = AIState.Idle;
            }
            else
            {
                _playerController.moveDir = (_movePoint - (Vector2)transform.position).normalized;
            }
        }
        
        protected void Dead()
        {
            // TODO : 죽음
        }

        private void OnDisable()
        {
            _target = null;
        }

        public void OnEvent(PandoraEventType pandoraEventType, Component sender, object param = null)
        {
            if(pandoraEventType == PandoraEventType.PlayerAttackEnemy)
            {
                _target = (GameObject)param;
                _currentState = AIState.MoveToTarget;
                isTargetPlayer = false;
                _minTargetDistance = _playerController._playerStat.AttackRange;
            }
        }
    }
}