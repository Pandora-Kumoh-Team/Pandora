using Pandora.Scripts.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    float timer;

    private void Awake()
    {
        timer = 0;
    }

    //임시 : 최대 5마리까지 5초마다 고블린 스폰
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 5 && GameManager.Instance.pool.numObject <5)
        {
            GameManager.Instance.pool.Get(1);

            timer = 0;
        }
    }
}
