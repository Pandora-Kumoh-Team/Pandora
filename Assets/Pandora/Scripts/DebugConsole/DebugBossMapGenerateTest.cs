using System.Collections.Generic;
using System.IO;
using Pandora.Scripts.NewDungeon;
using Pandora.Scripts.System.Event;
using UnityEngine;
using UnityEngine.SceneManagement;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.DebugConsole
{
    public class DebugBossMapGenerateTest : MonoBehaviour, IEventListener
    {
        int generateTimes = 0;
        public string errorSavePath = "Assets/Pandora/MapTestErrorData.txt";
        public string noErrorSavePath = "Assets/Pandora/MapTestAllData.txt";
        public StreamWriter errorWriter;
        public StreamWriter noErrorWriter;
        
        //singleton
        public static DebugBossMapGenerateTest _instance;
        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
            // Open File read and write
            errorWriter ??= new StreamWriter(errorSavePath, true);
            noErrorWriter ??= new StreamWriter(noErrorSavePath, true);
        }
        
        private void Start()
        {
            EventManager.Instance.AddListener(PandoraEventType.MapGenerateComplete, this);
        }

        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener(PandoraEventType.MapGenerateComplete, this);
            errorWriter?.Close();
            noErrorWriter?.Close();
        }

        public void OnEvent(PandoraEventType pandoraEventType, Component sender, object param = null)
        {
            if (pandoraEventType == PandoraEventType.MapGenerateComplete)
            {
                generateTimes++;
                var boosRoom = FindFirstObjectByType<DebugGoToBossPos>();
                var children = new List<string>();
                var rc = GameObject.Find("RoomController");
                foreach (Transform child in rc.transform) children.Add(child.gameObject.name);
                if (boosRoom != null)
                {
                    Debug.Log("Map generate " + (RoomPositionsGenerator.stage + 1) + "stage : " + generateTimes + " Complete!");
                }
                else
                {
                    // get all child
                    Debug.LogWarning("Map generate " + (RoomPositionsGenerator.stage + 1) + "stage : " + generateTimes + " Error!");
                    errorWriter.WriteLine("Map generate " + (RoomPositionsGenerator.stage + 1) + "stage : " + generateTimes + " Error!");
                    errorWriter.WriteLine("RoomController children count: " + children.Count);
                    foreach (var child in children)
                    {
                        errorWriter.WriteLine(child);
                    }
                }
                // write all data
                noErrorWriter.WriteLine("Map generate " + (RoomPositionsGenerator.stage + 1) + "stage : " + generateTimes + (boosRoom == null ? " Error!" : " Complete!"));
                noErrorWriter.WriteLine("RoomController children count: " + (RoomPositionsGenerator.stage + 1) + "stage : " + children.Count);
                foreach (var child in children)
                {
                    noErrorWriter.WriteLine(child);
                }

                // Destroy all GameObject in scene except this

                if (generateTimes < 1000)
                    SceneManager.LoadScene("BossMapGenerateTest");
                else
                {
                    Debug.Log("Map test complete!");
                }
            }
        }

        public void AddBossDenied(string str)
        {
            errorWriter.WriteLine(str);
        }
    }
}