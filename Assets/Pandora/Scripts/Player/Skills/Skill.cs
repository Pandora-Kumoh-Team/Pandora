using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pandora.Scripts.Player.Skill
{
    public abstract class Skill : MonoBehaviour
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
    
        public int id;
        public Sprite icon;
        public string name;
        public SkillGrade grade;
        public SkillType type;
        [TextArea]
        public string description;
        public float coolTime;
        public float duration;
        
        [HideInInspector]
        public GameObject ownerPlayer;
        
        public static bool operator ==(Skill a, Skill b)
        {
            return a!.id == b!.id;
        }

        public static bool operator !=(Skill a, Skill b)
        {
            return a!.id != b!.id;
        }
        protected bool Equals(Skill other)
        {
            return base.Equals(other) && id == other.id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Skill)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), id);
        }

    }
}