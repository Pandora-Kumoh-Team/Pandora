using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Pandora.Scripts.Effect;
using Pandora.Scripts.System;
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

        //Status
        public EnemyStatus _enemyStatus;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider2D>();

            //오브젝트 풀링으로 인한 clone 제거
            if(this.gameObject.name.Contains("(Clone)"))
                _enemyStatus = new EnemyStatus(this.gameObject.name.Replace("(Clone)",""));
            else
                _enemyStatus = new EnemyStatus(this.gameObject.name);
        }

        // Update is called once per frame
        void Update()
        {
            rb.velocity = Vector3.zero; // 밀림 방지
        }


        public void Hit(float damage, List<Buff> buff)
        {
            anim.SetTrigger(Hit1);
            
            // damage 이펙트 출력
            var position = transform.position + new Vector3(0, capsuleCollider.size.y / 2, 0);
            var damageEffect = Instantiate(GameManager.Instance.damageEffect, position, Quaternion.identity, transform);
            damageEffect.GetComponent<FadeTextEffect>()
                .Init(damage.ToString(), Color.white, 1f, 0.5f, 0.05f, Vector3.up);

            //피해 계산
            _enemyStatus.NowHealth -= damage;

            //hp 0에 도달 시 비활성화
            if (_enemyStatus.NowHealth <=0)
                gameObject.SetActive(false);
        }
        
        //스포너에 의해 활성화 될 시 스탯 초기화
        private void OnEnable()
        {
            if (this.gameObject.name.Contains("(Clone)"))
                _enemyStatus = new EnemyStatus(this.gameObject.name.Replace("(Clone)", ""));
            else
                _enemyStatus = new EnemyStatus(this.gameObject.name);
        }

        private void OnDisable()
        {
            // 출력된 이펙트 제거
            foreach (Transform child in transform)
            {
                if(child.gameObject.GetComponent<FadeTextEffect>() != null)
                    Destroy(child.gameObject);
            }
        }
    }
}
