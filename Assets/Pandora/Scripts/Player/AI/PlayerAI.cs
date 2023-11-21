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
        // components
        public PlayerController _playerController;
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;

        public PlayerAIState _currentState;
        public GameObject _target;
        
        public float maxOtherPlayerDistance = 5f;
        
        private void Start()
        {
            _playerController = GetComponent<PlayerController>();
            EventManager.Instance.AddListener(PandoraEventType.PlayerAttackEnemy, this);
        }

        private void OnEnable()
        {
            _playerController = GetComponent<PlayerController>();
            if(_target != null)
                ChangeState(new AttackTargetState().Init(_target));
            else
                ChangeState(new IdleState().Init(null));
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

        /// <summary>
        /// *** AIState need to Init() before use ***
        /// </summary>
        /// <param name="playerAIState"></param>
        /// <exception cref="Exception"></exception>
        public void ChangeState(PlayerAIState playerAIState)
        {
            if(!playerAIState.IsInitialized)
                throw new Exception("PlayerAIState is not initialized");
            _currentState?.Exit(this);
            _currentState = playerAIState;
            playerAIState.Enter(this);
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
                ChangeState(new AttackTargetState().Init(_target));
            }
        }
    }
}