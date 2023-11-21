using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pandora.Scripts.Player.Skill.Data
{
    [CreateAssetMenu(fileName = "SkillList", menuName = "Scriptable Object Asset/SkillList")]
    public class SkillList : ScriptableObject
    {
        public List<GameObject> skillPrefabList;
    }
}
