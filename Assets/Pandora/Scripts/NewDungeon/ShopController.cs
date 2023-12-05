using System;
using Pandora.Scripts.Player.Controller;
using UnityEngine;

namespace Pandora.Scripts.NewDungeon
{
    public class ShopController : MonoBehaviour
    {
        public RectTransform uiGroup;
        public RectTransform text;
        public bool isPlayerOnTrigger;

        private void Update()
        {
            // move ui from under screen to above screen
            // if (isPlayerOnTrigger)
            // {
            //     if (uiGroup.anchoredPosition.y < 0)
            //     {
            //         uiGroup.anchoredPosition += new Vector2(0, 10);
            //     }
            // }
            // else
            // {
            //     if (uiGroup.anchoredPosition.y > -1200)
            //     {
            //         uiGroup.anchoredPosition -= new Vector2(0, 10);
            //     }
            // }
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                if (col.GetComponent<PlayerController>().isControlByPlayer)
                {
                    isPlayerOnTrigger = true;
                    return;
                }
            }
            isPlayerOnTrigger = false;
        }
    }
}
