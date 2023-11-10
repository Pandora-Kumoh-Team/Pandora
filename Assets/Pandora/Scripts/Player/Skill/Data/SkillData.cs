using UnityEngine;

namespace Pandora.Scripts.Player.Skill.Data
{
    [global::System.Serializable]
    public class SkillData
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

        public SkillData(int id, Sprite icon, string name, SkillGrade grade, SkillType type, string description)
        {
            _id = id;
            _icon = icon;
            _grade = grade;
            _name = name;
            _type = type;
            _description = description;
        }
    }
}
