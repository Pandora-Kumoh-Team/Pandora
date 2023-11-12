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
        public string description;
        public float cooldown;
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
            return base.Equals(other) && id == other.id && Equals(icon, other.icon) && name == other.name && grade == other.grade && type == other.type && description == other.description && cooldown.Equals(other.cooldown) && duration.Equals(other.duration) && Equals(ownerPlayer, other.ownerPlayer);
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
            var hashCode = new HashCode();
            hashCode.Add(base.GetHashCode());
            hashCode.Add(id);
            hashCode.Add(icon);
            hashCode.Add(name);
            hashCode.Add((int)grade);
            hashCode.Add((int)type);
            hashCode.Add(description);
            hashCode.Add(cooldown);
            hashCode.Add(duration);
            hashCode.Add(ownerPlayer);
            return hashCode.ToHashCode();
        }

    }
}