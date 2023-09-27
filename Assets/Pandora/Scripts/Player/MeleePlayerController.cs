using System.Collections;
using UnityEngine;

namespace Pandora.Scripts.Player
{
    public class MeleePlayerController : PlayerController
    {
        private GameObject attackRange;
        
        public override void Start()
        {
            base.Start();
            attackRange = transform.Find("AttackRange").gameObject;
        }
        
        // 공격 코루틴
        protected override IEnumerator AttackCoroutine()
        {
            // 공격 방향 수정
            var zAngle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg;
            attackRange.transform.rotation = Quaternion.Euler(0, 0, zAngle);
            
            // 공격 활성화
            attackRange.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            // 공격 비활성화
            attackRange.SetActive(false);
        }
    }
}