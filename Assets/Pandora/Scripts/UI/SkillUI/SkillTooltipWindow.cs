using TMPro;
using UnityEngine;

namespace Pandora.Scripts.UI.SkillUI
{
    public class SkillTooltipWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text headerText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private RectTransform windowTransform;
        
        private void Start()
        {
            HideTooltip();
        }

        public void ShowTooltip(string header, string description, Color gradeColor, float positionX)
        {
            headerText.text = header;
            headerText.color = gradeColor;
            descriptionText.text = description;
            descriptionText.color = Color.white;
            var position = windowTransform.position;
            windowTransform.position = new Vector3(positionX, position.y, position.z);
            gameObject.SetActive(true);
        }

        public void HideTooltip()
        {
            gameObject.SetActive(false);
        }
    }
}