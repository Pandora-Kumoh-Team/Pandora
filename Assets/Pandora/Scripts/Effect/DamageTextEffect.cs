using System;
using TMPro;
using UnityEngine;

namespace Pandora.Scripts.Effect
{
    public class DamageTextEffect : FadeTextEffect
    {
        private void OnTriggerStay2D(Collider2D other)
        {
            var otherDamageEffect = other.GetComponent<DamageTextEffect>();
            if (otherDamageEffect != null)
            {
                // 둘 중 하나만 제거해야 하기 때문에 둘 중 생긴지 오래된 것을 제거한다
                if (fadeTimer < otherDamageEffect.fadeTimer) return;
                
                var otherDamage = int.Parse(otherDamageEffect.GetComponent<TextMeshPro>().text);
                var thisDamage = int.Parse(GetComponent<TextMeshPro>().text);
                GetComponent<TextMeshPro>().text = (otherDamage + thisDamage).ToString();
                fadeTimer = 0;
                Destroy(otherDamageEffect.gameObject);
            }
        }
    }
}