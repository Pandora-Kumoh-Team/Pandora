using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pandora.Scripts.Player
{
    public class RangedPlayerController : PlayerController
    {
        public GameObject projectile;
        
        // variables for the projectile
        public float projectileSpeed;

        public override void Start()
        {
            base.Start();
        }

        // 공격 코루틴
        protected override IEnumerator AttackCoroutine()
        {
            // 투사체 생성
            GameObject projectileInstance = Instantiate(projectile, transform.position, Quaternion.identity);
            //
            yield return null;
            projectileInstance.GetComponent<Projectile>().SetDirection(attackDir, projectileSpeed);
        }
    }
}