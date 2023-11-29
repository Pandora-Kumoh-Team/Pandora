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
        //Project Setting -> Input에 Iteration 추가해서 사용해야함
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
