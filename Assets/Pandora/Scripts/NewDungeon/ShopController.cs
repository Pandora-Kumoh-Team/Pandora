using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public RectTransform uiGroup;
    public RectTransform text;
    Player enterPlayer;

    public void Enter(Player player)
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
