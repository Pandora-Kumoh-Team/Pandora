using Pandora.Scripts.Player.Skill.Data;
using UnityEngine;

namespace Pandora.Scripts.Player.Skill
{
    public class Skill : MonoBehaviour
    {
        public SkillData skillData;
        
        public void Init(SkillData skillData)
        {
            this.skillData = skillData;
        }
    }
}