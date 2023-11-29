using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public RectTransform uiGroup;
    GameObject enterPlayer;

    public void Enter(GameObject player)
    {
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector3.zero;
    }

    public void Exit()
    {
        uiGroup.anchoredPosition = Vector3.down * 1200;
    }
}
