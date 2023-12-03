using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideAttack : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(GameObject.FindGameObjectWithTag("Player"))) //범위 내에 Player가 존재한다면
        {
            transform.parent.GetComponent<ShadowBossController>().SideAttackCollisionEvent();
            Debug.Log("상태 변환: SideAttack -> Follow");
            transform.parent.GetComponent<ShadowBossController>().anim.SetBool("isFollow", true);
            gameObject.SetActive(false);
        }
    }
}
