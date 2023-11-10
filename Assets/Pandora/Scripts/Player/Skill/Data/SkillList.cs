using UnityEngine;

namespace Pandora.Scripts.Player.Skill.Data
{
    [CreateAssetMenu(fileName = "SkillList", menuName = "Scriptable Object Asset/SkillList")]
    public class SkillList : ScriptableObject
    {
        private readonly int skillNum = 100;
        public SkillData[] skillList;
    }
}
