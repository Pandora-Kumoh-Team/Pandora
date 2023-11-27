using System;
using System.Collections.Generic;
using Pandora.Scripts.Enemy;
using Pandora.Scripts.System.Event;
using UnityEngine;

namespace Pandora.Scripts.Player
{
    public class AttackRange : MonoBehaviour
    {
        private HitParams _hitParams;
        private List<IHitAble> _hitted = new List<IHitAble>();
        
        public void SetHitParams(HitParams hitParams)
        {
            _hitParams = hitParams;
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
                hitAble.Hit(_hitParams);
                EventManager.Instance.TriggerEvent(PandoraEventType.PlayerAttackEnemy, col.gameObject);
            }
        }
    }
}