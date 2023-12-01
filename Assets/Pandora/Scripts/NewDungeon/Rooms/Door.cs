using System.Collections;
using UnityEngine;

namespace Pandora.Scripts.NewDungeon.Rooms
{
    public class Door : MonoBehaviour
    {
        public enum DoorType
        {
            left, right, top, bottom
        }

        public DoorType doorType;
    
        public void DisableDoor()
        {
            transform.Find("Door").gameObject.SetActive(false);
            transform.Find("Wall").gameObject.SetActive(true);
            var graphToScan = AstarPath.active.data.gridGraph;
            StartCoroutine(ScanAsync());
        }

        public void OpenDoor()
        {
            transform.Find("Door").gameObject.SetActive(false);
            transform.Find("Wall").gameObject.SetActive(false);
            var graphToScan = AstarPath.active.data.gridGraph;
            StartCoroutine(ScanAsync());
        }

        public void CloseDoor()
        {
            transform.Find("Door").gameObject.SetActive(true);
            transform.Find("Wall").gameObject.SetActive(true);
            StartCoroutine(ScanAsync());
        }
        
        private IEnumerator ScanAsync()
        {
            foreach (var progress in AstarPath.active.ScanAsync()) {
                Debug.Log("Scanning... " + progress.description + " - " + (progress.progress*100).ToString("0") + "%");
                yield return null;
            }
        }
    }
}
