using Pandora.Scripts.Player.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    //Component
    private float knifeDamage = 20f;

    void Start()
    {
               
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Player와 부딫히거나 벽과 부딫히면 삭제
        
    }
}
