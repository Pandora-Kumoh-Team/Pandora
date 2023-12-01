using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
