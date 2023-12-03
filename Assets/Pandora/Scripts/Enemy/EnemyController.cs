using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Pandora.Scripts.Effect;
using Pandora.Scripts.Player.Controller;
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
        
        public GameObject hitParticle;
        public AudioClip[] hitSounds;
        public float hitSoundVolume = 0.5f;
        private AudioSource audioSource;
        public int difficulty = 1;


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

            audioSource = gameObject.GetComponent<AudioSource>();
            if (hitSounds.Length == 0)
            {
                Debug.LogError(gameObject.name + " : hitSounds is empty");
            }
        }


        public void Hit(HitParams hitParams)
        {
            var damage = hitParams.damage;
            anim.SetTrigger(Hit1);
            
            // damage 이펙트 출력
            var relativePos = new Vector3(0, capsuleCollider.size.y / 2, 0);
            DamageTextEffectManager.Instance.SpawnDamageTextEffect(relativePos, gameObject, hitParams);

            // 파티클 이펙트 출력
            if (hitParticle == null)
                hitParticle = GameManager.Instance.bloodParticle;
            var bloodEffect = Instantiate(hitParticle, transform.position, Quaternion.identity);
            // 공격자의 반대 방향으로 피격 이펙트 회전
            float z;
            if(hitParams.attacker != null)
                z = transform.position.x > hitParams.attacker.transform.position.x ? -45 : 45;
            else
                z = 0;
            bloodEffect.transform.rotation = Quaternion.Euler(0, 0, z);
            Destroy(bloodEffect, 1f);

            // 사운드 출력
            audioSource.PlayOneShot(hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)], hitSoundVolume);
            
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
