using System;
using TMPro;
using UnityEngine;

namespace Pandora.Scripts.Effect
{
    public class DamageTextEffect : FadeTextEffect
    {
        private float _damage;
        
        public void Init(string text, Color color, float speed, float fadeTime, float moveTime, Vector3 moveDirection, float damage)
        {
            base.Init(text, color, speed, fadeTime, moveTime, moveDirection);
            _damage = damage;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            var otherDamageEffect = other.GetComponent<DamageTextEffect>();
            if (otherDamageEffect != null && transform.parent == otherDamageEffect.transform.parent)
            {
                // 둘 중 하나만 제거해야 하기 때문에 둘 중 생긴지 오래된 것을 제거한다
                if (fadeTimer > otherDamageEffect.fadeTimer) return;
                
                // 크리티컬 텍스트 뒤에 붙는 느낌표를 제거한다
                GetComponent<TextMeshPro>().text = (otherDamageEffect._damage + _damage).ToString();
                fadeTimer = 0;
                Destroy(otherDamageEffect.gameObject);
            }
        }
    }
}