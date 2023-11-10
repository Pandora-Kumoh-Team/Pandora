using System.Collections.Generic;
using Pandora.Scripts.Player.Skill.Data;
using UnityEngine;

namespace Pandora.Scripts.Player.Skill
{
    public class SkillManager : MonoBehaviour
    {
        // Singleton class
        public static SkillManager Instance { get; private set; }

        public SkillList passiveSkillList;
        public SkillList activeSkillList;

        private void Awake()
        {
            // singleton
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            if (passiveSkillList == null || activeSkillList == null)
            {
                Debug.LogError("스킬매니저에 스킬 리스트 등록 안됨");
            }
        }
        
        public PassiveSkill GetRandomPassiveSkill(SkillList nowPassiveSkillList)
        {
            // get random skill from passive skill list except now skill list
            var skillList = new List<Skill>();
            foreach (var skill in passiveSkillList.skillList)
            {
                if (!nowPassiveSkillList.skillList.Contains(skill))
                {
                    skillList.Add(skill);
                }
            }
            return (PassiveSkill)skillList[Random.Range(0, skillList.Count)];
        }
        
        public ActiveSkill GetRandomActiveSkill(SkillList nowActiveSkillList)
        {
            // get random skill from active skill list except now skill list
            var skillList = new List<Skill>();
            foreach (var skill in activeSkillList.skillList)
            {
                if (!nowActiveSkillList.skillList.Contains(skill))
                {
                    skillList.Add(skill);
                }
            }
            return (ActiveSkill)skillList[Random.Range(0, skillList.Count)];
        }
    }
}
