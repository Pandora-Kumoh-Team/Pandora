using Pandora.Scripts.Player.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    //Component
    private float knifeDamage = 20f;
    public GameObject boss;

    void Start()
    {
               
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() == null) return;
        collision.gameObject.GetComponent<PlayerController>().Hurt(knifeDamage, null, boss);
        Destroy(gameObject);
    }
}
