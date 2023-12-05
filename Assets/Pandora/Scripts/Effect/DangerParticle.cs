using System.Collections;
using System.Collections.Generic;
using Pandora.Scripts.Player.Controller;
using UnityEngine;

namespace Pandora.Scripts.Effect
{
    [RequireComponent(typeof(ParticleSystem))]
    public class DangerParticle : MonoBehaviour
    {
        private ParticleSystem _particleSystem;

        public float damage;
        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            StartCoroutine(AfterAwake());
        }
        
        // Awake 호출 이후에 호출될 코루틴
        private IEnumerator AfterAwake()
        {
            yield return null;
            _particleSystem.trigger.SetCollider(0, PlayerManager.Instance.GetPlayer(0).GetComponent<CircleCollider2D>());
            _particleSystem.trigger.SetCollider(1, PlayerManager.Instance.GetPlayer(1).GetComponent<CircleCollider2D>());
        }

        public void OnParticleTrigger()
        {
            List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
            int numEnter = _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
            for (int i = 0; i < numEnter; i++)
            {
                ParticleSystem.Particle p = enter[i];
                var players = PlayerManager.Instance.GetPlayers();
                var playerColliders = new CircleCollider2D[2];
                playerColliders[0] = players[0].GetComponent<CircleCollider2D>();
                playerColliders[1] = players[1].GetComponent<CircleCollider2D>();
                foreach (var co in playerColliders)
                {
                    if ((co.transform.position - p.position).magnitude < _particleSystem.trigger.radiusScale + co.radius)
                        co.GetComponent<PlayerController>().Hurt(damage, null, gameObject);
                }
            }
        }
    }
}