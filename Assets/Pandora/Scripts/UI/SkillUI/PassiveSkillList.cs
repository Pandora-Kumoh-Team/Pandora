using System.Collections.Generic;
using Pandora.Scripts.Player.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Pandora.Scripts.UI.SkillUI
{
    public class PassiveSkillList : MonoBehaviour
    {
        public int playerNum;
        public GameObject SkillPrefab;

        private void OnEnable()
        {
            var passiveSkills = PlayerManager.Instance.GetPlayer(playerNum)
                .GetComponent<PlayerController>().GetPassiveSkills();
            
            // Delete all children
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            var skills = new List<GameObject>();
            // Instantiate new children and set y position index * 100
            for (var i = 0; i < passiveSkills.Length; i++)
            {
                skills.Add(Instantiate(SkillPrefab, transform));
                skills[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -i * 100);
                skills[i].GetComponent<SkillInfoUi>().Init(passiveSkills[i], playerNum);
            }
            
            // Set color
            foreach (var skillObject in skills)
            {
                if (playerNum == 0)
                {
                    skillObject.transform.Find("Image").GetComponent<Image>().color = Color.red;
                    skillObject.transform.Find("SkillSlot").GetComponent<Image>().color = Color.red;
                }
                else
                {
                    skillObject.transform.Find("Image").GetComponent<Image>().color = Color.blue;
                    skillObject.transform.Find("SkillSlot").GetComponent<Image>().color = Color.blue;
                }
            }
        }
    }
}