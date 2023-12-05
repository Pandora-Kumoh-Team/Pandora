using Pandora.Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondAttackRange : MonoBehaviour
{
    private GameObject target;
    private void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            transform.parent.GetComponent<SecondBossController>().canAttack = true;
        }
    }
}
