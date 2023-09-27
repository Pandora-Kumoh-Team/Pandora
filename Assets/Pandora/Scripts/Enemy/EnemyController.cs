using UnityEngine;

namespace Pandora.Scripts.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        // Components
        private Rigidbody2D rb;
        private Animator anim;
    
        // Animator Hashes
        private static readonly int Hit1 = Animator.StringToHash("Hit");
    
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }


        public void Hit(float damage, Buff buff)
        {
            anim.SetTrigger(Hit1);
        }
    }
}
