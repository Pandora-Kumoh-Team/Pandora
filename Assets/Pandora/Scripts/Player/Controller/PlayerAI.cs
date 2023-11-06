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

        
        public AIState _currentState;
        
        public GameObject _target;
        private float _minTargetDistance;
        
        private Vector2 _moveDirection;
        private Vector2 _movePoint;

        private float idleWalkTimer;
        private float idleWaitTimer;
        private bool isIdleWait;
        
        public float maxOtherPlayerDistance = 10f;
        public float idleWalkSpeed = 0.25f;

        public enum AIState
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
        }

        private void OnEnable()
        {
            if(_target != null)
                _currentState = AIState.MoveToTarget;
            else
                _currentState = AIState.Idle;
            _moveDirection = Vector2.zero;
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
            if (distanceToOtherPlayer > maxOtherPlayerDistance && _currentState == AIState.Idle)
            {
                _currentState = AIState.MoveToOtherPlayer;
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
                    _playerController.moveDir = _moveDirection;
                    idleWalkTimer -= Time.deltaTime;
                }
                else
                {
                    _playerController.moveDir = Vector2.zero;
                    idleWaitTimer = Random.Range(1f, 2f);
                    isIdleWait = true;
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
            if (_target.activeSelf == false)
            {
                _currentState = AIState.Idle;
                _target = null;
                return;
            }
            _playerController.attackDir = (_target.transform.position - transform.position).normalized;
            if(_playerController.CanAttack()) _playerController.Attack();
            
            // 공격 사거리만큼 유지하며 접근 레이케스트로 측정하는 방식
            float distance;
            var hit = Physics2D.Raycast(transform.position, _target.transform.position - transform.position, 100f,
                LayerMask.GetMask("Enemy"));
            if(hit.collider != null)
                distance = hit.distance;
            else
                distance = Vector2.Distance(transform.position, _target.transform.position);
            // 접근
            if (distance > _minTargetDistance)
            {
                _playerController.moveDir = (_target.transform.position - transform.position).normalized;
            }
            // 너무 접근시 후퇴
            else if (distance < _minTargetDistance * 0.3f)
            {
                _playerController.moveDir = (transform.position - _target.transform.position).normalized;
            }
            // 중간 값에선 정지
            else
            {
                // _playerController.moveDir = Vector2.zero;
            }
        }
        
        private void MoveToOtherPlayer()
        {
            _target = PlayerManager.Instance.GetOtherPlayer(gameObject);
            var distance= Vector2.Distance(transform.position, _target.transform.position);
            if (distance > maxOtherPlayerDistance * 0.5f)
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
                _minTargetDistance = _playerController._playerStat.AttackRange;
            }
        }
    }
}