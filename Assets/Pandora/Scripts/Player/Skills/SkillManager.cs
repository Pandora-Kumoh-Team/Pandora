using System;
using System.Collections;
using System.Collections.Generic;
using Pandora.Scripts.Player.Controller;
using Pandora.Scripts.Player.Skill.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pandora.Scripts.Player.Skill
{
    public class SkillManager : MonoBehaviour
    {
        // Singleton class
        public static SkillManager Instance { get; private set; }

        public SkillList passiveSkillList;
        public SkillList activeSkillList;
        private SkillList p0passiveSkillList;
        private SkillList p0activeSkillList;
        private SkillList p1passiveSkillList;
        private SkillList p1activeSkillList;

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
            else
            {
                // instantiate ScriptableObject skill list
                p0passiveSkillList = Instantiate(passiveSkillList);
                p0activeSkillList = Instantiate(activeSkillList);
                p1passiveSkillList = Instantiate(passiveSkillList);
                p1activeSkillList = Instantiate(activeSkillList);
            }
        }
        
        public List<GameObject> GetRandomSkills(int playerNum, Skill.SkillType type, int count)
        {
            // get random skill from passive skill list except now skill list
            var skillObjectList = playerNum switch
            {
                0 when type == Skill.SkillType.Active => p0activeSkillList.skillPrefabList,
                0 when type == Skill.SkillType.Passive => p0passiveSkillList.skillPrefabList,
                1 when type == Skill.SkillType.Active => p1activeSkillList.skillPrefabList,
                _ => p1passiveSkillList.skillPrefabList
            };
            var result = new List<GameObject>();
            var ableSkillList = new List<GameObject>();
            // prefab list to skill list
            foreach (var skill in skillObjectList)
            {
                ableSkillList.Add(skill);
            }
            for (var i = 0; i < count; i++)
            {
                if(ableSkillList.Count == 0) break;
                var randomIndex = Random.Range(0, ableSkillList.Count);
                result.Add(ableSkillList[randomIndex]);
                ableSkillList.RemoveAt(randomIndex);
            }
            return result;
        }

        public void RemoveSkillAtList(int playerNum, Skill.SkillType type, GameObject skillObject)
        {
            if(playerNum == 0 && type == Skill.SkillType.Active)
                p0activeSkillList.skillPrefabList.Remove(skillObject);
            else if (playerNum == 0 && type == Skill.SkillType.Passive)
                p0passiveSkillList.skillPrefabList.Remove(skillObject);
            else if (playerNum == 1 && type == Skill.SkillType.Active)
                p1activeSkillList.skillPrefabList.Remove(skillObject);
            else if (playerNum == 1 && type == Skill.SkillType.Passive)
                p1passiveSkillList.skillPrefabList.Remove(skillObject);
        }
        
        public static Color SKillGradeToColor(Skill.SkillGrade grade)
        {
            return grade switch
            {
                Skill.SkillGrade.Normal => new Color(1f, 1f, 1f),
                Skill.SkillGrade.Rare => new Color(0.4f, 1f, 0.4f),
                Skill.SkillGrade.Unique => new Color(0.4f, 0.4f, 1f),
                Skill.SkillGrade.Legendary => new Color(1f, 1f, 0.4f),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
