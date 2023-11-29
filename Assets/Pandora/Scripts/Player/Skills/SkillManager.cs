using System.Collections;
using System.Collections.Generic;
using Pandora.Scripts.Player.Controller;
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
            else
            {
                // instantiate ScriptableObject skill list
                passiveSkillList = Instantiate(passiveSkillList);
                activeSkillList = Instantiate(activeSkillList);
            }
        }
        
        public List<GameObject> GetRandomPassiveSkills(int playerNum, int count)
        {
            // get random skill from passive skill list except now skill list
            var nowPassiveSkillList = PlayerManager.Instance.GetPlayer(playerNum).GetComponent<PlayerController>().GetPassiveSkills();
            return GetRandomSkills(count, nowPassiveSkillList, passiveSkillList.skillPrefabList);
        }
        
        public List<GameObject> GetRandomActiveSkills(int playerNum, int count)
        {
            // get random skill from passive skill list except now skill list
            var nowActiveSkillList = PlayerManager.Instance.GetPlayer(playerNum).GetComponent<PlayerController>().GetActiveSkills();
            return GetRandomSkills(count, nowActiveSkillList, activeSkillList.skillPrefabList);
        }

        private List<GameObject> GetRandomSkills(int count, IEnumerable<GameObject> nowSkillList, IEnumerable<GameObject> skillPrefabList)
        {
            var result = new List<GameObject>();
            var ableSkillList = new List<GameObject>();
            // prefab list to skill list
            foreach (var skill in skillPrefabList)
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
    }
}
