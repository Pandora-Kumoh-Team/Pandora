using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Pandora.Scripts.System.Event;
using Pathfinding;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Pandora.Scripts.NewDungeon.Rooms
{
    public class Room : MonoBehaviour, IEventListener
    {
        public int Width = 17;
        public int Height = 9;
        public int X;
        public int Y;
        public Door leftDoor;
        public Door rightDoor;
        public Door topDoor;
        public Door bottomDoor;
        public List<Door> doors = new List<Door>();

        protected bool isClear = false;
        
        private Coroutine _scanCoroutine;

        private  void Awake()
        {
            EventManager.Instance.AddListener(PandoraEventType.MapGenerateComplete, this);
        
            StartCoroutine(LateAwake());
        }

        private IEnumerator LateAwake()
        {
            yield return null;
            yield return null;
        
            Door[] ds = GetComponentsInChildren<Door>();
            foreach(Door d in ds)
            {
                doors.Add(d);
                switch(d.doorType)
                {
                    case Door.DoorType.right:
                        rightDoor = d;
                        break;
                    case Door.DoorType.left:
                        leftDoor = d;
                        break;
                    case Door.DoorType.bottom:
                        bottomDoor = d;
                        break;
                    case Door.DoorType.top:
                        topDoor = d;
                        break;
                }
            }
            RoomController.Instance.RegisterRoom(this);
        }

        private void OnDestroy()
        {
            EventManager.Instance.RemoveListener(PandoraEventType.MapGenerateComplete, this);
        }

        public void RemoveUnconnectedDoors()
        {
            var doorsToRemove = new List<Door>();
            foreach(Door door in doors)
            {
                switch(door.doorType)
                {
                    case Door.DoorType.right: 
                        if(GetRight() == null)
                        {
                            door.DisableDoor();
                            doorsToRemove.Add(door);
                        }
                        break;
                    case Door.DoorType.left:
                        if (GetLeft() == null)
                        {
                            door.DisableDoor();
                            doorsToRemove.Add(door);
                        }
                        break;
                    case Door.DoorType.top:
                        if (GetTop() == null)
                        {
                            door.DisableDoor();
                            doorsToRemove.Add(door);
                        }
                        break;
                    case Door.DoorType.bottom:
                        if (GetBottom() == null)
                        {
                            door.DisableDoor();
                            doorsToRemove.Add(door);
                        }
                        break;
                }
            }
            foreach(Door door in doorsToRemove)
            {
                doors.Remove(door);
            }
        }

        public Room GetRight()
        {
            if(RoomController.Instance.DoesRoomExist(X + 1, Y))
            {
                return RoomController.Instance.FindRoom(X + 1, Y);
            }
            return null;
        }

        public Room GetLeft()
        {
            if (RoomController.Instance.DoesRoomExist(X - 1, Y))
            {
                return RoomController.Instance.FindRoom(X - 1, Y);
            }
            return null;
        }
        public Room GetTop()
        {
            if (RoomController.Instance.DoesRoomExist(X, Y + 1))
            {
                return RoomController.Instance.FindRoom(X, Y + 1);
            }
            return null;
        }
        public Room GetBottom()
        {
            if (RoomController.Instance.DoesRoomExist(X, Y - 1))
            {
                return RoomController.Instance.FindRoom(X, Y - 1);
            }
            return null;
        }

        private void OnDrawGizmos()
        {
            // Gizmos.color = Color.red;
            // Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
        }

        public Vector3 GetRoomCenter()
        {
            return new Vector3(X * Width, Y * Height);
        }

        public virtual void OnPlayerEnter(GameObject playerObject)
        {
            var brain = Camera.main.GetComponent<CinemachineBrain>();
            var vcam = brain.ActiveVirtualCamera as CinemachineVirtualCamera;
            var confinder = vcam.GetComponent<CinemachineConfiner2D>();
            confinder.m_BoundingShape2D = transform.Find("EnterCollider").GetComponent<CompositeCollider2D>();
        }
        
        public virtual void OnPlayerExit(GameObject playerObject)
        {
            // Nothing
        }
        
        public virtual void OnClearRoom()
        {
            isClear = true;
            OpenAllDoors();
        }

        public void OnEvent(PandoraEventType pandoraEventType, Component sender, object param = null)
        {
            if (pandoraEventType == PandoraEventType.MapGenerateComplete)
            {
                if(GetRight() != null)
                    doors.Add(GetRight().leftDoor);
                if (GetLeft() != null)
                    doors.Add(GetLeft().rightDoor);
                if (GetTop() != null)
                    doors.Add(GetTop().bottomDoor);
                if (GetBottom() != null)
                    doors.Add(GetBottom().topDoor);
                transform.Find("EnterCollider").gameObject.SetActive(true);
                OnMapGenerateComplete();
            }
        }
        
        protected virtual void OnMapGenerateComplete()
        {
            // Nothing
        }
        
        public void OpenAllDoors()
        {
            foreach(Door door in doors)
            {
                door.OpenDoor();
            }
            if(_scanCoroutine != null)
                StopCoroutine(_scanCoroutine);
            _scanCoroutine = StartCoroutine(ScanAsync());
        }
        
        public void CloseAllDoors()
        {
            foreach(Door door in doors)
            {
                door.CloseDoor();
            }
            if(_scanCoroutine != null)
                StopCoroutine(_scanCoroutine);
            _scanCoroutine = StartCoroutine(ScanAsync());
        }
        
        private IEnumerator ScanAsync()
        {
            var graphToScan = AstarPath.active.data.gridGraph;
            foreach (var progress in AstarPath.active.ScanAsync(graphToScan)) {
                yield return null;
            }
        }
    }
}
