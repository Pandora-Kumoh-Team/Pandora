using System;
using Pandora.Scripts.System.Event;
using UnityEngine;

namespace Pandora.Scripts.Player.Controller
{
    public class PlayerAI : MonoBehaviour, IEventListener
    {
        private PlayerController _playerController;
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;

        private AIState _currentState;
        private GameObject _target;
        private float _minTargetDistance;
        private bool isTargetPlayer;

        private enum AIState
        {
            MoveToTarget,
            MoveToPoint,
            Dead
        }
        
        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
            EventManager.Instance.AddListener(PandoraEventType.PlayerAttackEnemy, this);
        }
        
        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener(PandoraEventType.PlayerHealthChanged, this);
        }

        private void Update()
        {
            switch (_currentState)
            {
                case AIState.MoveToTarget:
                    MoveToTarget();
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

        protected void MoveToTarget()
        {
            if (_target == null)
            {
                _target = PlayerManager.Instance.GetOtherPlayer(gameObject);
                isTargetPlayer = true;
                _minTargetDistance = 3f;
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

        protected void MoveToPoint()
        {
            // TODO : 공격하지 않고 긴급 회피
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
            if(pandoraEventType != PandoraEventType.PlayerAttackEnemy) return;
            _target = (GameObject) param;
            _currentState = AIState.MoveToTarget;
            isTargetPlayer = false;
            _minTargetDistance = _playerController._playerStat.AttackRange;
        }
    }
}