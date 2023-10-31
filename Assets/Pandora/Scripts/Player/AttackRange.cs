using System;
using System.Collections.Generic;
using Pandora.Scripts.Enemy;
using Pandora.Scripts.System.Event;
using UnityEngine;

namespace Pandora.Scripts.Player
{
    public class AttackRange : MonoBehaviour
    {
        private float _damage = 10f; // 임시 데메지
        private List<Buff> _buffs = new List<Buff>();
        private List<IHitAble> _hitted = new List<IHitAble>();
        
        public void SetDamage(float damage)
        {
            _damage = damage;
        }
        
        public void SetBuffs(List<Buff> buffs)
        {
            _buffs = buffs;
        }

        private void OnEnable()
        {
            _hitted.Clear();
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            var hitAble = col.gameObject.GetComponent<IHitAble>();
            if (hitAble != null && !_hitted.Contains(hitAble))
            {
                _hitted.Add(hitAble);
                hitAble.Hit(_damage, _buffs);
                EventManager.Instance.TriggerEvent(PandoraEventType.PlayerAttackEnemy, col.gameObject);
            }
        }
    }
}