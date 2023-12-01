using UnityEngine;

namespace Pandora.Scripts.NewDungeon
{
    public class ShopController : MonoBehaviour
    {
        public RectTransform uiGroup;
        public RectTransform text;
        global::Player enterPlayer;

        public void Enter(global::Player player)
        {
            enterPlayer = player;
            uiGroup.anchoredPosition = Vector3.zero;
            text.anchoredPosition = Vector3.down * 1200;
        }

        public void Exit()
        {
            uiGroup.anchoredPosition = Vector3.down * 1200;
            text.anchoredPosition = Vector3.zero;
        }
    }
}
