using Pandora.Scripts.Player;
using Pandora.Scripts;
using Pandora.Scripts.System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Pandora.Scripts.Player.Skill;
using Pandora.Scripts.Player.Skill.Data;

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
    }

    private void Awake()
    {
        ownSkillList = new List<bool>();
        skillList = ScriptableObject.CreateInstance<SkillList>();
    }

    public void ObtainSkill(int index)
    {
        ownSkillList[index] = true;
        if (skillList.skillList[index]._type == SkillData.SkillType.Passive)
        {

        }
        else
        {

        }
    }
}
