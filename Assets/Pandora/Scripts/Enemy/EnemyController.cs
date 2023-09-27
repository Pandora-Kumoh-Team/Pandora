using System.Collections.Generic;
using Pandora.Scripts.Effect;
using UnityEngine;

namespace Pandora.Scripts.Enemy
{
    public class EnemyController : MonoBehaviour, IHitAble
    {
        // Components
        private Rigidbody2D rb;
        private Animator anim;
        private CapsuleCollider2D capsuleCollider;

        // Animator Hashes
        private static readonly int Hit1 = Animator.StringToHash("Hit");

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void Hit(float damage, List<Buff> buff)
        {
            anim.SetTrigger(Hit1);
            
            // damage 이펙트 출력
            var position = transform.position + new Vector3(0, capsuleCollider.size.y / 2, 0);
            var damageEffect = Instantiate(GameManager.Instance.damageEffect, position, Quaternion.identity);
            damageEffect.GetComponent<FadeTextEffect>()
                .Init(damage.ToString(), Color.white, 1f, 0.5f, 0.05f, Vector3.up);
        }
    }
}
