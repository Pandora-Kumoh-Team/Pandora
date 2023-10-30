using Pandora.Scripts.System;
using UnityEngine;

namespace Pandora.Scripts.UI
{
    public class InGameCanvasManager : MonoBehaviour
    {
        public void OnPause()
        {
            var pausePanel = transform.Find("PauseMenu").gameObject;
            var isMenuActive = pausePanel.activeSelf;
            pausePanel.SetActive(!isMenuActive);
            Time.timeScale = isMenuActive ? 1 : 0;
        }
        
        public void ExitGame()
        {
            GameManager.ExitGame();
        }
    }
}