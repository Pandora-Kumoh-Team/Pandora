using Pandora.Scripts.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    private GameObject target;
    private void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == target) {
            transform.parent.GetComponent<FirstBossController>().canAttack = true;
        }
    }
}
