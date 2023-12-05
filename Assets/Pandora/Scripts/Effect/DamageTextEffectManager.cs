using System;
using Pandora.Scripts.Effect;
using UnityEngine;

namespace Pandora.Scripts.Enemy
{
    public class DamageTextEffectManager : MonoBehaviour
    {
        // singleton
        public static DamageTextEffectManager Instance { get; private set; }

        public GameObject damageEffectPrefab;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }
        
        public void SpawnDamageTextEffect(Vector3 relativePosition, GameObject attackedObject, HitParams hitParams)
        {
            var position = attackedObject.transform.position + relativePosition;
            var damageEffect = Instantiate(damageEffectPrefab, position, Quaternion.identity, attackedObject.transform);
            string text;
            Color textColor;
            if (hitParams.damage > 0)
            {
                text = Math.Round(hitParams.damage) + (hitParams.isCritical ? "!" : "");
                textColor = hitParams.isCritical ? Color.yellow : Color.white;
            }
            else if (hitParams.damage < 0)
            {
                text = Math.Round(-hitParams.damage).ToString();
                textColor = Color.green;
            }
            else
            {
                text = "Miss";
                textColor = Color.gray;
            }
            damageEffect.GetComponent<DamageTextEffect>().Init(text, textColor, 1f, 0.5f, 0.05f, Vector3.up, hitParams);
        }
    }
}