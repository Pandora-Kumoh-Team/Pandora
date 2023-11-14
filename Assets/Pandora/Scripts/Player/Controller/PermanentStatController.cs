using System.IO;
using UnityEngine;

namespace Pandora.Scripts.Player.Controller
{
    public class PermanentStatController : MonoBehaviour
    {
        // json file which save permanent stats
        private const string PermanentStatsFile = "permanentStats.json";
        
        
        // permanent stats
        private PlayerCurrentStat _permanentCurrentStats;
        
        // save permanent stats
        public void SavePermanentStats()
        {
            var json = JsonUtility.ToJson(_permanentCurrentStats);
            File.WriteAllText(Application.persistentDataPath + "/" + PermanentStatsFile, json);
        }
        
        // load permanent stats
        public void LoadPermanentStats()
        {
            if (File.Exists(Application.persistentDataPath + "/" + PermanentStatsFile))
            {
                var json = File.ReadAllText(Application.persistentDataPath + "/" + PermanentStatsFile);
                _permanentCurrentStats = JsonUtility.FromJson<PlayerCurrentStat>(json);
            }
            else
            {
                _permanentCurrentStats = new PlayerCurrentStat();
            }
        }
    }
}