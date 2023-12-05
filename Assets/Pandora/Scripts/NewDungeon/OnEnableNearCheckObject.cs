using Pandora.Scripts.Player.Controller;
using UnityEngine;

namespace Pandora.Scripts.NewDungeon
{
    /// <summary>
    /// 플레이어 근처에서 활성화 되어 바로 Trigger or Collision 하지 않게 하기 위한 스크립트
    /// </summary>
    public class OnEnableNearPlayerCheckObject : MonoBehaviour
    {
        protected bool IsPlayerStillNearAfterEnable;
        protected float DistanceToPlayer = 3f;

        protected virtual void OnEnable()
        {
            IsPlayerStillNearAfterEnable = IsPlayerNear();
            if(IsPlayerStillNearAfterEnable)
                OnEnableNearPlayer();
        }
        
        private void OnDisable()
        {
            IsPlayerStillNearAfterEnable = false;
        }
        
        private void Update()
        {
            // check if player is near
            if (!IsPlayerStillNearAfterEnable) return;
            if (!IsPlayerNear()) OnPlayerGetDistance();
        }
        
        protected virtual void OnEnableNearPlayer()
        {
            // override this method
        }
        
        protected virtual void OnPlayerGetDistance()
        {
            IsPlayerStillNearAfterEnable = false;
        }

        private bool IsPlayerNear()
        {
            var players = PlayerManager.Instance.GetPlayers();
            foreach (var player in players)
            {
                if (!player.GetComponent<PlayerController>().isControlByPlayer) continue;
                if (Vector2.Distance(transform.position, player.transform.position) < DistanceToPlayer)
                    return true;
            }
            return false;
        }

    }
}