using System.Collections.Generic;
using Pandora.Scripts.Enemy;
using UnityEngine;

namespace Pandora.Scripts.Player
{
    public class Projectile : MonoBehaviour
    {
        // Components
        private Rigidbody2D _rigidbody2D;
        private GameObject _target;
        
        // Variables
        private float _speed;
        private float _damage = 10f; // 임시 데메지
        private float _lifeTime;
        public float maxDistance;
        private float _movedDistance;
        private Vector2 _beforePosition;
        private List<Buff> _buffs;

        public void SetDirection(Vector2 direction, float speed)
        {
            _rigidbody2D.velocity = direction.normalized * speed;
        }
        
        
        public void SetDamage(float damage)
        {
            _damage = damage;
        }
        
        public void SetBuffs(List<Buff> buffs)
        {
            _buffs = buffs;
        }
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _lifeTime = 5f;
            _beforePosition = transform.position;
        }

        private void Update()
        {
            // 유도시 방향 전환
            if (_target != null)
            {
                // change direction to target slowly
                var direction = _target.transform.position - transform.position;
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.1f);
            }
            
            // 사거리 벗어나면 삭제
            _movedDistance += Vector2.Distance(_beforePosition, transform.position);
            _beforePosition = transform.position;
            if (_movedDistance > maxDistance)
            {
                Destroy(gameObject);
            }
            
            // life time
            _lifeTime -= Time.deltaTime;
            if (_lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var hitAble = col.gameObject.GetComponent<IHitAble>();
            if (hitAble != null)
            {
                hitAble.Hit(_damage, _buffs);
                Destroy(gameObject);
            }
        }

    }
}