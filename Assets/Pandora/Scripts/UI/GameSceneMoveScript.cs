using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneMoveScript : MonoBehaviour
{
    public void GameScenesCtrl()
    {
        SceneManager.LoadScene("Stage1Scene");
    }
    
}
