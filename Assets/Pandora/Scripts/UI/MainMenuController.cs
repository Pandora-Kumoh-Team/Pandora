using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pandora.Scripts.UI
{
    public class MainMenuController : MonoBehaviour
    {
        public void StageStart()
        {
            // TODO : 플레이어 영구 스텟 반영하여 destroy로 생성
            // 현재는 stage1 씬에 플레이어 오브젝트가 추가되어 있음
            SceneManager.LoadScene("Stage1");
        }

        public void Credit()
        {
            // TODO
            // SceneManager.LoadScene("Credit");
        }
        
        public void GameExit()
        {
            // TODO : JSON 저장
            Application.Quit();
        }
    }
}