using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pandora.Scripts.Player.Controller
{
    public class RangedPlayerController : PlayerController
    {
        public GameObject projectile;
        
        // variables for the projectile
        public float projectileSpeed;

        public override void AttackRangeChanged(float newRange)
        {
            base.AttackRangeChanged(newRange);
            if(projectile != null)
                projectile.GetComponent<Projectile>().maxDistance = newRange * 5;
        }
        
        // 공격 코루틴
        protected override IEnumerator AttackCoroutine(float damage, List<Buff> buffs)
        {
            // 투사체 생성
            GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
            //
            yield return null;
            var pj = projectileInstance.GetComponent<Projectile>();
            pj.SetDirection(attackDir, projectileSpeed);
            pj.SetDamage(damage);
            pj.SetBuffs(buffs);
        }
    }
}