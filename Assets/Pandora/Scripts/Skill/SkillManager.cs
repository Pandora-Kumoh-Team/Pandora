using Pandora.Scripts.Player;
using Pandora.Scripts;
using Pandora.Scripts.System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class SkillManager : MonoBehaviour
{
    //���� ��ų ����
    //��ųid == skillList�ε���
    //true�Ͻ� ��ų ȹ��
    public List<bool> ownSkillList;
    private readonly int skillNum = 100;
    private SkillList skillList;

    private void Start()
    {
        StartCoroutine(TestCoroutine()); //TEST
    }
    private void Awake()
    {
        ownSkillList = new List<bool>();
        skillList = new SkillList();
    }

    public void ObtainSkill(int index)
    {
        ownSkillList[index] = true;
        if (skillList.GetisActive(index))
        {
            //Active
        }
        else
        {
            GameManager.Instance.passiveSkillManager.Apply(index);
        }
    }


    
    public IEnumerator TestCoroutine() // TEST
    {
        while(true)
        {
            yield return new WaitForSeconds(5f);
            GameManager.Instance.passiveSkillManager.Apply(0);
        }
    }
}
