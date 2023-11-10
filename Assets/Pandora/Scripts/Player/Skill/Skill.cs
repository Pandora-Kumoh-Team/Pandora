using System;
using System.Collections;
using Pandora.Scripts.Player.Skill.Data;
using UnityEngine;

namespace Pandora.Scripts.Player.Skill
{
    [global::System.Serializable]
    public class Skill : MonoBehaviour
    {
        public enum SkillGrade
        {
            Normal,
            Rare,
            Unique,
            Legendary
        }

        public enum SkillType
        {
            Active,
            Passive
        }
    
        public int _id;
        public Sprite _icon;
        public string _name;
        public SkillGrade _grade;
        public SkillType _type;
        public string _description;
        public float _cooldown;
        public GameObject _ownerPlayer;
        public float _duration;
    }
}