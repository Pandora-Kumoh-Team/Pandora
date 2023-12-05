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
        }

        public void OpenDoor()
        {
            transform.Find("Door").gameObject.SetActive(false);
            transform.Find("Wall").gameObject.SetActive(false);
        }

        public void CloseDoor()
        {
            transform.Find("Door").gameObject.SetActive(true);
            transform.Find("Wall").gameObject.SetActive(true);
        }
    }
}