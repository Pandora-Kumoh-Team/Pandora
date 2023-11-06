using Pandora.Scripts.System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        public void ReStart()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }
        
        public void ExitGame()
        {
            GameManager.ExitGame();
        }
    }
}