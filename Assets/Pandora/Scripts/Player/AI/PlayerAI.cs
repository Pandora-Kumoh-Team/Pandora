using System;
using System.Collections.Generic;
using Pandora.Scripts.System.Event;
using Pathfinding;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Pandora.Scripts.Player.Controller
{
    public class PlayerAI : MonoBehaviour, IEventListener
    {
        public PlayerController _playerController;
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;

        
        public PlayerAIState _currentState;
        
        public GameObject _target;
        public float _minTargetDistance;
        
        private Vector2 _moveDirection;
        private Vector2 _movePoint;

        private float idleWalkTimer;
        private float idleWaitTimer;
        private bool isIdleWait;
        
        public float maxOtherPlayerDistance = 10f;
        public float idleWalkSpeed = 0.25f;
        
        private Seeker _seeker;
        private Path _path;
        private Vector2 _currentWaypoint;
        private int _currentPathIndex;
        
        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
            _seeker = GetComponent<Seeker>();
            EventManager.Instance.AddListener(PandoraEventType.PlayerAttackEnemy, this);
        }

        private void OnEnable()
        {
            _playerController = GetComponent<PlayerController>();
            if(_target != null)
                ChangeState(new AttackTargetState());
            else
                ChangeState(new IdleState());
        }

        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener(PandoraEventType.PlayerAttackEnemy, this);
        }

        private void Update()
        {
            // 비조작 자연회복
            _currentState.CheckTransition(this);
            _currentState.Update(this);
        }

        public void ChangeState(PlayerAIState playerAIState)
        {
            _currentState?.Exit(this);
            _currentState = playerAIState;
            playerAIState.Enter(this);
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
                ChangeState(new AttackTargetState());
                _minTargetDistance = _playerController.playerCurrentStat.AttackRange;
            }
        }
    }
}