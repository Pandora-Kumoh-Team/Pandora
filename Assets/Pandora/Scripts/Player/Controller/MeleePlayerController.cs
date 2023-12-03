using System.Collections;
using System.Collections.Generic;
using Pandora.Scripts.Enemy;
using UnityEngine;

namespace Pandora.Scripts.Player.Controller
{
    public class MeleePlayerController : PlayerController
    {
        private GameObject _attackRange;
        
        public override void Start()
        {
            base.Start();
            _attackRange = transform.Find("AttackRange").gameObject;
        }

        public override void AttackRangeChanged(float newRange)
        {
            base.AttackRangeChanged(newRange);
            _attackRange.transform.localScale = new Vector3(newRange, newRange, newRange);
        }
        
        // 공격 코루틴
        protected override IEnumerator AttackCoroutine(HitParams hitParams)
        {
            _attackRange.GetComponent<AttackRange>().SetHitParams(hitParams);
            
            // 공격 방향 수정
            var zAngle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg;
            _attackRange.transform.rotation = Quaternion.Euler(0, 0, zAngle);
            
            // 공격 활성화
            _attackRange.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            // 공격 비활성화
            _attackRange.SetActive(false);
        }
    }
}