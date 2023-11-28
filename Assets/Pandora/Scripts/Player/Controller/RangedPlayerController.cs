using System.Collections;
using System.Collections.Generic;
using Pandora.Scripts.Enemy;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pandora.Scripts.Player.Controller
{
    public class RangedPlayerController : PlayerController
    {
        public GameObject projectile;
        
        // variables for the projectile
        public float projectileSpeed;
        private float _projectileRange = 1f;

        private const float AttackRangeMagnitude = 1f;

        public override void Start()
        {
            base.Start();
            // DEBUG : 플레이어 스텟의 사거리를 3배로 설정
            // 후에는 원거리 플레이너캐릭터는 기본 사거리가 3이되도록 해야함
            _projectileRange = playerCurrentStat.AttackRange;
        }
        public override void AttackRangeChanged(float newRange)
        {
            base.AttackRangeChanged(newRange);
            if(projectile != null)
            {
                _projectileRange = newRange * AttackRangeMagnitude;
            }
            else
            {
                Debug.LogError("RangePlayer's Projectile is null");
            }
        }
        
        // 공격 코루틴
        protected override IEnumerator AttackCoroutine(HitParams hitParams)
        {
            // 투사체 생성
            GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
            //
            yield return null;
            var pj = projectileInstance.GetComponent<Projectile>();
            pj.maxDistance = _projectileRange * AttackRangeMagnitude;
            pj.SetDirection(attackDir, projectileSpeed);
            pj.SetHitParams(hitParams);
        }
    }
}