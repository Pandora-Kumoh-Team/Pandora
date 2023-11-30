using Pandora.Scripts.Player;
using Pandora.Scripts.Player.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Player : MonoBehaviour
{
    // TODO
    // 기존의 PlayerContoller에 아래 코드 수정하여 추가
    // 아이템 구입 시 플레이어의 체력, 스킬, 스텟 처리

    float h;
    float v;

    bool iDown;

    Vector3 dir;

    public float speed;
    GameObject nearObject;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Interation();
    }

    void GetInput()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        //Project Setting -> Input에 Iteration 추가해서 사용해야함(left shift)
        iDown = Input.GetButtonDown("Iteration");
    }

    void Move()
    {
        dir = new Vector3(h, v, 0).normalized;

        transform.position += dir * speed * Time.deltaTime;
    }

    void Interation()
    {
        if(iDown && nearObject != null)
        {
            if (nearObject.tag == "Shop")
            {
                ShopController shop = nearObject.GetComponent<ShopController>();
                shop.Enter(this);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Shop")
        {
            nearObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Shop")
        {
            ShopController shop = nearObject.GetComponent<ShopController>();
            shop.Exit();
            nearObject = null;
        }
    }
}
