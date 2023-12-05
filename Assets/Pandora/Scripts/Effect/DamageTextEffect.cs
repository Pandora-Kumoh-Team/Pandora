using System;
using Pandora.Scripts.Enemy;
using TMPro;
using UnityEngine;

namespace Pandora.Scripts.Effect
{
    public class DamageTextEffect : FadeTextEffect
    {
        private float _damage;
        private bool _isCritical;
        
        public void Init(string text, Color color, float speed, float fadeTime, float moveTime, Vector3 moveDirection, HitParams param)
        {
            base.Init(text, color, speed, fadeTime, moveTime, moveDirection);
            // 소숫점 반올림
            _damage = param.damage;
            _isCritical = param.isCritical;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            var otherDamageEffect = other.GetComponent<DamageTextEffect>();
            if (otherDamageEffect != null && transform.parent == otherDamageEffect.transform.parent)
            {
                // 둘 중 하나만 제거해야 하기 때문에 둘 중 생긴지 오래된 것을 제거한다
                if (fadeTimer > otherDamageEffect.fadeTimer) return;
                GetComponent<TextMeshPro>().text = Math.Round(otherDamageEffect._damage + _damage) + (_isCritical ? "!" : "");
                fadeTimer = 0;
                Destroy(otherDamageEffect.gameObject);
            }
        }
    }
}