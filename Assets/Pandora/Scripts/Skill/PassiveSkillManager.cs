using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillManager : MonoBehaviour
{
    public void Apply(int id)
    {
        switch (id)
        {
            case 000:
                Debug.Log("���ݷ� ��ȭ �нú� ȹ��");// TEST
                GameObject.Find("PlayerCharacterMelee").GetComponent<MeleePlayerController>()._playerStat.AttackPower *= 1.1f;
                GameObject.Find("PlayerCharacterRanged").GetComponent<RangedPlayerController>()._playerStat.AttackPower *= 1.1f;
                Debug.Log("���� ���ݷ�" + GameObject.Find("PlayerCharacterMelee").GetComponent<MeleePlayerController>()._playerStat.AttackPower);// TEST
                break;
        }
    }
}
