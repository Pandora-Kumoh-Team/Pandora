using System.Collections;
using System.Collections.Generic;
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

        private const float AttackRangeMagnitude = 3f;

        public override void Start()
        {
            base.Start();
            _projectileRange = _playerStat.AttackRange;
        }
        public override void AttackRangeChanged(float newRange)
        {
            base.AttackRangeChanged(newRange);
            if(projectile != null)
            {
                _projectileRange = newRange * AttackRangeMagnitude;
                Debug.Log(newRange * AttackRangeMagnitude);
            }
            else
            {
                Debug.LogError("RangePlayer's Projectile is null");
            }
        }
        
        // 공격 코루틴
        protected override IEnumerator AttackCoroutine(float damage, List<Buff> buffs)
        {
            // 투사체 생성
            GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
            //
            yield return null;
            var pj = projectileInstance.GetComponent<Projectile>();
            pj.maxDistance = _projectileRange * AttackRangeMagnitude;
            pj.SetDirection(attackDir, projectileSpeed);
            pj.SetDamage(damage);
            pj.SetBuffs(buffs);
        }
    }
}