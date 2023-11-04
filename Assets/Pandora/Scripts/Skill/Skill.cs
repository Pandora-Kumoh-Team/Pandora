using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public int _id;
    public Sprite _icon;
    public string _name;
    public string _grade; // 0 = Normal, 1 = Rare, 2 = Unique , 3 = legendary
    public bool _isActive;
    public string _func; // 스킬 설명

    public Skill(int id, Sprite icon, string name,string grade, bool isActive, string func)
    {
        _id = id;
        _icon = icon;
        _grade = grade;
        _name = name;
        _isActive = isActive;
        _func = func;
    }
}
